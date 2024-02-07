namespace SearchApp.Model.Search
{
    public class Filters
    {
        public string Name { get; set; } = string.Empty;
        public int PriceFrom { get; set; }
        public int PriceTo { get; set; }
        public string Date { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;
    }
}
