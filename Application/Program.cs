using Database.BazaPodataka;
using Database.Repozitorijumi;
using Domain.BazaPodataka;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Presentation.Authentifikacija;
using Presentation.Meni;
using Services.AutenftikacioniServisi;
using Services.ProdajaServisi;
using Services.KucniLjubimciServisi;
using Services.LoggerServisi;

namespace Loger_Bloger
{
    public class Program
    {
        public static void Main()
        {
            // Baza podataka
            //IBazaPodataka bazaPodataka = null; // TODO: Initialize the database with appropriate implementation
            IBazaPodataka bazaPodataka = new JsonBazaPodataka();
            bazaPodataka.Ucitaj();

            // Repozitorijumi
            IKorisniciRepozitorijum korisniciRepozitorijum = new KorisniciRepozitorijum(bazaPodataka);
            ILjubimciRepozitorijum ljubimciRepo = new LjubimciRepozitorijum(bazaPodataka);
            IFiskalniRacuniRepozitorijum racuniRepo = new FiskalniRacuniRepozitorijum(bazaPodataka);

            ILoggerServis logger = new FileLoggerServis(Path.Combine("logs", "app.log"));
            logger.Log("Aplikacija startovana");

            // Servisi
            IAutentifikacijaServis autentifikacijaServis = new AutentifikacioniServis(korisniciRepozitorijum, logger); // TODO: Pass necessary dependencies

            // Smena -> izbor implementacije prodaje (Day/Night)
            // Ako je van radnog vremena, prodajaServis ce biti null (sutra u meniju to hendlujes)
            IProdajaServis? prodajaServis = null;
            try
            {
                prodajaServis = KreirajProdajaServis(ljubimciRepo, racuniRepo, logger);
            }
            catch
            {
                prodajaServis = null;
            }

            // Prezentacioni sloj
            AutentifikacioniMeni am = new AutentifikacioniMeni(autentifikacijaServis);
            Korisnik prijavljen = new Korisnik();

            while (am.TryLogin(out prijavljen) == false)
            {
                Console.WriteLine("Pogrešno korisničko ime ili lozinka. Pokušajte ponovo.");
            }

            Console.Clear();
            Console.WriteLine($"Uspešno ste prijavljeni kao: {prijavljen.ImePrezime} ({prijavljen.Uloga})");

            if (prodajaServis == null)
            {
                logger.LogUpozorenje("Prodaja servis nije dostupan (verovatno van radnog vremena 08-22).");
            }

            // prosledi zavisnosti koje OpcijeMeni traži
            OpcijeMeni meni = new OpcijeMeni(ljubimciRepo, racuniRepo, prodajaServis!, logger);

            // prosledi ulogovanog korisnika
            meni.PrikaziMeni(prijavljen);

        }

        // "Smena" izbor implementacije - OVDE JE PRAVO MESTO
        private static IProdajaServis KreirajProdajaServis(
            ILjubimciRepozitorijum ljubimciRepo,
            IFiskalniRacuniRepozitorijum racuniRepo,
            ILoggerServis logger)
        {
            var t = DateTime.Now.TimeOfDay;

            // Dnevna smena: 08:00 - 16:00
            if (t >= new TimeSpan(8, 0, 0) && t < new TimeSpan(16, 0, 0))
                return new DnevniProdajaServis(ljubimciRepo, racuniRepo, logger);

            // Nocna smena: 16:00 - 22:00
            if (t >= new TimeSpan(16, 0, 0) && t < new TimeSpan(22, 0, 0))
                return new NocniProdajaServis(ljubimciRepo, racuniRepo, logger);

            // Van radnog vremena
            throw new InvalidOperationException("Van radnog vremena (08–22).");
        }
    }
}
