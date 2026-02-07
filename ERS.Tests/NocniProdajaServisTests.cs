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
    public class NocniProdajaServisTests
    {
        [Test]
        public void Prodaj_NeprodatLjubimac_VracaOkTrue_OznaciProdat_IUpiseRacun_SaPopustomNocna()
        {
            // Arrange
            var logger = new Mock<ILoggerServis>();
            var ljubimciRepo = new Mock<ILjubimciRepozitorijum>();
            var racuniRepo = new Mock<IFiskalniRacuniRepozitorijum>();

            var ljubimacId = Guid.NewGuid();
            var ljubimac = new KucniLjubimac
            {
                Id = ljubimacId,
                Ime = "Maza",
                LatinskiNaziv = "Felis catus",
                ProdajnaCena = 1000m,
                Prodat = false
            };

            ljubimciRepo
                .Setup(r => r.PronadjiPoId(ljubimacId))
                .Returns(ljubimac);

            var prodavac = new Korisnik { ImePrezime = "Marko Markovic" };

            var servis = new NocniProdajaServis(
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

            // nocna smena: +10% porez => 1000 * 1.10 = 1100
            Assert.That(racun!.UkupanIznos, Is.EqualTo(1100m));
            Assert.That(poruka, Does.Contain("nocna"));

            racuniRepo.Verify(r => r.Dodaj(It.IsAny<FiskalniRacun>()), Times.Once);
            racuniRepo.Verify(r => r.Sacuvaj(), Times.Once);
            ljubimciRepo.Verify(r => r.Sacuvaj(), Times.Once);
        }
    }
}
