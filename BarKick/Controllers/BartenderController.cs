using Newtonsoft.Json;
using BarKick.Models;
using BarKick.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
<<<<<<< Updated upstream
using System.Net.Http.Headers;
=======
using System.Linq;
>>>>>>> Stashed changes

namespace BarKick.Controllers
{
    public class BartenderController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer serializer = new JavaScriptSerializer();

        static BartenderController()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44335/api/")
            };
        }

        // GET: bartender/List
        public ActionResult List()
        {
            List<BartenderDto> bartenderDtos = new List<BartenderDto>();

            try
            {
                string url = "BartenderData/ListBartenders";
                HttpResponseMessage responseMessage = client.GetAsync(url).Result;

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    bartenderDtos = JsonConvert.DeserializeObject<List<BartenderDto>>(responseData);
                }
                else
                {
                    ViewBag.ErrorMessage = "Failed to retrieve bartenders from API.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
            }

            return View(bartenderDtos);
        }

        // GET: Bartender/Details/id
        public ActionResult Details(int id)
        {
            DetailsBartender viewModel = new DetailsBartender();

            // Fetch bartender details
<<<<<<< Updated upstream
            string url = "BartenderData/FindBartender/" + id;
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;
=======
            string bartenderUrl = "api/bartenderdata/findbartender/{id}";
            HttpResponseMessage bartenderResponse = client.GetAsync(bartenderUrl).Result;
>>>>>>> Stashed changes

            if (bartenderResponse.IsSuccessStatusCode)
            {
                string bartenderData = bartenderResponse.Content.ReadAsStringAsync().Result;
                BartenderDto selectedBartender = JsonConvert.DeserializeObject<BartenderDto>(bartenderData);
                viewModel.SelectedBartender = selectedBartender;
            }
<<<<<<< Updated upstream
            else if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ViewBag.ErrorMessage = "Bartender not found.";
                return View(viewModel);
            }
            else
            {
                ViewBag.ErrorMessage = "Failed to find bartender details.";
                return View(viewModel);
            }

            // Fetch venues associated with bartender
            string venuesUrl = "BartenderData/ListVenuesForBartender/" + id;
            HttpResponseMessage venuesResponse = client.GetAsync(venuesUrl).Result;

            if (venuesResponse.IsSuccessStatusCode)
            {
                string venuesData = venuesResponse.Content.ReadAsStringAsync().Result;
                viewModel.VenueBartenders = JsonConvert.DeserializeObject<IEnumerable<VenueDto>>(venuesData);
            }
            else
            {
                ViewBag.ErrorMessage = "Failed to fetch associated venues.";
=======
            else
            {
                ViewBag.ErrorMessage = "Failed to fetch bartender details.";
                return View(viewModel);
>>>>>>> Stashed changes
            }

            // Fetch cocktails made by bartender
            string cocktailsUrl = "api/cocktaildata/listcocktailsbybartender/{id}";
            HttpResponseMessage cocktailsResponse = client.GetAsync(cocktailsUrl).Result;

            if (cocktailsResponse.IsSuccessStatusCode)
            {
                string cocktailsData = cocktailsResponse.Content.ReadAsStringAsync().Result;
<<<<<<< Updated upstream
                viewModel.CocktailsMade = JsonConvert.DeserializeObject<IEnumerable<CocktailDto>>(cocktailsData);
=======
                ICollection<CocktailDto> cocktailsMade = JsonConvert.DeserializeObject<ICollection<CocktailDto>>(cocktailsData);
                viewModel.CocktailsMade = cocktailsMade;
>>>>>>> Stashed changes
            }
            else
            {
                ViewBag.ErrorMessage = "Failed to fetch cocktails made by bartender.";
            }

<<<<<<< Updated upstream

            // Fetch available venues (for association)
            string availableVenuesUrl = "VenueData/ListVenues/";
            HttpResponseMessage availableVenuesResponse = client.GetAsync(availableVenuesUrl).Result;

            if (availableVenuesResponse.IsSuccessStatusCode)
            {
                string availableVenuesData = availableVenuesResponse.Content.ReadAsStringAsync().Result;
                viewModel.AvailableVenues = JsonConvert.DeserializeObject<IEnumerable<VenueDto>>(availableVenuesData);
            }

            return View(viewModel);
        }

        // GET: Bartender/New
