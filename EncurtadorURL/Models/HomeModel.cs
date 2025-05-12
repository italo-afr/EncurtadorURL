using Postgrest.Attributes;
using Postgrest.Models;
using System;

namespace EncurtadorURL.Models
{
    // Esse model é usado para mapear a tabela "urls" no banco de dados no Supabase
    [Table("urls")]
    public class HomeModel : BaseModel
    {
        [PrimaryKey("id", false)]
        public int Id { get; set; }

        [Column("codigo")] 
        public string? short_code { get; set; }

        [Column("url_original")]
        public string? link_origin { get; set; }

        [Column("criado_em")]
        public DateTime created_at { get; set; }

        [Column("expira_em")]
        public DateTime? expires_at { get; set; }
    }
}
