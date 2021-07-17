using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RoomReservation.Models;

namespace RoomReservation.Controllers
{
    public class AmenitieDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <example>
        /// GET: api/AmenitieData/ListAmenities
        /// </example>
        [HttpGet]
        public IEnumerable<AmenityDto> ListAmenities()
        {
            List<Amenity> Amenities = db.Amenities.ToList();
            List<AmenityDto> AmenityDtos = new List<AmenityDto>();

            Amenities.ForEach(a => AmenityDtos.Add(new AmenityDto()
            {
                AmenityID = a.AmenityID,
                AmenityName = a.AmenityName,
            }));
            return AmenityDtos;
        }

        /// <summary>
        /// Returns all amenities in the system
        /// </summary>
        /// Contents: all amentities in the database,associated with the room
        /// <returns>
        /// </returns>
        /// <param name="id">Room Primary Key</param>
        /// <example>
        /// GET: api/AmenitieData/ListAmenitiesForRoom/1
        /// </example>
        [HttpGet]
        [ResponseType(typeof(AmenityDto))]
        public IEnumerable<AmenityDto> ListAmenitiesForRoom(int id)
        {
            List<Amenity> Amenities = db.Amenities.Where(
                a=> a.Rooms.Any(
                    r=>r.RoomID==id)).
                ToList();
            List<AmenityDto> AmenityDtos = new List<AmenityDto>();

            Amenities.ForEach(a => AmenityDtos.Add(new AmenityDto()
            {
                AmenityID = a.AmenityID,
                AmenityName = a.AmenityName,
            }));
            return AmenityDtos;
        }


        // GET: api/AmenitieData/FindAmenity/5
        [ResponseType(typeof(Amenity))]
        [HttpGet]
        public IHttpActionResult FindAmenity(int id)
        {
            Amenity Amenity = db.Amenities.Find(id);
            AmenityDto AmenityDto = new AmenityDto()
            {
                AmenityID = Amenity.AmenityID,
                AmenityName = Amenity.AmenityName,
            };
            if (Amenity == null)
            {
                return NotFound();
            }

            return Ok(Amenity);
        }

        // POST: api/AmenitieData/UpdateAmenity/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAmenity(int id, Amenity amenity)
        {
            Debug.WriteLine("I have reached the update room amenity method");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != amenity.AmenityID)
            {
                return BadRequest();
            }

            db.Entry(amenity).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AmenityExists(id))
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

        // POST: api/AmenitieData/AddAmenity
        [ResponseType(typeof(Amenity))]
        [HttpPost]
        public IHttpActionResult AddAmenity(Amenity amenity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Amenities.Add(amenity);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = amenity.AmenityID }, amenity);
        }

        // DELETE: api/AmenitieData/DeleteAmenity/5
        [ResponseType(typeof(Amenity))]
        [HttpPost]
        public IHttpActionResult DeleteAmenity(int id)
        {
            Amenity amenity = db.Amenities.Find(id);
            if (amenity == null)
            {
                return NotFound();
            }

            db.Amenities.Remove(amenity);
            db.SaveChanges();

            return Ok(amenity);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AmenityExists(int id)
        {
            return db.Amenities.Count(e => e.AmenityID == id) > 0;
        }
    }
}