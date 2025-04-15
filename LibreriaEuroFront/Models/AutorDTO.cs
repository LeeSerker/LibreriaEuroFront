namespace LibreriaEuroFront.Models;
using System;
using System.ComponentModel.DataAnnotations;

public partial class AutorDTO
{
    //public int Id { get; set; }
    [Required]
    public string Rut { get; set; } = null!;
    [Required]
    public string NombreCompleto { get; set; } = null!;
    [Required]
    public DateTime FechaNacimiento { get; set; }

    public string? CiudadOrigen { get; set; }
    [Required]
    public string Mail { get; set; } = null!;
}
