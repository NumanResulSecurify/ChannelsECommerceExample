using ChannelsECommerceExample.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Runtime.ConstrainedExecution;

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


        //while (await _channelService.OrderChannel.Reader.WaitToReadAsync(stoppingToken)): 
        //    Bu satır, kanal üzerinde veri olup olmadığını kontrol eder ve veri gelene kadar asenkron olarak bekler.
        //Eğer stoppingToken iptal edilirse, bekleme işlemi sonlanır ve döngü sonlanır.

        //while (_channelService.OrderChannel.Reader.TryRead(out var order)): 
        //    Bu satır, kanalda veri varsa veriyi okur.Bu işlem, kanal boşalana kadar devam eder.
        //ProcessOrder(order): Her bir siparişi işler. Bu örnekte, sipariş bilgilerini loga yazdırır.

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
