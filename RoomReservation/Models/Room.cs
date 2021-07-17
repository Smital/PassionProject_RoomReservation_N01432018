using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RoomReservation.Models
{
    public class Room
    {
        [Key]
        public int RoomID { get; set; }
        public int RoomNumber { get; set; }
        public string RoomType { get; set; }
        public int RoomCapacity { get; set; }
        public string RoomStatus { get; set; }

        //Each room has more than one Amenities
        public ICollection<Amenity> Amenities { get; set; }

    }

    public class RoomDto
    {
        public int RoomID { get; set; }
        public int RoomNumber { get; set; }
        public string RoomType { get; set; }
        public int RoomCapacity { get; set; }
        public string RoomStatus { get; set; }
    }
}