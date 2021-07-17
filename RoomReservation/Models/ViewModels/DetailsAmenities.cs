using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoomReservation.Models.ViewModels
{
    public class DetailsAmenities
    {
        public AmenityDto SelectedAmenity { get; set; }
        public IEnumerable<RoomDto> AssoRooms { get; set; }

        public IEnumerable<RoomDto> AvailableRooms { get; set; }
    }
}