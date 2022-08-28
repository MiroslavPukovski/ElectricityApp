using ElectricityApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectricityApp.Classes
{
    public class ElectricityDto
    {
        public int OBJ_NUMERIS { get; init; }
        public string TINKLAS { get; init; }
        public string OBT_PAVADINIMAS { get; init; }
        public string OBJ_GV_TIPAS { get; init; }
        public double? consumed { get; init; }
        public DateTime PL_T { get; init; }
        public double? produced { get; init; }


        public ElectricityDto(ElectricityModel electricityDB)
        {
            OBJ_NUMERIS = electricityDB.OBJ_NUMERIS;
            TINKLAS = electricityDB.TINKLAS;
            OBT_PAVADINIMAS = electricityDB.OBT_PAVADINIMAS;
            OBJ_GV_TIPAS = electricityDB.OBJ_GV_TIPAS;
            consumed = electricityDB.consumed;
            PL_T = electricityDB.PL_T;
            produced = electricityDB.produced;
        }
    }
}
