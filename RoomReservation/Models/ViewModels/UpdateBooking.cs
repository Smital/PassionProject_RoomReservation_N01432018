using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoomReservation.Models.ViewModels
{
    public class UpdateBooking
    {
        //This viewmodel is a class which stores information that we need to present to /Booking/Update/{}
        //The existing information

        public BookingDto SelectedBooking { get; set; }
        //Any room numbers to choose from when updating the bookings

        public IEnumerable<RoomDto> RoomOptions { get; set; }
    }
}