namespace BuildingsWebApi.Parameters
{
    public class Filter
    {
        public int? Id { get; set; }
        public int? CountRooms { get; set; }
        public string? Link { get; set; }
        public float? Area { get; set; }

        public bool IsEmpty => Id == null && CountRooms == null && Link == null && Area == null;
    }
}
