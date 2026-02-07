using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.KucniLjubimciServisi
{
    public class KucniLjubimciServis : IKucniLjubimciServis
    {
        private readonly ILjubimciRepozitorijum _ljubimciRepo;

        public KucniLjubimciServis(ILjubimciRepozitorijum ljubimciRepo)
        {
            _ljubimciRepo = ljubimciRepo;
        }

        public (bool ok, string poruka) Dodaj(string latinskiNaziv, string ime, TipLjubimca tip, decimal prodajnaCena)
        {
            if (string.IsNullOrWhiteSpace(latinskiNaziv) || string.IsNullOrWhiteSpace(ime))
                return (false, "Naziv i ime su obavezni.");

            if (prodajnaCena <= 0)
                return (false, "Cena mora biti veca od 0.");

            // U zadatku: max 10 ljubimaca u prodavnici (u radnji = neprodatih)
            int neprodatih = _ljubimciRepo.VratiNeprodate().Count;
            if (neprodatih >= 10)
                return (false, "U prodavnici ne moze biti vise od 10 neprodatih ljubimaca.");

            var ljubimac = new KucniLjubimac
            {
                Id = Guid.NewGuid(),
                LatinskiNaziv = latinskiNaziv.Trim(),
                Ime = ime.Trim(),
                Tip = tip,
                ProdajnaCena = prodajnaCena,
                Prodat = false
            };

            _ljubimciRepo.Dodaj(ljubimac);
            _ljubimciRepo.Sacuvaj();

            return (true, "Ljubimac je dodat.");
        }

        public List<KucniLjubimac> VratiSve() => _ljubimciRepo.VratiSve();

        public List<KucniLjubimac> VratiNeprodate() => _ljubimciRepo.VratiNeprodate();
    }
}