=======
            // Fetch venues employed at
            string venuesUrl = "api/venuedata/listvenuesbybartender/{id}";
            HttpResponseMessage venuesResponse = client.GetAsync(venuesUrl).Result;

            if (venuesResponse.IsSuccessStatusCode)
            {
                string venuesData = venuesResponse.Content.ReadAsStringAsync().Result;
                ICollection<VenueBartender> venueBartenders = JsonConvert.DeserializeObject<ICollection<VenueBartender>>(venuesData);
                viewModel.VenueBartenders = venueBartenders;
            }
            else
            {
                ViewBag.ErrorMessage = "Failed to fetch venues employed at.";
            }

            return View(viewModel);
        }


        public ActionResult Error()
        {
            return View();
        }

        private ApplicationDbContext db = new ApplicationDbContext();
>>>>>>> Stashed changes
        public ActionResult New()
        {
            var venues = db.Venues.Select(v => new VenueDto
            {
                VenueID = v.VenueID,
                VenueName = v.VenueName,
                VenueLocation = v.VenueLocation
            }).ToList();

            return View(venues);

        }

        // POST: bartender/create
        [HttpPost]
        public async Task<ActionResult> Create(Bartender bartender)
        {
            Debug.WriteLine("the json payload is :");
            Debug.WriteLine(bartender.FirstName + bartender.LastName);

            string url = "BartenderData/AddBartender";

            string jsonpayload = serializer.Serialize(bartender);
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload, Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = await client.PostAsync(url, content);
            /*
            if (responseMessage.IsSuccessStatusCode)
            {
            */
                return RedirectToAction("List");
            /*
            }
            else
            {
                string error = await responseMessage.Content.ReadAsStringAsync();
                Debug.WriteLine("Error: response message: " + error);
                return RedirectToAction("Error");
            } */
        }

        // GET: bartender/edit/id
        public async Task<ActionResult> Edit(int id)
        {
            string url = $"BartenderData/FindBartender/{id}";
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            BartenderDto selectedBartender = await responseMessage.Content.ReadAsAsync<BartenderDto>();
            return View(selectedBartender);
        }

        // POST: bartender/update/id
        [HttpPost]
        public async Task<ActionResult> Update(int id, Bartender bartender)
        {
            string url = $"BartenderData/UpdateBartender/{id}";
            string jsonpayload = serializer.Serialize(bartender);
            HttpContent content = new StringContent(jsonpayload, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync(url, content);
            Debug.WriteLine(content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                string error = await responseMessage.Content.ReadAsStringAsync();
                Debug.WriteLine("Error: response message: " + error);
                return RedirectToAction("Error");
            }
        }

        // GET: bartender/delete/id
        public async Task<ActionResult> DeleteConfirm(int id)
        {
            string url = $"BartenderData/FindBartender/{id}";
            HttpResponseMessage responseMessage = await client.GetAsync(url);
            BartenderDto selectedBartender = await responseMessage.Content.ReadAsAsync<BartenderDto>();
            return View(selectedBartender);
        }

        // POST: Bartender/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            string url = $"BartenderData/DeleteBartender/{id}";
            HttpContent content = new StringContent("", Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await client.PostAsync(url, content);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                string error = await responseMessage.Content.ReadAsStringAsync();
                Debug.WriteLine("Error: response message: " + error);
                return RedirectToAction("Error");
            }
        }

        //POST: Bartender/Associate/{VenueID}
        [HttpPost]
        public ActionResult AssociateVenue(int id, int VenueID)
        {
            Debug.WriteLine("Attempting to associate bartender :" + id + " with venue " + VenueID);

            string url = "BartenderData/AssociateVenue/" + id + "/" + VenueID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details/" + id);
            }
            else
            {
                ModelState.AddModelError("", "Failed to associate bartender with the venue.");
                return RedirectToAction("Details/" + id);
            }
        }

        [HttpPost]
        public ActionResult UnassociateVenue(int id, int VenueID)
        {
            Debug.WriteLine("Attempting to unassociate bartender :" + id + " with venue: " + VenueID);

            string url = "BartenderData/UnassociateVenue/" + id + "/" + VenueID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details/" + id);
            }
            else
            {
                ModelState.AddModelError("", "Failed to unassociate bartender from the venue.");
                return RedirectToAction("Details/"+ id);
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}