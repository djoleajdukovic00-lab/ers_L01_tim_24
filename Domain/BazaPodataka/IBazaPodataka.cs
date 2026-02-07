namespace Domain.BazaPodataka
{
    public interface IBazaPodataka
    {
        public TabeleBazaPodataka Tabele { get; set; }

        bool Ucitaj();

        public bool SacuvajPromene();
    }
}
