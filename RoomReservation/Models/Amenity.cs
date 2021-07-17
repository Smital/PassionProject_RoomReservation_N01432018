using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoomReservation.Models
{
    public class Amenity
    {
        [Key]
        public int AmenityID { get; set; }
        public string AmenityName { get; set; }

        //An Amenity is associated with many rooms
        public ICollection<Room> Rooms { get; set; }

    }

    public class AmenityDto
    {
        public int AmenityID { get; set; }
        public string AmenityName { get; set; }

    }
}