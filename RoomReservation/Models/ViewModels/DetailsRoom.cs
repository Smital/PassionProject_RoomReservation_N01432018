using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoomReservation.Models.ViewModels
{
    public class DetailsRoom
    {
        public RoomDto SelectedRoom { get; set; }
        public IEnumerable<BookingDto> ReletedBooking { get; set; }

        public IEnumerable<AmenityDto> AssoAmenities { get; set; }

       

    }
}