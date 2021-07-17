using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using RoomReservation.Models;
using RoomReservation.Models.ViewModels;

namespace RoomReservation.Controllers
{
    public class BookingController : Controller
    {
        /// <summary>
        /// Create a client once and used again for diffrenet purpose
        /// </summary>
        private static readonly HttpClient client;

        public object HttpContent { get; private set; }

        static BookingController()
        {
            client = new HttpClient();

            
        }

        // GET: Booking/List
        /// <summary>
        /// Communicate with our booking data api to retrieve a list of bookings
        /// curl https://localhost:44374/api/BookingData/ListBookings
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            //Client is the person who accesing any information.
   
            string url = "https://localhost:44374/api/BookingData/ListBookings";
            HttpResponseMessage response = client.GetAsync(url).Result;
            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            IEnumerable<BookingDto> Bookings = response.Content.ReadAsAsync<IEnumerable<BookingDto>>().Result;
            Debug.WriteLine("Number of bookings received");
            Debug.WriteLine(Bookings.Count());
            return View(Bookings);

        }

        // GET: Booking/Details/5
        public ActionResult Details(int id)
        {
            //Communicate with our booking data api to retrieve one booking details
            
            string url = "https://localhost:44374/api/BookingData/FindBooking/"+id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            Debug.WriteLine("The response code is");
            Debug.WriteLine(response.StatusCode);

            BookingDto selectedBooking = response.Content.ReadAsAsync<BookingDto>().Result;
            Debug.WriteLine("Booking Name :");
            Debug.WriteLine(selectedBooking.BookingName);
            return View(selectedBooking);
            
        }
        /// <summary>
        /// Diffirenciate the method Create and New.
        /// New is use to get the data from the user through the form
        /// Create will actually create the booking
        /// </summary>
        /// <returns></returns>

        // GET: Booking/New
        [HttpGet]
        public ActionResult New()
        {
            //information about all the rooms in the system
            //GET api/RoomData/ListRooms
            string url = "https://localhost:44374/api/RoomData/ListRooms";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<RoomDto> RoomsOptions = response.Content.ReadAsAsync<IEnumerable<RoomDto>>().Result;

            return View(RoomsOptions);
        }

        // POST: Booking/Create
        [HttpPost]
        public ActionResult Create(Booking booking)
        {
            Debug.WriteLine("Jason Payload is : ");
            Debug.WriteLine("The inputed Booking name is : ");
            Debug.WriteLine(booking.BookingName);

            //Add a new booking into the system using API
            //curl -H "Content-Type:application/json" -d @booking.json https://localhost:44374/api/BookingData/AddBooking

            string url = "https://localhost:44374/api/BookingData/AddBooking";
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string jsonpayload = jss.Serialize(booking);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            client.PostAsync(url, content);
            return RedirectToAction("List");
        }

        // GET: Booking/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateBooking ViewModel = new UpdateBooking();
            //The existing booking informations
            string url = "https://localhost:44374/api/BookingData/FindBooking/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BookingDto SelectedBooking = response.Content.ReadAsAsync<BookingDto>().Result;
            ViewModel.SelectedBooking = SelectedBooking;


            //Also like to include all room number to choose from when updating this booking
            url = "https://localhost:44374/api/RoomData/ListRooms/";
            response = client.GetAsync(url).Result;
            IEnumerable<RoomDto> RoomOptions = response.Content.ReadAsAsync<IEnumerable<RoomDto>>().Result;
            ViewModel.RoomOptions = RoomOptions;

            return View(ViewModel);
        }

        // POST: Booking/Update/5
        [HttpPost]
        public ActionResult Update(int id, Booking booking)
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

        // GET: Booking/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Booking/Delete/5
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
