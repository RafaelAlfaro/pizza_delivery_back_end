using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PizzaServiceAPI.Models
{
    public class Product
    {
        public int id { set; get; }
        public String name { set; get; }
        public Double price { set; get; }
    }
}