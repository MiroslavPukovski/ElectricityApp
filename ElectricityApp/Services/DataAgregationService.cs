using ElectricityApp.Classes;
using ElectricityApp.Classes.api;
using ElectricityApp.Models;
using ElectricityApp.Services.Contracts;
using Newtonsoft.Json.Linq;
using NuGet.Protocol.Plugins;
using System.Collections.Generic;

namespace ElectricityApp.Services
{
    public class DataAgregationService : IDataAgregationService
    {
        private readonly WebDatabaseContext _dbContext;
        private ILogger<DataAgregationService> _logeer;


        public DataAgregationService(WebDatabaseContext dbContext, ILogger<DataAgregationService> logeer)
        {
            _dbContext = dbContext;
            _logeer = logeer;
        }



        public async Task<Result<string>> DifferenceConsumedAndProduced()
        {
            try
            {
                var electricity = _dbContext.electricityDB.ToList();

                if(electricity.Count() == 0)
                {
                    return new Result<string>("No data found!", null);
                }

                var consumedSum = electricity.Select(x => x.consumed).Sum();
                var producedSum = electricity.Select(x => x.produced).Sum();

                string result = $"Consumed: {String.Format("{0:0.00}", consumedSum)}   Produced: {String.Format("{0:0.00}", producedSum)}";

                _logeer.LogInformation("succesfuly Get Data From DB");
                return new Result<string>(result);

            }
            catch (Exception ex)
            {
                _logeer.LogWarning($"DifferenceConsumedAndProduced method failure, info: {ex.Message}");
                return new Result<string>(ex.Message, null);
            }
        }


        public async Task<Result<List<ElectricityDto>>> GetAllAgregatedData()
        {
            try
            {
                var electricityData = _dbContext.electricityDB.ToList();

                if(electricityData.Count() == 0)
                {
                    return new Result<List<ElectricityDto>>("No data found!", null);
                }

                var dtoElectricity = new List<ElectricityDto>();

                foreach(var data in electricityData)
                {
                    dtoElectricity.Add(new ElectricityDto(data));
                }

                _logeer.LogInformation("succesfuly Get Data From DB");
                return new Result<List<ElectricityDto>>(dtoElectricity);
            }
            catch(Exception ex)
            {
                _logeer.LogWarning($"GetAllAgregatedData method failure, info: {ex.Message}");
                return new Result<List<ElectricityDto>>(ex.Message, null);
            }
        }


        public async Task<Result<List<ElectricityDto>>> GetAgregatedDataByPage(int pageNr = 1, int pageSize = 20)
        {
            try
            {
                var electricityData = _dbContext.electricityDB
                    .Skip((pageNr - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                if (electricityData.Count() == 0)
                {
                    return new Result<List<ElectricityDto>>("No data found!", null);
                }

                var dtoElectricity = new List<ElectricityDto>();


                foreach (var data in electricityData)
                {

                    dtoElectricity.Add(new ElectricityDto(data));
                }

                _logeer.LogInformation("succesfuly Get Data From DB");
                return new Result<List<ElectricityDto>>(dtoElectricity);
            }
            catch (Exception ex)
            {
                _logeer.LogWarning($"GetAgregatedDataByPage method failure, info: {ex.Message}");
                return new Result<List<ElectricityDto>>(ex.Message, null);
            }
        }

    }
}
