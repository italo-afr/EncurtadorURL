using Postgrest.Models;
using Postgrest.Attributes;

namespace EncurtadorURL.Models
{
    [Table("encurtador")]
    public class HomeModel : BaseModel
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("short_code")]
        public string? short_code { get; set; }

        [Column("link_origin")]
        public string? link_origin { get; set; }

        [Column("created_at")]
        public DateTime created_at { get; set; }

        [Column("clicks")]
        public int clicks { get; set; }
    }
}
