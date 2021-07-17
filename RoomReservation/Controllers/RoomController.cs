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
    public class RoomController : Controller
    {

        /// <summary>
        /// Create a client once and used again for diffrenet purpose
        /// </summary>
        private static readonly HttpClient client;

        static RoomController()
        {
            client = new HttpClient();
        }

        // GET: Room/List
        /// <summary>
        /// curl "https://localhost:44374/api/RoomData/ListRooms";
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            //Client is the person who accesing any information.
            
            string url = "https://localhost:44374/api/RoomData/ListRooms";
            HttpResponseMessage response = client.GetAsync(url).Result;
            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<RoomDto> Rooms = response.Content.ReadAsAsync<IEnumerable<RoomDto>>().Result;
            Debug.WriteLine("Number of rooms are in the hotel");
            Debug.WriteLine(Rooms.Count());
            return View(Rooms);
        }

        // GET: Room/Details/5
        public ActionResult Details(int id)
        {
            //Client is the person who accesing any information.

            DetailsRoom ViewModel = new DetailsRoom();

            string url = "https://localhost:44374/api/RoomData/ListRooms"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            RoomDto SelectedRoom = response.Content.ReadAsAsync<RoomDto>().Result;

            Debug.WriteLine("Room Number :");
            Debug.WriteLine(SelectedRoom.RoomNumber);
            

            ViewModel.SelectedRoom = SelectedRoom;

            //Showcase information about bookings related to the particular rooms
            //Send a request to gather information about bookings related to particular  room ID

            url = "https://localhost:44374/api/BookingData/ListBookingsForRooms/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<BookingDto> ReletedBooking = response.Content.ReadAsAsync<IEnumerable<BookingDto>>().Result; 
            
            ViewModel.ReletedBooking = ReletedBooking;

            //Show associated amenities with this particular room
            url = "https://localhost:44374/api/AmenitieData/ListAmenitiesForRoom/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<AmenityDto> AssoAmenities = response.Content.ReadAsAsync<IEnumerable<AmenityDto>>().Result;

            ViewModel.AssoAmenities = AssoAmenities;


            return View(ViewModel);
        }

        // GET: Room/New
        public ActionResult New()
        {
            return View();
        }

        // POST: Room/Create
        [HttpPost]
        public ActionResult Create(Room room)
        {
            Debug.WriteLine("Jason Payload is : ");
            Debug.WriteLine("The inputed Room number is : ");
            Debug.WriteLine(room.RoomNumber);

            //Add a new booking into the system using API
            //curl -H "Content-Type:application/json" -d @room.json https://localhost:44374/api/RoomData/AddRoom

            string url = "https://localhost:44374/api/RoomData/AddRoom";
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(room);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            client.PostAsync(url, content);

            return RedirectToAction("List");
        }

        // GET: Room/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Room/Edit/5
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

        // GET: Room/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Room/Delete/5
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
