namespace WebApi.Models
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; } = true;

        // Nullable Image property - This will store the image path or URL.
        public string? ImagePath { get; set; }  // Nullable to handle missing or incorrect paths.

        public List<Booking>? Bookings { get; set; } = new List<Booking>();
    }
}
