using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using BharatMirror.Dataclasses;
using BharatMirror.Models;
using BharatMirror.Services;

namespace BharatMirror.Controllers
{
    public class NewsController : Controller
    {
        private readonly IAdvertisementRepository _advertisementRepository;
        public NewsController(IAdvertisementRepository advertisementRepo)
        {
            _advertisementRepository = advertisementRepo;
        }
       
        public async Task<IActionResult> Index([FromQuery(Name = "Category")] string category)
        {
            var Model = _advertisementRepository.GetAdvertisements(category);
            Model = ShuffleAdvertisement.Shuffle(Model, Model.Count());
            if(category != null)
            {
                ViewBag.category = category;

                category = "&category=" + category;
            }
            else
            {
                ViewBag.category = "";
                category = "";
            }
            string baseUrl = "https://newsapi.org/v2/top-headlines?country=us&apiKey=524423f5855d4a7ab7e5a4dc35ba8a6a&pagesize=6" + category;
            //Have your using statements within a try/catch block
            try
            {
                //We will now define your HttpClient with your first using statement which will use a IDisposable.
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; " +
                                  "Windows NT 5.2; .NET CLR 1.0.3705;)");
                    //In the next using statement you will initiate the Get Request, use the await keyword so it will execute the using statement in order.
                    using (HttpResponseMessage res = await client.GetAsync(baseUrl))
                    {
                        //Then get the content from the response in the next using statement, then within it you will get the data, and convert it to a c# object.
                        using (HttpContent content = res.Content)
                        {
                            //Now assign your content to your data variable, by converting into a string using the await keyword.
                            var data = await content.ReadAsStringAsync();
                            int len = data.Length;
                            
                            //If the data isn't null return log convert the data using newtonsoft JObject Parse class method on the data.
                            if (len > 165)
                            {
                                int index = data.IndexOf('[');
                                int n = data.Length;
                                data = data.Substring(index, n - 1 - index);
                                var artList = JsonSerializer.Deserialize<IList<Articles>>(data);
                                ViewBag.articles = artList;
                                ViewBag.status = "true";
                            }
                            else
                            {
                                ViewBag.status = "false";
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception Hit------------");
                Console.WriteLine(exception);
            }

            return View(Model);
        }

        [HttpPost]
        public void AddCtr(int Id)
        {
            _advertisementRepository.AddCtr(Id);
        }
    }
}
