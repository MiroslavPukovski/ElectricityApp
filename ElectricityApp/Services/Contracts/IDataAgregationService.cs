using ElectricityApp.Classes;
using ElectricityApp.Classes.api;

namespace ElectricityApp.Services.Contracts
{
    public interface IDataAgregationService
    {
        public Task<Result<string>> DifferenceConsumedAndProduced();
    }
}
