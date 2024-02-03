namespace BuildingsWebApi.Models
{
    public class PriceHistory
    {
        public int Id { get; set; }
        public int FlatId { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

    }
}
