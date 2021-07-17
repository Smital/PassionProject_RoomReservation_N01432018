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
    public class RoomDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/RoomData/ListRooms
        [HttpGet]
        public IEnumerable<RoomDto> ListRooms()
        {
            List<Room> Rooms = db.Rooms.ToList();
            List<RoomDto> RoomDtos = new List<RoomDto>();

            Rooms.ForEach(a => RoomDtos.Add(new RoomDto()
            {
                RoomID = a.RoomID,
                RoomNumber = a.RoomNumber,
                RoomType = a.RoomType,
                RoomCapacity = a.RoomCapacity,
                RoomStatus = a.RoomStatus
            }));
            return RoomDtos; 
        }

        /// <summary>
        /// Gather information about rooms related to particular amenity
        /// </summary>
        /// <returns>
        /// Actual data transfer object
        /// </returns>
        /// Content: All rooms in the database which associated with particular amenity that match to a particular 
        /// amenity ID
        /// <param name="id">AmenityID </param>
        /// <example>
        /// GET:api/RoomData/ListRoomsForAmenity/1
        /// </example>

        [HttpGet]
        [ResponseType(typeof(RoomDto))]
        public IEnumerable<RoomDto> ListRoomsForAmenity(int id)
        {
            //All Rooms that contains amenity which matches with ID
            List<Room> Rooms = db.Rooms.Where(
                    r => r.Amenities.Any(
                    a => a.AmenityID==id)).ToList();
            List<RoomDto> RoomDtos = new List<RoomDto>();

            Rooms.ForEach(r => RoomDtos.Add(new RoomDto()
            {
                RoomID = r.RoomID,
                RoomType = r.RoomType,
                RoomCapacity = r.RoomCapacity,
                RoomStatus = r.RoomStatus,
                RoomNumber = r.RoomNumber,
            }));

            return RoomDtos;
        }

        /// <summary>
        ///  Associates a particular Room with a particular Amenity
        /// </summary>
        /// <param name="RoomID">The RoomID is the primary key</param>
        /// <param name="AmenityID">The AmenityID is the primary key</param>
        /// <returns>
        /// HEADER : 
        /// </returns>
        
        [HttpPost]
        [Route("api/RoomData/AssociateRoomToAmenity/{RoomID}/{AmenityID}")]
        public IHttpActionResult AssociateRoomToAmenity(int RoomID,int AmenityID)
        {
            Room SelectedRoom = db.Rooms.Include(r => r.Amenities).Where(r=>r.RoomID == RoomID).FirstOrDefault();
            Amenity SelectedAmenity = db.Amenities.Find(AmenityID);

            if(SelectedRoom == null || SelectedAmenity == null)
            {
                return NotFound();
            }

            SelectedRoom.Amenities.Add(SelectedAmenity);
            db.SaveChanges();

            return Ok();
        }



        /// <summary>
        ///  Remove an Association to a particular Room with a particular Amenity
        /// </summary>
        /// <param name="RoomID">The RoomID is the primary key</param>
        /// <param name="AmenityID">The AmenityID is the primary key</param>
        /// <returns>
        /// HEADER : 
        /// </returns>
        /// <example>
        /// POST: api/RoomData/UnAssociateRoomToAmenity/{RoomID}/{AmenityID}
        /// </example>

        [HttpPost]
        [Route("api/RoomData/UnAssociateRoomToAmenity/{RoomID}/{AmenityID}")]
        public IHttpActionResult UnAssociateRoomToAmenity(int RoomID, int AmenityID)
        {
            Room SelectedRoom = db.Rooms.Include(r => r.Amenities).Where(r => r.RoomID == RoomID).FirstOrDefault();
            Amenity SelectedAmenity = db.Amenities.Find(AmenityID);

            if (SelectedRoom == null || SelectedAmenity == null)
            {
                return NotFound();
            }

            SelectedRoom.Amenities.Remove(SelectedAmenity);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Gather information about rooms related to particular amenity
        /// </summary>
        /// <returns>
        /// Actual data transfer object
        /// </returns>
        /// Content: All rooms which are not in the database which associated with particular amenity that match to a particular 
        /// amenity ID
        /// <param name="id">AmenityID </param>
        /// <example>
        /// GET:api/RoomData/ListRoomsWithNotAssoAmenity/1
        /// </example>

        [HttpGet]
        [ResponseType(typeof(RoomDto))]
        public IEnumerable<RoomDto> ListRoomsWithNotAssoAmenity(int id)
        {
            //All Rooms that contains amenity which matches with ID
            List<Room> Rooms = db.Rooms.Where(
                    r => !r.Amenities.Any(
                       a => a.AmenityID == id)).ToList();
            List<RoomDto> RoomDtos = new List<RoomDto>();

            Rooms.ForEach(r => RoomDtos.Add(new RoomDto()
            {
                RoomID = r.RoomID,
                RoomType = r.RoomType,
                RoomCapacity = r.RoomCapacity,
                RoomStatus = r.RoomStatus,
                RoomNumber = r.RoomNumber,
            }));

            return RoomDtos;
        }

        // GET: api/RoomData/FindRoom/5
        [ResponseType(typeof(Room))]
        [HttpGet]
        public IHttpActionResult FindRoom(int id)
        {
            Room Room = db.Rooms.Find(id);
            RoomDto RoomDto = new RoomDto()
            {
                RoomID = Room.RoomID,
                RoomNumber = Room.RoomNumber,
                RoomType = Room.RoomType,
                RoomCapacity = Room.RoomCapacity,
                RoomStatus = Room.RoomStatus
            };

            if (Room == null)
            {
                return NotFound();
            }

            return Ok(Room);
        }

        // POST: api/RoomData/UpdateRoom/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateRoom(int id, Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != room.RoomID)
            {
                return BadRequest();
            }

            db.Entry(room).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(id))
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

        // POST: api/RoomData/AddRoom
        [ResponseType(typeof(Room))]
        [HttpPost]
        public IHttpActionResult AddRoom(Room room)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Rooms.Add(room);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = room.RoomID }, room);
        }

        // DELETE: api/RoomData/DeleteRoom/5
        [ResponseType(typeof(Room))]
        [HttpPost]
        public IHttpActionResult DeleteRoom(int id)
        {
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            db.Rooms.Remove(room);
            db.SaveChanges();

            return Ok(room);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool RoomExists(int id)
        {
            return db.Rooms.Count(e => e.RoomID == id) > 0;
        }
    }
}