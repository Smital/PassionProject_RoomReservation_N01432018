 using RoomReservation.Models;
using RoomReservation.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace RoomReservation.Controllers
{
    public class AmenitieController : Controller
    {

        /// <summary>
        /// Create a client once and used again for diffrenet purpose
        /// </summary>
        private static readonly HttpClient client;

        static AmenitieController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44374/api/AmenitieData/");
        }
        // GET: Amenitie
        /// <summary>
        /// curl "https://localhost:44374/api/AmenitieData/ListAmenities";
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            //Client is the person who accesing any information.
            
            string url = "ListAmenities";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);


            IEnumerable<AmenityDto> Amenities = response.Content.ReadAsAsync<IEnumerable<AmenityDto>>().Result;
            Debug.WriteLine("Number of Amenities are in the Rooms");
            Debug.WriteLine(Amenities.Count());
            return View(Amenities);
            
        }

        // GET: Amenitie/Details/5
        public ActionResult Details(int id)
        {
            DetailsAmenities ViewModel = new DetailsAmenities();
            //Client is the person who accesing any information.
            
            string url = "https://localhost:44374/api/AmenitieData/FindAmenities/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);


            AmenityDto SelectedAmenity = response.Content.ReadAsAsync<AmenityDto>().Result;
            Debug.WriteLine("Amenity Name : ");
            Debug.WriteLine(SelectedAmenity.AmenityName);
          
            ViewModel.SelectedAmenity = SelectedAmenity;

            //Show all rooms associated with a particular amenity
            url = "https://localhost:44374/api/RoomData/ListRoomsForAmenity/" + id;
            response = client.GetAsync(url).Result; ;
            IEnumerable<RoomDto> AssoRooms = response.Content.ReadAsAsync<IEnumerable<RoomDto>>().Result;

            ViewModel.AssoRooms = AssoRooms;


            //Show all rooms which is not associated with a particular amenity
            url = "https://localhost:44374/api/RoomData/ListRoomsWithNotAssoAmenity/" + id;
            response = client.GetAsync(url).Result; ;
            IEnumerable<RoomDto> AvailableRooms = response.Content.ReadAsAsync<IEnumerable<RoomDto>>().Result;

            ViewModel.AvailableRooms = AvailableRooms;

            return View(ViewModel);
        }

        //POST: Amenitie/Associate/{AmenityID}
        [HttpPost]
        public ActionResult Associate(int id,int AmenityID) 
        {
            Debug.WriteLine("Attempting to associate room "+ id +" with the amenity "+ AmenityID);

            //call our api to associate room with amenity
            string url = "https://localhost:44374/api/RoomData/AssociateRoomToAmenity/" + id + "/" + AmenityID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url,content).Result;

            return RedirectToAction("Amenitie/Details/" + id);
        }

        //POST: Amenitie/UnAssociate/{AmenityID}
        [HttpPost]
        public ActionResult UnAssociate(int id, int AmenityID)
        {
            Debug.WriteLine("Attempting to associate room " + id + " with the amenity " + AmenityID);

            //call our api to associate room with amenity
            string url = "https://localhost:44374/api/RoomData/UnAssociateRoomToAmenity/" + id + "/" + AmenityID;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            return RedirectToAction("Amenitie/Details/" + id);
        }

        // GET: Amenitie/New
        [HttpGet]
        public ActionResult New()
        {
            return View();
        }

        // POST: Amenitie/Create
        [HttpPost]
        public ActionResult Create(Amenity amenity)
        {
            Debug.WriteLine("Jason Payload is : ");
            Debug.WriteLine("The inputed Amenity name is : ");
            Debug.WriteLine(amenity.AmenityName);

            //Add a new booking into the system using API
            //curl -H "Content-Type:application/json" -d @amenity.json https://localhost:44374/api/AmenitieData/AddAmenity

            string url = "https://localhost:44374/api/AmenitieData/AddAmenity";
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(amenity);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            client.PostAsync(url, content);
            return RedirectToAction("List");
        }

        // GET: Amenitie/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Amenitie/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Amenitie/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Amenitie/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
