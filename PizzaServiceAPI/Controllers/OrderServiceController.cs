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
using MongoDB.Driver;
using System.Security.Authentication;
using MongoDB.Bson;

namespace PizzaServiceAPI.Controllers
{
    public class OrderServiceController : ApiController
    {
        private String host = "group4pizza.documents.azure.com";
        private int port = 10255;
        private String dbName = "pizzadelivery";
        private String userName = "group4pizza";
        private String password = "NQPMOstlYSlI6qrfZ8gePZNJHEQStcSGM4rZpPdM2p1F44P8qfj04PXX6MIIqiXWRyvTH0yXAXeE37A5k4sH1A==";

        private String collectionOrderService = "orderservice";

        MongoDB.Driver.MongoClient client;
        public OrderServiceController()
        {
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(host, port);
            settings.UseSsl = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

            MongoIdentity identity = new MongoInternalIdentity(dbName, userName);
            MongoIdentityEvidence evidence = new PasswordEvidence(password);

            settings.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);

            client = new MongoClient(settings);
        }

        // GET: api/OrderService/
        [SwaggerOperation("GetAllOrdersService")]
        public IEnumerable<OrderService> Get()
        {
            IMongoDatabase database = client.GetDatabase(dbName);
            var collection = database.GetCollection<OrderService>(collectionOrderService);
            IEnumerable<OrderService> data = collection.Find(new BsonDocument()).ToEnumerable<OrderService>();
            return data;
        }

        // GET: api/OrderService/:id
        [SwaggerOperation("GetOrderServiceyByOrderId")]
        public OrderService Get(int orderId)
        {
            IMongoDatabase database = client.GetDatabase(dbName);
            var collection = database.GetCollection<OrderService>(collectionOrderService);
            IEnumerable<OrderService> data = collection.Find<OrderService>(x => x.orderId == orderId).Limit(1).ToEnumerable<OrderService>();
            return data.First<OrderService>();
        }

        // PUT: api/OrderService/
        [SwaggerOperation("AddOrderService")]
        public void Put([FromBody]JObject data)
        {
            int orderId = Convert.ToInt32(data.GetValue("orderId"));
            string customerName = Convert.ToString(data.GetValue("customerName"));
            int userId = Convert.ToInt32(data.GetValue("orderId"));
            DateTime thisTime = DateTime.UtcNow;
            string notes = "";

            IMongoDatabase database = client.GetDatabase(dbName);
            var collection = database.GetCollection<OrderService>(collectionOrderService);
            OrderService orderService = new OrderService {
                id = Guid.NewGuid(),
                orderId = orderId,
                customerName = customerName,
                status = 1,
                serverStart = thisTime,
                serverEnd = thisTime,
                notes = notes
            };
            collection.InsertOne(orderService);
        }

        // POST: api/OrderService/
        [SwaggerOperation("UpdateOrderService")]
        public void Post(int id, [FromBody]JObject data)
        {
            string notes = Convert.ToString(data.GetValue("notes"));

            IMongoDatabase database = client.GetDatabase(dbName);
            var collection = database.GetCollection<OrderService>(collectionOrderService);
            var filter = Builders<OrderService>.Filter.Eq("orderId", id);
            var update = Builders<OrderService>.Update.Set("serverEnd", DateTime.UtcNow)
                .Set("status", 2);
            collection.UpdateOne(filter, update);
        }
    }
}
