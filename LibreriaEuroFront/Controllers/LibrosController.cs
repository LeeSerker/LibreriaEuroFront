using Microsoft.AspNetCore.Mvc;
using LibreriaEuroFront.Models;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Net.Http;

namespace LibreriaEuro.FrontMVC.Controllers
{
    public class LibrosController : Controller
    {
        private readonly HttpClient _httpClient;

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly string _apiLink = "https://localhost:7016/api/";

        public LibrosController(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
            _httpClient = clientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(_apiLink);
        }

        public async Task<IActionResult> Index()
        {
            var libros = await _httpClient.GetFromJsonAsync<List<LibroDTO>>("Libros");
            return View(libros);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(LibroDTO libro)
        {
            // Validación (opcional)
            if (!ModelState.IsValid)
                return View(libro);

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri("https://localhost:7016/api/");
            var response = await httpClient.PostAsJsonAsync("Libros", libro);

            if (response.IsSuccessStatusCode)
            {
                TempData["MensajeExito"] = "Libro creado correctamente.";
                return RedirectToAction("Index");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                var salidaError = "";

                if (errorMessage == "AutorNoExiste")
                {
                    
                    salidaError = "El rut ingresado no está registrado como autor.";
                }

                if (errorMessage == "TopeLibros")
                {
                    salidaError = "No es posible registrar el libro, se alcanzó el máximo permitido.";
                }
                ModelState.AddModelError("", salidaError);
                return View(libro);
            }
        }


        /*public async Task<IActionResult> Buscar(string? rutAutor, string? autor, string? titulo, int? anno)
        {
            using var http = new HttpClient();
            if (anno <= 0)
            {
                anno = null;
            }
            var url = $"{_apiLink}api/libro/buscar?rutAutor={rutAutor}&autor={autor}&titulo={titulo}&año={anno}";

            var libros = await http.GetFromJsonAsync<List<LibroDTO>>(url);
            return View("_Buscador", libros);
        }*/

        [HttpGet]
        public async Task<IActionResult> Buscar(string? rutAutor, string? nombreAutor, string? titulo, int? anno)
        {
            var query = $"Buscar?";

            if (!string.IsNullOrEmpty(rutAutor))
                query += $"rutAutor={rutAutor}&";

            if (!string.IsNullOrEmpty(nombreAutor))
                query += $"nombreAutor={nombreAutor}&";

            if (!string.IsNullOrEmpty(titulo))
                query += $"titulo={titulo}&";

            if (anno.HasValue && anno > 0)
                query += $"anno={anno}";

            var response = await _httpClient.GetAsync($"Libros/{query}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var libros = JsonConvert.DeserializeObject<List<LibroDTO>>(json);
                return View("_ResultadoBusqueda", libros);
            }

            return View("_ResultadoBusqueda", new List<LibroDTO>());
        }
    }
}