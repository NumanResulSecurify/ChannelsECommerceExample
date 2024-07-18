using ChannelsECommerceExample.Models;

namespace ChannelsECommerceExample.Services
{
    public class OrderProcessor : BackgroundService
    {
        private readonly ChannelService _channelService;

        public OrderProcessor(ChannelService channelService)
        {
            _channelService = channelService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await _channelService.OrderChannel.Reader.WaitToReadAsync(stoppingToken))
            {
                while (_channelService.OrderChannel.Reader.TryRead(out var order))
                {
                    // Siparişi işliyoruz
                    ProcessOrder(order);
                }
            }
        }

        private void ProcessOrder(Order order)
        {
            // Sipariş işlemleri burada yapılır.
            // Örneğin, stok kontrolü ve siparişin veritabanına kaydedilmesi.

            // Basit bir örnek işlem:
            order.Status = "Processed";
            Console.WriteLine($"Order {order.OrderId} processed.");
        }
    }
}
