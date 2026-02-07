using Domain.BazaPodataka;
using Domain.Enumeracije;
using Domain.Modeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Database.BazaPodataka
{
    public class JsonBazaPodataka : IBazaPodataka
    {
        private const string Putanja = "baza.json";

        public TabeleBazaPodataka Tabele { get; set; } = new();

        public bool Ucitaj()
        {
            try
            {
                if (!File.Exists(Putanja))
                {
                    Tabele = KreirajInicijalnePodatke();
                    return SacuvajPromene();
                }

                string json = File.ReadAllText(Putanja);
                Tabele = JsonSerializer.Deserialize<TabeleBazaPodataka>(json)
                         ?? new TabeleBazaPodataka();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SacuvajPromene()
        {
            try
            {
                var opcije = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string json = JsonSerializer.Serialize(Tabele, opcije);
                File.WriteAllText(Putanja, json);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private TabeleBazaPodataka KreirajInicijalnePodatke()
        {
            return new TabeleBazaPodataka
            {
                Korisnici =
                {
                    new Korisnik
                    {
                        Id = Guid.NewGuid(),
                        KorisnickoIme = "menadzer",
                        Lozinka = "123",
                        ImePrezime = "Petar Petrovic",
                        Uloga = TipKorisnika.Menadzer
                    },
                    new Korisnik
                    {
                        Id = Guid.NewGuid(),
                        KorisnickoIme = "prodavac",
                        Lozinka = "123",
                        ImePrezime = "Marko Markovic",
                        Uloga = TipKorisnika.Prodavac
                    }
                },
                Ljubimci =
                {
                    new KucniLjubimac
                    {
                        Id = Guid.NewGuid(),
                        LatinskiNaziv = "Canis lupus familiaris",
                        Ime = "Rex",
                        Tip = TipLjubimca.Sisar,
                        ProdajnaCena = 15000,
                        Prodat = false
                    }
                }
            };
        }
    }
}
