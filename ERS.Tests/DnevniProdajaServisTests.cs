using System;
using NUnit.Framework;
using Moq;

using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;
using Services.ProdajaServisi;

namespace ERS.Tests
{
    [TestFixture]
    public class DnevniProdajaServisTests
    {
        [Test]
        public void Prodaj_NeprodatLjubimac_VracaOkTrue_OznaciProdat_IUpiseRacun_SaPopustom15()
        {
            // Arrange
            var logger = new Mock<ILoggerServis>();
            var ljubimciRepo = new Mock<ILjubimciRepozitorijum>();
            var racuniRepo = new Mock<IFiskalniRacuniRepozitorijum>();

            var ljubimacId = Guid.NewGuid();
            var ljubimac = new KucniLjubimac
            {
                Id = ljubimacId,
                Ime = "Rex",
                LatinskiNaziv = "Canis lupus familiaris",
                ProdajnaCena = 1000m,
                Prodat = false
            };

            ljubimciRepo
                .Setup(r => r.PronadjiPoId(ljubimacId))
                .Returns(ljubimac);

            var prodavac = new Korisnik { ImePrezime = "Marko Markovic" };

            var servis = new DnevniProdajaServis(
                ljubimciRepo.Object,
                racuniRepo.Object,
                logger.Object
            );

            // Act
            var (ok, poruka, racun) = servis.Prodaj(ljubimacId, prodavac);

            // Assert
            Assert.That(ok, Is.True);
            Assert.That(racun, Is.Not.Null);
            Assert.That(ljubimac.Prodat, Is.True);

            // popust 15% => 1000 * 0.85 = 850
            Assert.That(racun!.UkupanIznos, Is.EqualTo(850m));
            Assert.That(poruka, Does.Contain("dnevna smena"));

            // upis racuna + snimanje
            racuniRepo.Verify(r => r.Dodaj(It.IsAny<FiskalniRacun>()), Times.Once);
            racuniRepo.Verify(r => r.Sacuvaj(), Times.Once);
            ljubimciRepo.Verify(r => r.Sacuvaj(), Times.Once);
        }
    }
}
