using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Modeli;

namespace Domain.Servisi
{
    public interface IProdajaServis
    {
        (bool ok, string poruka, FiskalniRacun? racun) Prodaj(Guid ljubimacId, Korisnik prodavac);
    }
}
