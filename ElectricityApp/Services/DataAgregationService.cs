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



        public DataAgregationService(WebDatabaseContext dbContext)
        {
            _dbContext = dbContext;
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

                return new Result<string>(result);

            }
            catch (Exception ex)
            {
                return new Result<string>(ex.Message, null);
            }
        }


    }
}
