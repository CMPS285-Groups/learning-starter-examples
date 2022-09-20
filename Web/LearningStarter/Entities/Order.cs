﻿using System;
using System.Collections.Generic;

namespace LearningStarter.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string PaymentType { get; set; }
        public DateTimeOffset DatePurchased { get; set; }

        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }

    public class OrderGetDto 
    {
        public int Id { get; set; }
        public string PaymentType { get; set; }
        public DateTimeOffset DatePurchased { get; set; }
    }
}
