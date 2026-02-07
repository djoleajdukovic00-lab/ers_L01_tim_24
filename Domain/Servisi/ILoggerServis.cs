using Domain.Enumeracije;

namespace Domain.Servisi
{
    public interface ILoggerServis
    {
        void Log(string poruka);
        void LogGreska(string poruka);
        void LogUpozorenje(string poruka);
    }
}

