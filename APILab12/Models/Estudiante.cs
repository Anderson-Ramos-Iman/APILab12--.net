using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APILab12.Models;

public class Estudiante
{
    [Key]
    public int IdEstudiante { get; set; }

    [Required, StringLength(100)]
    public string Nombres { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Apellidos { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, Phone, StringLength(20)]
    public string Telefono { get; set; } = string.Empty;

    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public bool Activo { get; set; } = true;

    [JsonIgnore]
    public ICollection<Matricula> Matriculas { get; set; } = [];
}
