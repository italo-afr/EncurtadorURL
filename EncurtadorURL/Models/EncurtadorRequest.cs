using Postgrest.Attributes;
using Postgrest.Models;

namespace EncurtadorURL.Models
{
    // Esse faz a chamada da API     
    public class EncurtadorRequest
    {
        public string? Url { get; set; }
    }
    
    // Esse é para exibir os dados no formulário
    public class ShortenViewModel
    {
        public string? UrlOriginal { get; set; }
        public string? UrlEncurtada { get; set; }
        public string? MensagemErro { get; set; }
    }
}
