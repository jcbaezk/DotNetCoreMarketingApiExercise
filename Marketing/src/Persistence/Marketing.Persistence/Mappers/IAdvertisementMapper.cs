using System.Collections.Generic;
using Marketing.Domain.Domains;

namespace Marketing.Persistence.Mappers
{
    public interface IAdvertisementMapper
    {
        Advertisement ToDomain(Entities.Advertisement advertisement);

        IEnumerable<Advertisement> ToDomains(IEnumerable<Entities.Advertisement> advertisements);

        Entities.Advertisement ToEntity(AdvertisementEntry advertisementEntry);
    }
}