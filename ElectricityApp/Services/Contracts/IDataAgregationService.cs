using ElectricityApp.Classes;
using ElectricityApp.Classes.api;

namespace ElectricityApp.Services.Contracts
{
    public interface IDataAgregationService
    {
        public Task<Result<string>> DifferenceConsumedAndProduced();
        public Task<Result<List<ElectricityDto>>> GetAllAgregatedData();
        public Task<Result<List<ElectricityDto>>> GetAgregatedDataByPage(int pageNr = 1, int pageSize = 20);
    }
}
