using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Marketing.Persistence.Entities.Configurations
{
    public class AdvertisementChannelConfiguration : IEntityTypeConfiguration<AdvertisementChannel>
    {
        public void Configure(EntityTypeBuilder<AdvertisementChannel> builder)
        {
            builder.HasKey(x => new { x.AdvertisementId, x.ChannelId });

            builder.HasOne(x => x.Advertisement)
                .WithMany(x => x.AdvertisementChannels)
                .HasForeignKey(x => x.AdvertisementId);

            builder.HasOne(x => x.Channel)
                .WithMany(x => x.AdvertisementChannels)
                .HasForeignKey(x => x.ChannelId);
        }
    }
}