using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;
using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IKucniLjubimciServis
    {
        (bool ok, string poruka) Dodaj(string latinskiNaziv, string ime, TipLjubimca tip, decimal prodajnaCena);
        List<KucniLjubimac> VratiSve();
        List<KucniLjubimac> VratiNeprodate();
    }
}

