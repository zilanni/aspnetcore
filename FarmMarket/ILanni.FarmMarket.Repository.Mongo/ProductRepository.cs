using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace ILanni.FarmMarket.Repository.Mongo
{
    public class ProductRepository
    {
        protected MongoClient Client
        {
            get;
            private set;
        }

        
        
        public ProductRepository(MongoClient client)
        {
            this.Client = client;
        }

        public async void Add(ILanni.FarmMarket.Models.Product product)
        {
            var dbModel= AutoMapper.Mapper.Map<Product>(product);
            var db = Client.GetDatabase("test");
            var collection = db.GetCollection<Product>("product");
            await collection.InsertOneAsync(dbModel, new InsertOneOptions() { BypassDocumentValidation = true });
        }

    }
}
