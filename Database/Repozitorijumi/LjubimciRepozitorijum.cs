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
    public class LjubimciRepozitorijum : ILjubimciRepozitorijum
    {
        private readonly IBazaPodataka _baza;

        public LjubimciRepozitorijum(IBazaPodataka baza)
        {
            _baza = baza;
        }

        public List<KucniLjubimac> VratiSve()
            => _baza.Tabele.Ljubimci;

        public List<KucniLjubimac> VratiNeprodate()
            => _baza.Tabele.Ljubimci.Where(l => !l.Prodat).ToList();

        public KucniLjubimac? PronadjiPoId(Guid id)
            => _baza.Tabele.Ljubimci.FirstOrDefault(l => l.Id == id);

        public void Dodaj(KucniLjubimac ljubimac)
        {
            if (ljubimac.Id == Guid.Empty)
                ljubimac.Id = Guid.NewGuid();

            _baza.Tabele.Ljubimci.Add(ljubimac);
        }

        public bool Sacuvaj() => _baza.SacuvajPromene();
    }
}

