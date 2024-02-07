namespace SearchApp.Model.Search
{
    public class Flight
    {
        public string Name { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
        public DateTime Schedule { get; set; }
        public int Price { get; set; }
    }
}
