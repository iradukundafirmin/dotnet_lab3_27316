using System;

namespace CoworkingApp.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int DeskId { get; set; }
        public string MemberName { get; set; }    // filled by the join query, for display only
        public string DeskName { get; set; }      // filled by the join query, for display only
        public DateTime BookingDate { get; set; }
    }
}
