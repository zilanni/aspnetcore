using System;
using System.Collections.Generic;
using System.Text;
using ILanni.FarmMarket.Models;
using ILanni.FarmMarket.Repository.Mongo;
using ILanni.FarmMarket.MQ;

namespace ILanni.FarmMarket.Domain
{
    public class ProductService
    {

        private ProductRepository repository;
        private ProductPublisher publisher;
  
        public ProductService(ProductRepository repository, ProductPublisher publisher)
        {
            this.repository = repository;
            this.publisher = publisher;
        }

        public void Add(ILanni.FarmMarket.Models.Product product)
        {
            repository.Add(product);
            publisher.Publish(product);
        }
    }
}
