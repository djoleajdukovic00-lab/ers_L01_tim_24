using Domain.Enumeracije;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Modeli
{
    public class KucniLjubimac
    {
        public Guid Id { get; set; }
        public string LatinskiNaziv { get; set; } = string.Empty;
        public string Ime { get; set; } = string.Empty;
        public TipLjubimca Tip { get; set; }
        public decimal ProdajnaCena { get; set; }
        public bool Prodat { get; set; }
    }
}
