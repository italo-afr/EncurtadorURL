using EncurtadorURL.Models;
using Microsoft.AspNetCore.Mvc;

namespace EncurtadorURL.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly Supabase.Client _supabase;

        public UrlController(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        [HttpPost("encurtar")]
        public async Task<IActionResult> Encurtar([FromBody] EncurtadorRequest request)
        {
            if (string.IsNullOrEmpty(request.UrlOriginal))
                return BadRequest(new { erro = "URL original é obrigatória." });

            // Gerar código aleatório de 6 caracteres  
            var codigo = GerarCodigo(6);

            // Criar objeto para salvar no Supabase  
            var url = new UrlEncurtada
            {
                Codigo = codigo,
                UrlOriginal = request.UrlOriginal
            };

            await _supabase.From<UrlEncurtada>().Insert(url);

            // Retornar a URL encurtada  
            var dominio = $"{Request.Scheme}://{Request.Host}";
            return Ok(new { url = $"{dominio}/{codigo}" });
        }

        // Função para gerar código aleatório  
        private string GerarCodigo(int tamanho)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, tamanho)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
