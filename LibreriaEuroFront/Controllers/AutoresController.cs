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

        // GET: Autores/Modificar/rut
        [HttpGet]
        public async Task<IActionResult> Modificar(string rutAutor)
        {
            var response = await _httpClient.GetAsync($"Autores/{rutAutor}");
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var json = await response.Content.ReadAsStringAsync();
            var autor = JsonConvert.DeserializeObject<AutorDTO>(json);

            return View(autor);
        }

        // POST: Autores/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Modificar(string rutAutor, AutorDTO autor)
        {
            if (!ModelState.IsValid)
                return View(autor);

            var response = await _httpClient.PutAsJsonAsync($"Autores/{rutAutor}", autor);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, $"Error: {error}");
            return View(autor);
        }

        // GET: Autores/Eliminar/rut
        [HttpGet]
        public async Task<IActionResult> Eliminar(string rutAutor)
        {
            var response = await _httpClient.GetAsync($"Autores/{rutAutor}");
            if (!response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var autor = JsonConvert.DeserializeObject<AutorDTO>(await response.Content.ReadAsStringAsync());
            return View(autor);
        }

        // POST: Autores/Eliminar/rut
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(string rutAutor)
        {
            var response = await _httpClient.DeleteAsync($"Autores/{rutAutor}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ModelState.AddModelError(string.Empty, "Error al eliminar el autor.");
            return RedirectToAction("Index");
        }


    }
}