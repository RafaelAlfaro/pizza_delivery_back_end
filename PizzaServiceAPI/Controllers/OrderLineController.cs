using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using PizzaServiceAPI.Models;
using Swashbuckle.Swagger.Annotations;
using Newtonsoft.Json.Linq;
using System.Web.Hosting;

namespace PizzaServiceAPI.Controllers
{
    public class OrderLineController : ApiController
    {
        // PUT: api/OrderLine/:id
        [SwaggerOperation("UpdateOrderLine")]
        public void Put(int id, [FromBody]JObject value)
        {
            string fileName = HostingEnvironment.MapPath(@"/App_Data/orderline.json");
            var data = File.ReadAllText(fileName);

            try
            {
                var orderArray = JsonConvert.DeserializeObject<IEnumerable<OrderLine>>(data);
                if (id > 0)
                {
                    foreach (var order in orderArray.Where(obj => obj.id == id))
                    {
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

