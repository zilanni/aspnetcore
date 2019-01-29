using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILanni.FarmMarket.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MongoDB.Driver;
using ILanni.FarmMarket.Domain;

namespace ILanni.FarmMarket.Web.Controllers
{
    public class TestController : Controller
    {
        ElasticClient client;
        MongoClient mClient;
        ProductService service;

        public TestController(ElasticClient client, MongoClient mClient, ProductService service)
        {
            this.client = client;
            this.mClient = mClient;
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            var response = await client.ListAsnyc<Product>(
                s => s.Size(20)
                .Query(q => q.Term(p => p.Sptype, "北乡马蹄"))
            );
            var j = JArray.Parse(JsonConvert.SerializeObject(response.Raws));
            return View("JContainer", j);
        }

        public IActionResult Add()
        {
            ILanni.FarmMarket.Models.Product p = new ILanni.FarmMarket.Models.Product()
            {
                Id = DateTime.Now.Ticks,
                Area = new List<string>() { "广东省", "韶关市", "乐昌市" },
                Areacode = new List<string>() { "广东省", "韶关市", "乐昌市" },
                Category = new List<string>() { "水果", "马蹄" },
                Categorycode = new List<string>() { "水果", "马蹄" },
                Desc = "乐昌北乡马蹄，个大，柔嫩，爽甜",
                Keywords = new List<string>() { "马蹄", "北乡", "贡品" },
                Name = "北乡马蹄",
                Position = new[] { 102.21f, 35.21f },
                Sptype = "北乡马蹄",
                Summary = "北乡马蹄",
                Title = "北乡马蹄"
            };

            /*var db = mClient.GetDatabase("test");
            var collection = db.GetCollection<Product>("product");
            await collection.InsertOneAsync(p, new InsertOneOptions() { BypassDocumentValidation = true });*/
            service.Add(p);
            var response = new { ok = true };
           /*var response = await client.IndexAsync(p);
            if (response.IsValid)
            {
                return RedirectToAction("Details", new { id = p.Id });
            }*/
            return View("JContainer", JObject.FromObject(response));
        }

        public async Task<IActionResult> Details(long id)
        {
            var (product, isVaild, response) = await client.GetByIdAsync<Product>(id);
            if (null != product)
            {
                return View("JContainer", JObject.Parse(JsonConvert.SerializeObject(product)));
            }
            if (isVaild)
            {
                return NotFound();
            }
            return View("JContainer", JObject.Parse(JsonConvert.SerializeObject(response)));
        }
    }
}