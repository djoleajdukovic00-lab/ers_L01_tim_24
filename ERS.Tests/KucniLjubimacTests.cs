using NUnit.Framework;
using Domain.Modeli;

namespace ERS.Tests
{
    [TestFixture]
    public class KucniLjubimacTests
    {
        [Test]
        public void KucniLjubimac_MozeDaSeKreira_ImaIspravnaPolja()
        {
            var ljubimac = new KucniLjubimac
            {
                Ime = "Rex",
                LatinskiNaziv = "Canis lupus familiaris",
                ProdajnaCena = 1200m,
                Prodat = false
            };

            Assert.That(ljubimac.Ime, Is.EqualTo("Rex"));
            Assert.That(ljubimac.LatinskiNaziv, Is.EqualTo("Canis lupus familiaris"));
            Assert.That(ljubimac.ProdajnaCena, Is.GreaterThan(0));
            Assert.That(ljubimac.Prodat, Is.False);
        }
    }
}
