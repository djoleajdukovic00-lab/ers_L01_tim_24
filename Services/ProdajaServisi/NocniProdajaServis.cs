using System;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.ProdajaServisi
{
    public class NocniProdajaServis : IProdajaServis
    {
        private readonly ILjubimciRepozitorijum _ljubimciRepo;
        private readonly IFiskalniRacuniRepozitorijum _racuniRepo;
        private readonly ILoggerServis _logger;

        public NocniProdajaServis(
            ILjubimciRepozitorijum ljubimciRepo,
            IFiskalniRacuniRepozitorijum racuniRepo,
            ILoggerServis logger)
        {
            _ljubimciRepo = ljubimciRepo;
            _racuniRepo = racuniRepo;
            _logger = logger;
        }

        public (bool ok, string poruka, FiskalniRacun? racun) Prodaj(Guid ljubimacId, Korisnik prodavac)
        {
            try
            {
                _logger.Log($"[NOCNA] Pocetak prodaje: ljubimacId={ljubimacId}, prodavac={prodavac.ImePrezime}");

                var ljubimac = _ljubimciRepo.PronadjiPoId(ljubimacId);
                if (ljubimac == null)
                {
                    _logger.LogUpozorenje($"[NOCNA] Neuspesna prodaja: ljubimacId={ljubimacId} ne postoji.");
                    return (false, "Ljubimac ne postoji.", null);
                }

                if (ljubimac.Prodat)
                {
                    _logger.LogUpozorenje($"[NOCNA] Neuspesna prodaja: ljubimacId={ljubimacId} je vec prodat.");
                    return (false, "Ljubimac je vec prodat.", null);
                }

                // nocna smena: +10% porez
                decimal ukupno = ljubimac.ProdajnaCena * 1.10m;

                ljubimac.Prodat = true;
                _ljubimciRepo.Sacuvaj();

                var racun = new FiskalniRacun
                {
                    Id = Guid.NewGuid(),
                    ImeProdavca = prodavac.ImePrezime,
                    DatumVreme = DateTime.Now,
                    UkupanIznos = ukupno
                };

                _racuniRepo.Dodaj(racun);
                _racuniRepo.Sacuvaj();

                _logger.Log($"[NOCNA] Prodaja uspesna: racunId={racun.Id}, ukupno={ukupno}");
                return (true, "Prodaja uspesna (nocna smena +10%).", racun);
            }
            catch (Exception ex)
            {
                _logger.LogGreska($"[NOCNA] Greska pri prodaji ljubimacId={ljubimacId}: {ex.Message}");
                return (false, "Doslo je do greske pri prodaji.", null);
            }
        }
    }
}
