using NUnit.Framework;
using Moq;

using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Services.AutenftikacioniServisi;

namespace ERS.Tests
{
    [TestFixture]
    public class AutentifikacioniServisTests
    {
        [Test]
        public void Prijava_TacniPodaci_VracaOkTrueIKorisnika()
        {
            // Arrange
            var logger = new Mock<ILoggerServis>();
            var korisniciRepo = new Mock<IKorisniciRepozitorijum>();

            var korisnik = new Korisnik
            {
                KorisnickoIme = "menadzer",
                Lozinka = "123",
                ImePrezime = "Petar Petrovic"
            };

            korisniciRepo
                .Setup(r => r.PronadjiKorisnikaPoKorisnickomImenu("menadzer"))
                .Returns(korisnik);

            var servis = new AutentifikacioniServis(korisniciRepo.Object, logger.Object);

            // Act
            var (ok, rezultat) = servis.Prijava("menadzer", "123");

            // Assert
            Assert.That(ok, Is.True);
            Assert.That(rezultat, Is.Not.Null);
            Assert.That(rezultat.KorisnickoIme, Is.EqualTo("menadzer"));
        }

        [Test]
        public void Prijava_PogresnaLozinka_VracaOkFalse_IPraviPraznogKorisnika()
        {
            // Arrange
            var logger = new Mock<ILoggerServis>();
            var korisniciRepo = new Mock<IKorisniciRepozitorijum>();

            var korisnik = new Korisnik
            {
                KorisnickoIme = "menadzer",
                Lozinka = "123",
                ImePrezime = "Petar Petrovic"
            };

            korisniciRepo
                .Setup(r => r.PronadjiKorisnikaPoKorisnickomImenu("menadzer"))
                .Returns(korisnik);

            var servis = new AutentifikacioniServis(
                korisniciRepo.Object,
                logger.Object
            );

            // Act
            var (ok, rezultat) = servis.Prijava("menadzer", "xxx");

            // Assert
            Assert.That(ok, Is.False);
            Assert.That(rezultat, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(rezultat.KorisnickoIme), Is.True);
        }

    }
}
