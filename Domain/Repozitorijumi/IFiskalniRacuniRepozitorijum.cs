using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Modeli;

namespace Domain.Repozitorijumi
{
    public interface IFiskalniRacuniRepozitorijum
    {
        List<FiskalniRacun> VratiSve();
        void Dodaj(FiskalniRacun racun);
        bool Sacuvaj();
    }
}
