using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaServiceAPI.Models
{
    public class OrderService
    {
        public Guid id { set; get; }
        public int orderId { set; get; }
        public String customerName { set; get; }
        public int userId { set; get; }
        public int status { set; get; }
        public DateTime serverStart { set; get; }
        public DateTime serverEnd { set; get; }
        public String notes { set; get; }
    }
}