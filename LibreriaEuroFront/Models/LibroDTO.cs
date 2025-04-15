using System.ComponentModel.DataAnnotations;

namespace LibreriaEuroFront.Models;

public partial class LibroDTO
{
    //public int Id { get; set; }

    [Required]
    public string Titulo { get; set; } = null!;
    [Required]
    public int Anno { get; set; }
    [Required]
    public string Genero { get; set; } = null!;
    [Required]
    public int NumeroDePaginas { get; set; }
    [Required]
    public string RutAutor { get; set; } = null!;
}
