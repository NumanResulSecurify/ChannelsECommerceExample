using ChannelsECommerceExample.Models;
using ChannelsECommerceExample.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChannelsECommerceExample.Controllers
{
    public class OrderController : Controller
    {
        private readonly ChannelService _channelService;

        public OrderController(ChannelService channelService)
        {
            _channelService = channelService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            // Siparişi kanala yazıyoruz.
            await _channelService.OrderChannel.Writer.WriteAsync(order);

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
