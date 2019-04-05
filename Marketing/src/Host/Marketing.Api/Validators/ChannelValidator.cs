using FluentValidation;
using Marketing.Domain.Domains;

namespace Marketing.Api.Validators
{
    public class ChannelValidator : AbstractValidator<Channel>
    {
        public ChannelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}