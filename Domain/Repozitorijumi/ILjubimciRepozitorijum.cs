using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Modeli;

namespace Domain.Repozitorijumi
{
    public interface ILjubimciRepozitorijum
    {
        List<KucniLjubimac> VratiSve();
        List<KucniLjubimac> VratiNeprodate();
        KucniLjubimac? PronadjiPoId(Guid id);

        void Dodaj(KucniLjubimac ljubimac);
        bool Sacuvaj(); // samo prosledi na bazu
    }
}