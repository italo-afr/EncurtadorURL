namespace EncurtadorURL.Models
{
    // Esse é usado no endpoint /api/shorten
    public class ShortenRequest
    {
        public string? Url { get; set; }
    }
}