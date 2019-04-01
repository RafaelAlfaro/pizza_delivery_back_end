using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PizzaServiceAPI.Models;
using Swashbuckle.Swagger.Annotations;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Web.Hosting;

namespace PizzaServiceAPI.Controllers
{
    public class OrderController : ApiController
    {
        // GET: api/Order
        [SwaggerOperation("GetAllOrders")]
        public IEnumerable<Order> Get()
        {
            var data = File.ReadAllText(HostingEnvironment.MapPath(@"/App_Data/order.json"));
            try
            {
                return JsonConvert.DeserializeObject<IEnumerable<Order>>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: api/Order/:id
        [SwaggerOperation("GetOrderById")]
        public Order Get(int id)
        {
            var data = File.ReadAllText(HostingEnvironment.MapPath(@"/App_Data/order.json"));
            var datalines = File.ReadAllText(HostingEnvironment.MapPath(@"/App_Data/orderline.json"));
            var dataProducts = File.ReadAllText(HostingEnvironment.MapPath(@"/App_Data/product.json"));
            Order orderResults = null;
            try
            {
                var orders = JsonConvert.DeserializeObject<IEnumerable<Order>>(data);
                var orderQuery =
                    from order in orders
                    where order.id == id
                    select order;
                if (orderQuery.Count() > 0)
                {
                    orderResults = orderQuery.ToArray<Order>()[0];
                    var ordersLines = JsonConvert.DeserializeObject<IEnumerable<OrderLine>>(datalines);
                    var orderLineQuery =
                        from line in ordersLines
                        where line.orderId == orderResults.id
                        select line;
                    if (orderLineQuery.Count() > 0)
                    {
                        OrderLine[] lines = orderLineQuery.ToArray<OrderLine>();
                        var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(dataProducts);
                        foreach (OrderLine line in lines)
                        {
                            var productQuery =
                            from product in products
                            where product.id == line.productId
                            select product;
                            line.product = productQuery.ToArray<Product>()[0];
                        }
                        orderResults.lines = lines;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return orderResults;
        }

        // PUT: api/Order/:id
        [SwaggerOperation("UpdateOrder")]
        public void Put(int id, [FromBody] JObject value)
        {
            string fileName = HostingEnvironment.MapPath(@"/App_Data/order.json");
            var data = File.ReadAllText(fileName);

            try
            {
                var orderArray = JsonConvert.DeserializeObject<IEnumerable<Order>>(data);
               

                if (id > 0)
                {
                    foreach (var order in orderArray.Where(obj => obj.id == id))
                    {
                      //  order.status = Int32.Parse(value.status);
                        order.status = Int32.Parse(value.GetValue("status").ToString());
                    }

                    data = orderArray.ToString();
                    string output = JsonConvert.SerializeObject(orderArray, Formatting.Indented);
                    File.WriteAllText(fileName, output);
                }
                else
                {
                    Console.Write("Invalid order ID");
                }
                
            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}
