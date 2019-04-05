using System.Collections.Generic;
using AutoFixture;
using FluentValidation.TestHelper;
using Marketing.Api.Validators;
using Marketing.Domain.Domains;
using Xunit;

namespace Marketing.Api.UnitTests.Validators
{
    public class AdvertisementEntryValidatorTests
    {
        private readonly AdvertisementEntryValidator _validator;
        private readonly Fixture _fixture;

        public AdvertisementEntryValidatorTests()
        {
            _fixture = new Fixture();
            _validator = new AdvertisementEntryValidator();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validation_ShouldReturnValidationErrorGivenAnInvalidName(string name)
        {
            var input = _fixture.Build<AdvertisementEntry>()
                .With(x => x.Name, name)
                .Create();

            _validator.ShouldHaveValidationErrorFor(x => x.Name, input);
        }

        [Fact]
        public void Validation_ShouldNotReturnValidationErrorGivenAValidName()
        {
            var input = _fixture.Build<AdvertisementEntry>()
                .With(x => x.Name, _fixture.Create<string>())
                .Create();

            _validator.ShouldNotHaveValidationErrorFor(x => x.Name, input);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-123)]
        [InlineData(0)]
        public void Validation_ShouldReturnValidationErrorGivenAnInvalidClientId(int? clientId)
        {
            var input = _fixture.Build<AdvertisementEntry>()
                .With(x => x.ClientId, clientId)
                .Create();

            _validator.ShouldHaveValidationErrorFor(x => x.ClientId, input);
        }

        [Fact]
        public void Validation_ShouldNotReturnValidationErrorGivenAValidClientId()
        {
            var input = _fixture.Build<AdvertisementEntry>()
                .With(x => x.ClientId, _fixture.Create<int>())
                .Create();

            _validator.ShouldNotHaveValidationErrorFor(x => x.ClientId, input);
        }

        [Fact]
        public void Validation_ShouldReturnValidationErrorGivenAnInvalidCollectionOfChannelIds()
        {
            var input = _fixture.Build<AdvertisementEntry>()
                .With(x => x.ChannelIds, null)
                .Create();

            _validator.ShouldHaveValidationErrorFor(x => x.ChannelIds, input);
        }

        [Fact]
        public void Validation_ShouldNotReturnValidationErrorGivenAValidCollectionOfChannelIds()
        {
            var input = _fixture.Build<AdvertisementEntry>()
                .With(x => x.ChannelIds, new List<int>())
                .Create();

            _validator.ShouldNotHaveValidationErrorFor(x => x.ChannelIds, input);
        }
    }
}