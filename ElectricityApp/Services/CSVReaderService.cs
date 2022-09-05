using CsvHelper;
using ElectricityApp.Models;
using System.Globalization;
using CsvHelper.Configuration;
using ElectricityApp.Data;

namespace ElectricityApp.Services
{
    public class CSVReaderService : IHostedService, IDisposable
    {
        private readonly string _csvFileFolder = @$"{AppDomain.CurrentDomain.BaseDirectory}\ElectricityCsvFiles\";
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly int _runEveryMinutes;

        private Timer _timer;
        private ILogger<CSVReaderService> _logeer;

        private string[] _downloadLinks = new string[] 
        {
            "https://data.gov.lt/dataset/1975/download/10766/2022-05.csv",
            "https://data.gov.lt/dataset/1975/download/10765/2022-04.csv"
        };     

        public CSVReaderService(IServiceScopeFactory serviceScopeFactory, ILogger<CSVReaderService> logeer)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _runEveryMinutes = 60;
            _logeer = logeer;
        }

        //Read csv and drop to DB
        public async Task CsvReadAndPushToDb()
        {
            await CSVFileScraper();

            using (var serviseScope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = serviseScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var oldRecords = dbContext.Electricities.ToList();

                foreach (var oldRecord in oldRecords)
                {
                    dbContext.Electricities.Remove(oldRecord);
                }

                var ElectricityEntities = new List<Electricity>();
                var csvFilesPath = FindCsvFiles(_csvFileFolder);
                var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
                csvConfiguration.Delimiter = ",";

                _logeer.LogInformation("succesfuly read CV files");

                foreach (var csvFilePath in csvFilesPath)
                {
                    using (var reader = new StreamReader(csvFilePath))
                    using (var csv = new CsvReader(reader, csvConfiguration))
                    {
                        csv.Context.RegisterClassMap<ElectricityMap>();
                        var records = csv.GetRecords<Electricity>();

                        foreach (var record in records)
                        {
                            if (record.OBT_PAVADINIMAS == "Namas" && record.consumed <= 1)
                            {
                                ElectricityEntities.Add(new Electricity
                                {
                                    OBJ_NUMERIS = record.OBJ_NUMERIS,
                                    TINKLAS = record.TINKLAS,
                                    OBT_PAVADINIMAS = record.OBT_PAVADINIMAS,
                                    OBJ_GV_TIPAS = record.OBJ_GV_TIPAS,
                                    consumed = (double)(record.consumed ?? 0),
                                    PL_T = record.PL_T,
                                    produced = (double)(record.produced ?? 0),
                                    producedAndConsumed = record.consumed - record.produced //difference between generated and consumed data
                                });
                            }
                        }
                        try
                        {
                            await dbContext.AddRangeAsync(ElectricityEntities);
                            _logeer.LogInformation("succesfuly upload data to DB");
                        }
                        catch(Exception e)
                        {
                            _logeer.LogWarning($"Adding to DB failure, info: {e.Message}");
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                await dbContext.SaveChangesAsync();
                _logeer.LogInformation("succesfuly Save Changes To DB");
            }
        }

        //Scrape file from links
        public async Task CSVFileScraper()
        {
            foreach (var link in _downloadLinks)
            {
                Directory.CreateDirectory($@"{AppDomain.CurrentDomain.BaseDirectory}\ElectricityCsvFiles");

                using HttpClient client = new HttpClient();
                byte[] buffer = await client.GetByteArrayAsync(link);
                

                var dateOfData = link.Replace("https://data.gov.lt/dataset/1975/download/", string.Empty);

                if (dateOfData.Contains("/"))
                {
                    dateOfData = dateOfData.Split("/")[1];
                }

                string filePath = $@"{AppDomain.CurrentDomain.BaseDirectory}\ElectricityCsvFiles\Data_{dateOfData}";

                Stream stream = new FileStream(filePath, FileMode.Create);
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write(buffer);
                stream.Close();
            }  
        }

        //Find all csv files
        public string[] FindCsvFiles(string directory)
        {
            var csvFilesInfo = new DirectoryInfo(_csvFileFolder).GetFiles("*.csv");
            var csvFilesPath = new string[csvFilesInfo.Length];

            for (int i = 0; i < csvFilesInfo.Length; i++)
            {
                csvFilesPath[i] = csvFilesInfo[i].FullName;
            }

            return csvFilesPath;
        }

        //Background services, scrape files every hour
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(async (state) => await CsvReadAndPushToDb(), null, TimeSpan.Zero, TimeSpan.FromMinutes(_runEveryMinutes));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
