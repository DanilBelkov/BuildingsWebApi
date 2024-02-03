namespace BuildingsWebApi.Models
{
    public class FlatWithPrice
    {
        public int? Id { get; set; }
        public int CountRooms { get; set; }
        public string? Link { get; set; }
        public float? Area { get; set; }
        public decimal? Price { get; set; }
        public DateTime? Date { get; set; }
    }
}
