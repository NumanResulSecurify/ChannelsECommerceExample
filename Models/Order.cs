﻿namespace ChannelsECommerceExample.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }
}
