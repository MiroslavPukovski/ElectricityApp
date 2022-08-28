using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using ElectricityApp.Classes;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace ElectricityApp.Models
{
         [Table("ElectricityData")]
        public class ElectricityModel
        {
            [Key]
            public int Id { get; set; }
             public int OBJ_NUMERIS { get; set; }
            public string TINKLAS { get; set; }
            public string OBT_PAVADINIMAS { get; set; }
            public string OBJ_GV_TIPAS { get; set; }

            [Column("P+")]
            [Name("P+")]
            public double? consumed { get; set; }
            public DateTime PL_T { get; set; }

            [Column("P-")]
            [Name("P-")]
            public double? produced { get; set; }
        }


    public sealed class ElectricityModelMap : ClassMap<ElectricityModel>
    {
        public ElectricityModelMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.Id).Ignore();
        }
    }

}
