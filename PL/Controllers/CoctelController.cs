using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ML;
using Newtonsoft.Json;
using System.Dynamic;

namespace PL.Controllers
{
    public class CoctelController : Controller
    {
        [HttpGet]
        public ActionResult GetAll(string letra = "a")
        {
            ML.Coctel coctel = new ML.Coctel();
            coctel.Cocteles = new List<object>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://www.thecocktaildb.com/api/json/v1/1/");

                var responseTask = client.GetAsync("search.php?f=" + letra);
                responseTask.Wait();

                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<dynamic>();
                    readTask.Wait();
                        
                    foreach (var resultItem in readTask.Result.drinks)
                    {
                        ML.Coctel ResultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Coctel>(resultItem.ToString());
                        coctel.Cocteles.Add(ResultItemList);
                    }
                }
            }
            return View(coctel);
        }

        [HttpPost]
        public ActionResult GetAll(ML.Coctel coctel)
        {
            if (coctel.strDrink != null)
            {
                coctel.Cocteles = new List<object>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://www.thecocktaildb.com/api/json/v1/1/");

                    var responseTask = client.GetAsync("search.php?s=" + coctel.strDrink);
                    responseTask.Wait();

                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<dynamic>();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.drinks)
                        {
                            ML.Coctel ResultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Coctel>(resultItem.ToString());
                            coctel.Cocteles.Add(ResultItemList);
                        }
                    }
                }
                return View(coctel);
            }
            else
            {
                coctel.Cocteles = new List<object>();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://www.thecocktaildb.com/api/json/v1/1/");

                    var responseTask = client.GetAsync("filter.php?i=" + coctel.strIngredient1);
                    responseTask.Wait();

                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<dynamic>();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.drinks)
                        {
                            ML.Coctel ResultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Coctel>(resultItem.ToString());

                            using (var clientId = new HttpClient())
                            {
                                clientId.BaseAddress = new Uri("https://www.thecocktaildb.com/api/json/v1/1/");

                                var responseTaskId = clientId.GetAsync("lookup.php?i=" + ResultItemList.idDrink);
                                responseTaskId.Wait();

                                var resultId = responseTaskId.Result;

                                if (resultId.IsSuccessStatusCode)
                                {
                                    var readTaskId = resultId.Content.ReadAsAsync<dynamic>();
                                    readTaskId.Wait();

                                    foreach (var resultItemId in readTaskId.Result.drinks)
                                    {
                                        ML.Coctel ResultItemListId = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Coctel>(resultItemId.ToString());
                                        coctel.Cocteles.Add(ResultItemListId);
                                    }
                                }
                            }
                        }
                    }
                    return View(coctel);
                }
            }
        }
    }
}
