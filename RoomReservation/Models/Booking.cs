using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RoomReservation.Models
{
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }
        public string BookingName { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime DateOut { get; set; }


        /// <summary>
        /// Connect Booking table to Room table  because the
        /// a room can be share by two person at the same time
        /// So,Booking has the connection with the Room.
        /// RoomID will be the foreign key.
        /// </summary>
        /// 
        //Specify that this Foreign key is of Room entity
        //A room associated with more than one person.
        //One person belongs to one room.
        [ForeignKey("Room")]
        public int RoomID { get; set; }

        //Model Room associated with particular Room which the booking table used
        public virtual Room Room { get; set; }
    }


    public class BookingDto
    {
        public int BookingID { get; set; }
        public string BookingName { get; set; }
        public DateTime DateIn { get; set; }
        public DateTime DateOut { get; set; }

        public int RoomNumber { get; set; }
    }
}