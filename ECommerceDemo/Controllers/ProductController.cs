using System;
using System.Collections.Generic;
using System.Net.Http;
using ECommerceDemo.Models;
using ECommerceDemo.Repositary;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceDemo.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product  
        public ActionResult Products([FromQuery(Name = "page")] int page = 1)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                PagedResult<Product> products = null;
                HttpResponseMessage response = serviceObj.GetResponse("api/Products/GetAllProducts/?page=" + page);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                    products = response.Content.ReadAsAsync<PagedResult<Product>>().Result;

                ViewBag.Title = "All Products";
                return View(products);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }


        public ActionResult EditProduct(int id)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                List<Models.ProductsDTO> products = null;

                HttpResponseMessage response = serviceObj.GetResponse("api/Products/" + id.ToString());
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                    products = response.Content.ReadAsAsync<List<Models.ProductsDTO>>().Result;

                ViewBag.Title = "All Products";
                return View(products);
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        public ActionResult Update(Models.Product product)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PutResponse("api/Products/" + product.ProductId.ToString(), product);
                response.EnsureSuccessStatusCode();
                return RedirectToAction("Products");
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Models.Product product)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.PostResponse("api/Products", product);
                response.EnsureSuccessStatusCode();
                return RedirectToAction("Products");
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                ServiceRepository serviceObj = new ServiceRepository();
                HttpResponseMessage response = serviceObj.DeleteResponse("api/Products/" + id.ToString());
                response.EnsureSuccessStatusCode();
                return RedirectToAction("Products");
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }

    }
}