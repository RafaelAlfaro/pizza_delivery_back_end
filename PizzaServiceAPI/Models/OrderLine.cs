using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaServiceAPI.Models
{
    public class OrderLine
    {
        public int id { set; get; }
        public int productId { set; get; }
        public int orderId { set; get; }
        public Double quantity { set; get; }
        public int status { set; get; }
        public Product product { set; get; }
    }
}