using Domain.Modeli;

namespace Domain.BazaPodataka
{
    public class TabeleBazaPodataka
    {
        public List<Korisnik> Korisnici { get; set; } = [];
        // TODO: Add other database tables as needed
        public List<KucniLjubimac> Ljubimci { get; set; } = [];
        public List<FiskalniRacun> FiskalniRacuni { get; set; } = [];

        public TabeleBazaPodataka() { }
    }
}
