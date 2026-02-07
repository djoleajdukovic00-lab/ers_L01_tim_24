using System;
using NUnit.Framework;
using Domain.Modeli;

namespace ERS.Tests
{
    [TestFixture]
    public class FiskalniRacunTests
    {
        [Test]
        public void FiskalniRacun_MozeDaSeKreira_ImaIspravnaPolja()
        {
            var racun = new FiskalniRacun
            {
                Id = Guid.NewGuid(),
                ImeProdavca = "Marko Markovic",
                DatumVreme = DateTime.Now,
                UkupanIznos = 1500m
            };

            Assert.That(racun.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(racun.ImeProdavca, Is.EqualTo("Marko Markovic"));
            Assert.That(racun.DatumVreme, Is.LessThanOrEqualTo(DateTime.Now));
            Assert.That(racun.UkupanIznos, Is.GreaterThan(0));
        }
    }
}
