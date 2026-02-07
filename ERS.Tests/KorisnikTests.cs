using NUnit.Framework;
using Domain.Modeli;
using Domain.Enumeracije;

namespace ERS.Tests
{
    [TestFixture]
    public class KorisnikTests
    {
        [Test]
        public void Korisnik_MozeDaSeKreira_ImaIspravnaPolja()
        {
            var k = new Korisnik
            {
                KorisnickoIme = "menadzer",
                Lozinka = "123",
                ImePrezime = "Petar Petrovic",
                Uloga = TipKorisnika.Menadzer
            };

            Assert.That(k.KorisnickoIme, Is.EqualTo("menadzer"));
            Assert.That(k.Lozinka, Is.EqualTo("123"));
            Assert.That(k.ImePrezime, Is.EqualTo("Petar Petrovic"));
            Assert.That(k.Uloga, Is.EqualTo(TipKorisnika.Menadzer));
        }
    }
}
