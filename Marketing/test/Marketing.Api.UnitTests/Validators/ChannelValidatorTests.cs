using AutoFixture;
using FluentValidation.TestHelper;
using Marketing.Api.Validators;
using Marketing.Domain.Domains;
using Xunit;

namespace Marketing.Api.UnitTests.Validators
{
    public class ChannelValidatorTests
    {
        private readonly ChannelValidator _validator;
        private readonly Fixture _fixture;

        public ChannelValidatorTests()
        {
            _fixture = new Fixture();
            _validator = new ChannelValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validation_ShouldReturnValidationErrorGivenAnInvalidName(string name)
        {
            var input = _fixture.Build<Channel>()
                .With(x => x.Name, name)
                .Create();

            _validator.ShouldHaveValidationErrorFor(x => x.Name, input);
        }

        [Fact]
        public void Validation_ShouldNotReturnValidationErrorGivenAValidName()
        {
            var input = _fixture.Build<Channel>()
                .With(x => x.Name, _fixture.Create<string>())
                .Create();

            _validator.ShouldNotHaveValidationErrorFor(x => x.Name, input);
        }
    }
}