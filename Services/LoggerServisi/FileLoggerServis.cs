using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enumeracije;
using Domain.Modeli;
using Domain.Repozitorijumi;
using Domain.Servisi;

namespace Services.LoggerServisi
{
    public class FileLoggerServis : ILoggerServis
    {
        private readonly string _putanja;
        private readonly object _lock = new();

        public FileLoggerServis(string putanja)
        {
            _putanja = putanja;

            var dir = Path.GetDirectoryName(_putanja);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);
        }

        public void Log(string poruka) => Upisi("INFO", poruka);

        public void LogGreska(string poruka) => Upisi("ERROR", poruka);

        public void LogUpozorenje(string poruka) => Upisi("WARNING", poruka);

        private void Upisi(string tip, string poruka)
        {
            lock (_lock)
            {
                File.AppendAllText(
                    _putanja,
                    $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{tip}] {poruka}{Environment.NewLine}"
                );
            }
        }
    }
}
