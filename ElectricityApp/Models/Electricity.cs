using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace ElectricityApp.Models
{
         
    public class Electricity
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
        public DateTime PL_T { get; set; } = DateTime.Now;
        [Column("P-")]
        [Name("P-")]
        public double? produced { get; set; } 
        public double? producedAndConsumed { get; set; }
    }

    public sealed class ElectricityMap : ClassMap<Electricity>
    {
        public ElectricityMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.Id).Ignore();
            Map(m=> m.producedAndConsumed).Ignore();
        }
    }

}
