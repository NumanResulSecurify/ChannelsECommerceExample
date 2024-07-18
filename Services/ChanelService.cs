using ChannelsECommerceExample.Models;
using System.Threading.Channels;

namespace ChannelsECommerceExample.Services
{
    public class ChannelService
    {
        public Channel<Order> OrderChannel { get; }

        public ChannelService()
        {
            // 1000 kapasiteli sınırlı bir kanal oluşturuyoruz.
            OrderChannel = Channel.CreateBounded<Order>(
                new BoundedChannelOptions(1000)
                {
                    FullMode = BoundedChannelFullMode.DropOldest // Kanal dolduğunda en eski mesajı sil.
                });
        }
    }
}
