using Postgrest.Attributes;
using Postgrest.Models;

namespace EncurtadorURL.Models
{
    [Table("urls")]
    public class UrlEncurtada : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }
        [Column("codigo")]
        public string? Codigo { get; set; }
        [Column("url_original")]
        public string? UrlOriginal { get; set; }
        [Column("criado_em")]
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}
