using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Presentation.Meni
{
    public class OpcijeMeni
    {
        private readonly ILjubimciRepozitorijum _ljubimciRepo;
        private readonly IFiskalniRacuniRepozitorijum _racuniRepo;
        private readonly IProdajaServis _prodajaServis;
        private readonly ILoggerServis _logger;

        public OpcijeMeni(
            ILjubimciRepozitorijum ljubimciRepo,
            IFiskalniRacuniRepozitorijum racuniRepo,
            IProdajaServis prodajaServis,
            ILoggerServis logger)
        {
            _ljubimciRepo = ljubimciRepo;
            _racuniRepo = racuniRepo;
            _prodajaServis = prodajaServis;
            _logger = logger;
        }

        public void PrikaziMeni(Korisnik ulogovaniKorisnik)
        {
            bool kraj = false;

            while (!kraj)
            {
                Console.WriteLine("\n=== MENI ===");

                if (ulogovaniKorisnik.Uloga == TipKorisnika.Menadzer)
                {
                    Console.WriteLine("1. Prikaz svih ljubimaca");
                    Console.WriteLine("2. Dodaj ljubimca");
                    Console.WriteLine("3. Prikaz svih fiskalnih racuna");
                    Console.WriteLine("0. Izlaz");
                }
                else if (ulogovaniKorisnik.Uloga == TipKorisnika.Prodavac)
                {
                    Console.WriteLine("1. Prikaz neprodatih ljubimaca");
                    Console.WriteLine("2. Prodaj ljubimca");
                    Console.WriteLine("0. Izlaz");
                }

                Console.Write("Izbor: ");
                string izbor = Console.ReadLine() ?? "";

                if (ulogovaniKorisnik.Uloga == TipKorisnika.Menadzer)
                {
                    kraj = ObradiMeniMenadzera(izbor, ulogovaniKorisnik);
                }
                else
                {
                    kraj = ObradiMeniProdavca(izbor, ulogovaniKorisnik);
                }
            }
        }

        private bool ObradiMeniMenadzera(string izbor, Korisnik korisnik)
        {
            switch (izbor)
            {
                case "1":
                    PrikaziSveLjubimce();
                    break;
                case "2":
                    DodajLjubimca();
                    break;
                case "3":
                    PrikaziFiskalneRacune();
                    break;
                case "0":
                    return true;
                default:
                    Console.WriteLine("Nepoznata opcija.");
                    break;
            }
            return false;
        }

        private void PrikaziSveLjubimce()
        {
            Console.WriteLine("\n--- SVI LJUBIMCI ---");

            var svi = _ljubimciRepo.VratiSve();

            if (svi.Count == 0)
            {
                Console.WriteLine("Nema ljubimaca u sistemu.");
                return;
            }

            Console.WriteLine($"Ukupno: {svi.Count}");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("ID                                   | Ime        | Latinski naziv                | Cena     | Status");
            Console.WriteLine("--------------------------------------------------------------------------------");

            foreach (var l in svi)
            {
                string status = l.Prodat ? "PRODAT" : "NEPRODAT";
                Console.WriteLine($"{l.Id} | {l.Ime,-10} | {l.LatinskiNaziv,-28} | {l.ProdajnaCena,8} | {status}");
            }
        }


        private void DodajLjubimca()
        {
            var sviLjubimci = _ljubimciRepo.VratiSve();

            if (sviLjubimci.Count >= 10)
            {
                Console.WriteLine("Nije moguce dodati vise od 10 ljubimaca u radnju.");
                _logger.LogUpozorenje("Pokusaj dodavanja ljubimca preko limita (10).");
                return;
            }

            Console.Write("Ime ljubimca: ");
            string ime = Console.ReadLine() ?? "";

            Console.Write("Latinski naziv: ");
            string latinskiNaziv = Console.ReadLine() ?? "";

            Console.Write("Cena: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal cena) || cena <= 0)
            {
                Console.WriteLine("Neispravna cena.");
                return;
            }

            KucniLjubimac novi = new KucniLjubimac
            {
                Id = Guid.NewGuid(),
                Ime = ime,
                LatinskiNaziv = latinskiNaziv,
                ProdajnaCena = cena,
                Prodat = false
            };

            _ljubimciRepo.Dodaj(novi);
            _ljubimciRepo.Sacuvaj();

            Console.WriteLine("Ljubimac uspesno dodat.");
            _logger.Log($"Dodat ljubimac: {novi.Ime} ({novi.LatinskiNaziv})");
        }


        private bool ObradiMeniProdavca(string izbor, Korisnik korisnik)
        {
            switch (izbor)
            {
                case "1":
                    PrikaziNeprodateLjubimce();
                    break;
                case "2":
                    ProdajLjubimca(korisnik);
                    break;
                case "0":
                    return true;
                default:
                    Console.WriteLine("Nepoznata opcija.");
                    break;
            }
            return false;
        }


        private void PrikaziNeprodateLjubimce()
        {
            Console.WriteLine("\n--- NEPRODATI LJUBIMCI ---");

            var ljubimci = _ljubimciRepo.VratiSve().Where(l => !l.Prodat).ToList();


            if (ljubimci.Count == 0)
            {
                Console.WriteLine("Nema neprodatih ljubimaca.");
                return;
            }

            foreach (var l in ljubimci)
            {
                Console.WriteLine($"{l.Id} | {l.Ime} | {l.LatinskiNaziv} | {l.ProdajnaCena}");
            }
        }

        private void ProdajLjubimca(Korisnik ulogovaniKorisnik)
        {
            if (_prodajaServis == null)
            {
                Console.WriteLine("Prodaja trenutno nije dostupna (radno vreme 08-22).");
                _logger.LogUpozorenje("Pokusaj prodaje dok je _prodajaServis == null (van radnog vremena).");
                return;
            }

            Console.Write("Unesi ID ljubimca (GUID): ");
            string? unos = Console.ReadLine();

            if (!Guid.TryParse(unos, out Guid ljubimacId))
            {
                Console.WriteLine("Neispravan GUID.");
                _logger.LogUpozorenje($"Prodaja: pogresan GUID unos: {unos}");
                return;
            }

            var rezultat = _prodajaServis.Prodaj(ljubimacId, ulogovaniKorisnik);

            if (!rezultat.ok)
            {
                Console.WriteLine($"Prodaja nije uspela: {rezultat.poruka}");
                return;
            }

            Console.WriteLine("Prodaja uspesna!");

            if (rezultat.racun != null)
            {
                Console.WriteLine("\n--- FISKALNI RACUN ---");
                Console.WriteLine($"Racun ID: {rezultat.racun.Id}");
                Console.WriteLine($"Prodavac: {rezultat.racun.ImeProdavca}");
                Console.WriteLine($"Datum: {rezultat.racun.DatumVreme}");
                Console.WriteLine($"Ukupno: {rezultat.racun.UkupanIznos}");
            }
        }


        private void PrikaziFiskalneRacune()
        {
            Console.WriteLine("\n--- FISKALNI RACUNI ---");

            var racuni = _racuniRepo.VratiSve();

            if (racuni.Count == 0)
            {
                Console.WriteLine("Nema fiskalnih racuna.");
                return;
            }

            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("ID                                   | Prodavac          | Datum/Vreme           | Ukupno");
            Console.WriteLine("--------------------------------------------------------------------------------");

            foreach (var r in racuni)
            {
                Console.WriteLine($"{r.Id} | {r.ImeProdavca,-16} | {r.DatumVreme,-19} | {r.UkupanIznos,8}");
            }
        }



    }

}
