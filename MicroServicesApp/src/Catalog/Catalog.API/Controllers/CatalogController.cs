using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using MongoDB.Driver;
using System;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {

        private readonly ILogger<CatalogController> _logger;
        private readonly IProductRepository _repository;
        public CatalogController(ILogger<CatalogController> logger, IProductRepository repo)
        {
            _logger = logger;
            _repository = repo;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var products = _repository.GetItems<Product>().ToList();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public ActionResult<Product> GetProduct(string id)
        {
            var product = _repository.GetItem<Product>(x => x.Id == id);

            if (product == null)
            {
                _logger.LogError($"Product with id: {id}, hasn't been found in database.");
                return NotFound();
            }

            return Ok(product);
        }

        [Route("[action]/{category}")]
        [HttpGet]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<Product>> GetProductByCategory(string category)
        {
            var product = _repository.GetItem<Product>(x => x.Category == category);
            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
        public ActionResult<Product> CreateProduct([FromBody] Product product)
        {
            product.Id = Guid.NewGuid().ToString();
            _repository.Save<Product>(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public IActionResult UpdateProduct([FromBody] Product value)
        {
            var collection = ((ProductRepository)this._repository).GetCollection<Product>();
            var filter = Builders<Product>.Filter.Eq(x => x.Id, value.Id);
            var set = Builders<Product>.Update.Set(x => x.ImageFile, value.ImageFile)
                .Set(x => x.Name, value.Name).Set(x => x.Summary, value.Summary);
            var result = collection.UpdateMany(filter, set);
            return Ok(result.ModifiedCount);
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public IActionResult DeleteProductById(string id)
        {
            var collection = ((ProductRepository)this._repository).GetCollection<Product>();
            var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
            var result = collection.DeleteOne(filter);
            return Ok(result.DeletedCount);
        }

    }
}
