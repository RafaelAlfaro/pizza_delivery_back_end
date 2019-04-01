using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaServiceAPI.Models
{
    public class Order
    {
        public int id { set; get; }
        public int customerId { set; get; }
        public String customerName { set; get; }
        public int userId { set; get; }
        public int status { set; get; }
        public OrderLine[] lines{set; get; }
    }
}