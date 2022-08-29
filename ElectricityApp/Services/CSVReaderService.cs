using CsvHelper;
using ElectricityApp.Models;
using System.Globalization;
using System.Net;
using CsvHelper.Configuration;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient.Server;
using Microsoft.Data.SqlClient;

namespace ElectricityApp.Services
{
    public class CSVReaderService : IHostedService, IDisposable
    {

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly int _runEveryMinutes;

        private Timer _timer;
        private ILogger<CSVReaderService> _logeer;

        private string[] _downloadLinks = new string[] 
        {
            "https://data.gov.lt/dataset/1975/download/10766/2022-05.csv",
            "https://data.gov.lt/dataset/1975/download/10765/2022-04.csv"
        };     
        private readonly string _csvFileFolder = @$"{AppDomain.CurrentDomain.BaseDirectory}\ElectricityApp\";

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
                var dbContext = serviseScope.ServiceProvider.GetRequiredService<WebDatabaseContext>();

                var oldRecors = dbContext.electricityDB.ToList();

                foreach(var oldRecor in oldRecors)
                {
                    dbContext.electricityDB.Remove(oldRecor);
                }
                


                var ElectricityEntities = new List<ElectricityModel>();
                var csvFilesPath = FindCsvFiles(_csvFileFolder);
                var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);
                csvConfiguration.Delimiter = ",";

                _logeer.LogInformation("succesfuly read CV files");

                foreach (var csvFilePath in csvFilesPath)
                {
                    using (var reader = new StreamReader(csvFilePath))
                    using (var csv = new CsvReader(reader, csvConfiguration))
                    {
                        csv.Context.RegisterClassMap<ElectricityModelMap>();
                        var records = csv.GetRecords<ElectricityModel>();

                        foreach (var record in records)
                        {
                            if (record.OBT_PAVADINIMAS == "Namas" && record.consumed <= 1)
                            {
                                ElectricityEntities.Add(new ElectricityModel
                                {
                                    OBJ_NUMERIS = record.OBJ_NUMERIS,
                                    TINKLAS = record.TINKLAS,
                                    OBT_PAVADINIMAS = record.OBT_PAVADINIMAS,
                                    OBJ_GV_TIPAS = record.OBJ_GV_TIPAS,
                                    consumed = record.consumed,
                                    PL_T = record.PL_T,
                                    produced = record.produced
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
                _logeer.LogInformation("succesfuly Save Changes");
            }
        }


        

        //Scrape file from links
        public async Task CSVFileScraper()
        {
            foreach (var link in _downloadLinks)
            {
                Directory.CreateDirectory($@"{AppDomain.CurrentDomain.BaseDirectory}\ElectricityApp");

                using WebClient client = new WebClient();
                byte[] buffer = client.DownloadData(link);

                var dateOfData = link.Replace("https://data.gov.lt/dataset/1975/download/", string.Empty);

                if (dateOfData.Contains("/"))
                {
                    dateOfData = dateOfData.Split("/")[1];
                }

                string filePath = $@"{AppDomain.CurrentDomain.BaseDirectory}\ElectricityApp\Data_{dateOfData}";

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
