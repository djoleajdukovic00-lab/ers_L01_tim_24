using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.AutenftikacioniServisi
{
    public class AutentifikacioniServis : IAutentifikacijaServis
    {
        private readonly IKorisniciRepozitorijum _korisniciRepo;
        private readonly ILoggerServis _logger;

        // TODO: Add necessary dependencies (e.g., user repository) via dependency injection
        public AutentifikacioniServis(IKorisniciRepozitorijum korisniciRepo, ILoggerServis logger)
        {
            _korisniciRepo = korisniciRepo;
            _logger = logger;
        }

        // TODO: Implement login method
        public (bool, Korisnik) Prijava(string korisnickoIme, string lozinka)
        {
            try
            {
                // osnovna validacija
                if (string.IsNullOrWhiteSpace(korisnickoIme) || string.IsNullOrWhiteSpace(lozinka))
                {
                    _logger.LogUpozorenje("Prijava neuspesna: prazno korisnicko ime ili lozinka.");
                    return (false, new Korisnik());
                }

                _logger.Log($"Pokusaj prijave: {korisnickoIme}");

                // Kod tebe repo vraća "prazan Korisnik" ako ne postoji (KorisnickoIme == string.Empty)
                Korisnik korisnik = _korisniciRepo.PronadjiKorisnikaPoKorisnickomImenu(korisnickoIme);

                if (korisnik == null || korisnik.KorisnickoIme == string.Empty)
                {
                    _logger.LogUpozorenje($"Prijava neuspesna: korisnik '{korisnickoIme}' ne postoji.");
                    return (false, new Korisnik());
                }

                if (korisnik.Lozinka != lozinka)
                {
                    _logger.LogUpozorenje($"Prijava neuspesna: pogresna lozinka za korisnika '{korisnickoIme}'.");
                    return (false, new Korisnik());
                }

                _logger.Log($"Prijava uspesna: {korisnickoIme}");
                return (true, korisnik);
            }
            catch (Exception ex)
            {
                _logger.LogGreska($"Greska u prijavi za '{korisnickoIme}': {ex.Message}");
                return (false, new Korisnik());
            }
        }
    }
}
