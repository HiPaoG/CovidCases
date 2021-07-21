namespace ApiService.Models
{
    public class RegionResponse
    {
        public string iso { get; set; }
        public string name { get; set; }
        public string province { get; set; }
        public int confirmed { get; set; }
        public int deaths { get; set; }
    }
}
