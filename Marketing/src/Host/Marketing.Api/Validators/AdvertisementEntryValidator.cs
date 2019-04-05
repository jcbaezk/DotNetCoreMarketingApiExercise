using FluentValidation;
using Marketing.Domain.Domains;

namespace Marketing.Api.Validators
{
    public class AdvertisementEntryValidator : AbstractValidator<AdvertisementEntry>
    {
        public AdvertisementEntryValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.ClientId).NotNull().GreaterThan(0);
            RuleFor(x => x.ChannelIds).NotNull();
        }
    }
}