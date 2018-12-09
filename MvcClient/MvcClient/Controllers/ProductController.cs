using MvcClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MvcClient.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public async Task<ActionResult> Index()
        {
            var products = await GetProducts();
            return View(products); 
        }

        public async Task<ActionResult> Create() {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(Product product)
        {
            var result = await SaveProduct(product);
            if (result == true)            
                ViewBag.Saved = true;        
            else
                ViewBag.Saved = false;
            return View();
        }


        private async Task<List<Product>> GetProducts()
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync("http://localhost:58307/api/Product/GetProducts");
            response.EnsureSuccessStatusCode();
            var result = response.Content.ReadAsAsync<List<Product>>();
            var products = result.Result;
            return products;
        }


        private async Task<bool> SaveProduct(Product product)
        {
            string url = "http://localhost:58307/api/Product/Create";
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            // client.BaseAddress = new Uri("http://localhost:58307/api/Product/Create");

            HttpResponseMessage response = await client.PostAsJsonAsync(
               url, product);
            response.EnsureSuccessStatusCode();
            var result = response.Content.ReadAsAsync<bool>();
            var IsSucceeded = result.Result;
            return IsSucceeded;
        }
    }
}