namespace CoworkingApp.Models
{
    public class Desk
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }          // Hot desk, Private office, Meeting room
        public decimal PricePerDay { get; set; }
    }
}
