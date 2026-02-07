using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;

namespace Database.Repozitorijumi
{
    public class FiskalniRacuniRepozitorijum : IFiskalniRacuniRepozitorijum
    {
        private readonly IBazaPodataka _baza;

        public FiskalniRacuniRepozitorijum(IBazaPodataka baza)
        {
            _baza = baza;
        }

        public List<FiskalniRacun> VratiSve()
            => _baza.Tabele.FiskalniRacuni;

        public void Dodaj(FiskalniRacun racun)
        {
            if (racun.Id == Guid.Empty)
                racun.Id = Guid.NewGuid();

            _baza.Tabele.FiskalniRacuni.Add(racun);
        }

        public bool Sacuvaj() => _baza.SacuvajPromene();
    }
}

