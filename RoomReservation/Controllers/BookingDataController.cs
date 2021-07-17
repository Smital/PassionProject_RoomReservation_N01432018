using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RoomReservation.Models;

namespace RoomReservation.Controllers
{
    public class BookingDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all bookings in the system
        /// </summary>
        /// <returns>
        /// Actual data transfer object
        /// All bookings in the system related to their associated room.
        /// </returns>
        /// <example>
        /// GET: api/BookingData/ListBookings
        /// </example>
        [HttpGet]
        [ResponseType(typeof(Booking))]
        public IEnumerable<BookingDto> ListBookings()
        {
            List<Booking> Bookings = db.Bookings.ToList();
            List<BookingDto> BookingDtos = new List<BookingDto>();

            Bookings.ForEach(a => BookingDtos.Add(new BookingDto()
            {
                BookingID = a.BookingID,
                BookingName = a.BookingName,
                DateIn = a.DateIn,
                DateOut = a.DateOut,
                RoomNumber = a.Room.RoomNumber
            }));
            return BookingDtos;
        }

        /// <summary>
        /// Gathers information about all bookings to a particular room ID
        /// </summary>
        /// <returns>
        /// Actual data transfer object
        /// </returns>
        /// <param name="id">RoomID</param>
        /// <example>
        ///  GET: api/BookingData/ListBookingsForRooms/3
        /// </example>
        [HttpGet]
        [ResponseType(typeof(BookingDto))]
        public IEnumerable<BookingDto> ListBookingsForRooms(int id)
        {
            List<Booking> Bookings = db.Bookings.Where(b=>b.RoomID==id).ToList();
            List<BookingDto> BookingDtos = new List<BookingDto>();

            Bookings.ForEach(b => BookingDtos.Add(new BookingDto()
            {
                BookingID = b.BookingID,
                BookingName = b.BookingName,
                DateIn = b.DateIn,
                DateOut = b.DateOut,
                RoomNumber = b.Room.RoomNumber
            }));

            return BookingDtos;
        }



        // GET: api/BookingData/FindBooking/2
        [ResponseType(typeof(BookingDto))]
        [HttpGet]
        public IHttpActionResult FindBooking(int id)
        {
            Booking Booking = db.Bookings.Find(id);
            BookingDto BookingDto = new BookingDto()
            {
               BookingID = Booking.BookingID,
                
                BookingName = Booking.BookingName,
                DateIn = Booking.DateIn,
                DateOut = Booking.DateOut,
                RoomNumber = Booking.Room.RoomNumber
            };

            if (Booking == null)
            {
                return NotFound();
            }

            return Ok(BookingDto);
        }

        // POST: api/BookingData/UpdateBooking/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateBooking(int id, Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != booking.BookingID)
            {
                return BadRequest();
            }

            db.Entry(booking).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/BookingData/AddBooking
        [ResponseType(typeof(Booking))]
        [HttpPost]
        public IHttpActionResult AddBooking(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Bookings.Add(booking);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = booking.BookingID }, booking);
        }

        // DELETE: api/BookingData/DeleteBooking/5
        [ResponseType(typeof(Booking))]
        [HttpPost]
        public IHttpActionResult DeleteBooking(int id)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }

            db.Bookings.Remove(booking);
            db.SaveChanges();

            return Ok(booking);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BookingExists(int id)
        {
            return db.Bookings.Count(e => e.BookingID == id) > 0;
        }
    }
}