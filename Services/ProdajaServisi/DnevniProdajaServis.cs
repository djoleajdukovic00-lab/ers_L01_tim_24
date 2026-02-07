using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.ProdajaServisi
{
    public class DnevniProdajaServis : IProdajaServis
    {
        private readonly ILjubimciRepozitorijum _ljubimciRepo;
        private readonly IFiskalniRacuniRepozitorijum _racuniRepo;
        private readonly ILoggerServis _logger;


        public DnevniProdajaServis(ILjubimciRepozitorijum ljubimciRepo, IFiskalniRacuniRepozitorijum racuniRepo, ILoggerServis logger)
        {
            _ljubimciRepo = ljubimciRepo;
            _racuniRepo = racuniRepo;
            _logger = logger;
        }

        public (bool ok, string poruka, FiskalniRacun? racun) Prodaj(Guid ljubimacId, Korisnik prodavac)
        {
            try
            {
                _logger.Log($"[DNEVNA] Pocetak prodaje: ljubimacId={ljubimacId}, prodavac={prodavac.ImePrezime}");

                var ljubimac = _ljubimciRepo.PronadjiPoId(ljubimacId);
                if (ljubimac == null)
                {
                    _logger.LogUpozorenje($"[DNEVNA] Neuspesna prodaja: ljubimacId={ljubimacId} ne postoji.");
                    return (false, "Ljubimac ne postoji.", null);
                }

                if (ljubimac.Prodat)
                {
                    _logger.LogUpozorenje($"[DNEVNA] Neuspesna prodaja: ljubimacId={ljubimacId} je vec prodat.");
                    return (false, "Ljubimac je vec prodat.", null);
                }

                // dnevna smena: 15% popust
                decimal ukupno = ljubimac.ProdajnaCena * 0.85m;

                // obeleži prodat
                ljubimac.Prodat = true;
                _ljubimciRepo.Sacuvaj();

                // kreiraj fiskalni racun
                var racun = new FiskalniRacun
                {
                    Id = Guid.NewGuid(),
                    ImeProdavca = prodavac.ImePrezime,
                    DatumVreme = DateTime.Now,
                    UkupanIznos = ukupno
                };

                _racuniRepo.Dodaj(racun);
                _racuniRepo.Sacuvaj();

                _logger.Log($"[DNEVNA] Prodaja uspesna: racunId={racun.Id}, ukupno={ukupno}");
                return (true, "Prodaja uspesna (dnevna smena -15%).", racun);
            }
            catch (Exception ex)
            {
                _logger.LogGreska($"[DNEVNA] Greska pri prodaji ljubimacId={ljubimacId}: {ex.Message}");
                return (false, "Doslo je do greske pri prodaji.", null);
            }
        }

    }
}

