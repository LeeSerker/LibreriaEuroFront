using System;
using System.ComponentModel.DataAnnotations;


namespace LibreriaEuroFront.Models;

public partial class AutorDTO
{
    [Required(ErrorMessage = "El RUT es obligatorio")]
    [RutChileno(ErrorMessage = "El RUT ingresado no es válido")]
    public string Rut { get; set; } = null!;
    [Required]
    public string NombreCompleto { get; set; } = null!;
    [Required]
    public DateTime FechaNacimiento { get; set; }

    public string? CiudadOrigen { get; set; }
    [Required]
    public string Mail { get; set; } = null!;
}
