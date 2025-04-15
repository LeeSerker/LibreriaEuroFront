using Microsoft.AspNetCore.Mvc;
using LibreriaEuroFront.Models;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Net.Http;

namespace LibreriaEuro.FrontMVC.Controllers
{
    public class AutoresController : Controller
    {
        private readonly HttpClient _httpClient;

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly string _apiLink = "https://localhost:7016/api/";

        public AutoresController(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
            _httpClient = clientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(_apiLink);
        }

        public async Task<IActionResult> Index()
        {
            var autores = await _httpClient.GetFromJsonAsync<List<AutorDTO>>("Autores");
            return View(autores);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(AutorDTO autor)
        {
            // Validación (opcional)
             if (!ModelState.IsValid)
                return View(autor);

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("https://localhost:7016/api/");
            var response = await httpClient.PostAsJsonAsync("Autores", autor);

            if (response.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "Autor creado correctamente.";
                return RedirectToAction("Index");
            }

            // Mostrar errores si falla
            ModelState.AddModelError("", "Error al guardar el autor.");
            return View(autor);
        }

        [HttpGet]
        public async Task<IActionResult> Buscar(string? rut, string? nombreCompleto)
        {
            var query = "Buscar?";

            if (!string.IsNullOrEmpty(rut))
                query += $"rut={rut}&";

            if (!string.IsNullOrEmpty(nombreCompleto))
                query += $"nombreCompleto={nombreCompleto}&";

            var response = await _httpClient.GetAsync($"Autores/{query}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var autores = JsonConvert.DeserializeObject<List<AutorDTO>>(json);
                return View("_ResultadoBusqueda", autores);
            }

            return View("_ResultadoBusqueda", new List<AutorDTO>());
        }

    }
}