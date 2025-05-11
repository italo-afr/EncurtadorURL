using System.Diagnostics;
using EncurtadorURL.Models;
using Microsoft.AspNetCore.Mvc;
using Supabase;
using Microsoft.Extensions.Logging;

namespace EncurtadorURL.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Supabase.Client _supabase;

        public HomeController(ILogger<HomeController> logger, Supabase.Client supabase)
        {
            _logger = logger;
            _supabase = supabase;
        }

        private static string GenerateShortCode(int length = 6)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Endpoint para API externa (Postman, por exemplo)
        [HttpPost("api/shorten")]
        public async Task<IActionResult> Shorten([FromBody] ShortenRequest request)
        {
            if (string.IsNullOrEmpty(request.Url))
                return BadRequest(new { message = "A URL é obrigatória" });

            string shortCode;
            bool codeExists;

            do
            {
                shortCode = GenerateShortCode();
                var existing = await _supabase
                    .From<HomeModel>()
                    .Filter("codigo", Postgrest.Constants.Operator.Equals, shortCode.ToLower())
                    .Get();

                codeExists = existing.Models.Any();
            }
            while (codeExists);

            var newLink = new HomeModel
            {
                short_code = shortCode.ToLower(),
                link_origin = request.Url,
                created_at = DateTime.UtcNow
            };

            await _supabase.From<HomeModel>().Insert(newLink);

            return Ok(new
            {
                url = $"http://short.local/{shortCode}"
            });
        }

        // Redirecionamento de links encurtados (acesso pelo navegador)
        [Route("/{shortCode}")]
        [HttpGet]
        public async Task<IActionResult> RedirectToOriginal(string shortCode)
        {
            var result = await _supabase
                .From<HomeModel>()
                .Filter("codigo", Postgrest.Constants.Operator.Equals, shortCode.ToLower())
                .Get();

            var link = result.Models.FirstOrDefault();

            if (link == null)
                return NotFound("Link encurtado não encontrado");

            return Redirect(link.link_origin);
        }

        // Tela inicial (GET)
        [HttpGet]
        public IActionResult Index()
        {
            return View(new ShortenViewModel());
        }

        // Recebe o formulário e salva (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ShortenViewModel model)
        {
            if (string.IsNullOrEmpty(model.UrlOriginal))
            {
                model.MensagemErro = "A URL é obrigatória";
                return View(model);
            }

            string shortCode;
            bool codeExists;

            do
            {
                shortCode = GenerateShortCode();
                var existing = await _supabase
                    .From<HomeModel>()
                    .Filter("codigo", Postgrest.Constants.Operator.Equals, shortCode.ToLower())
                    .Get();

                codeExists = existing.Models.Any();
            }
            while (codeExists);

            var newLink = new HomeModel
            {
                short_code = shortCode.ToLower(),
                link_origin = model.UrlOriginal,
                created_at = DateTime.UtcNow,
            };

            await _supabase.From<HomeModel>().Insert(newLink);

            model.UrlEncurtada = $"http://short.local/{shortCode}";
            return View(model);
        }
    }
}
