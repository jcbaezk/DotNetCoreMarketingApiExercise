namespace Marketing.Persistence.Entities
{
    public class AdvertisementChannel
    {
        public int AdvertisementId { get; set; }

        public Advertisement Advertisement { get; set; }

        public int ChannelId { get; set; }

        public Channel Channel { get; set; }
    }
}