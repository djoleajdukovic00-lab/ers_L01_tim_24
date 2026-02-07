using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class FiskalniRacun
    {
        public Guid Id { get; set; }
        public string ImeProdavca { get; set; } = string.Empty;
        public DateTime DatumVreme { get; set; }
        public decimal UkupanIznos { get; set; }
    }
}
