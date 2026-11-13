using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APILab12.Models;

public class Instructor
{
    [Key]
    public int IdInstructor { get; set; }

    [Required, StringLength(100)]
    public string Nombres { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string Apellidos { get; set; } = string.Empty;

    [Required, StringLength(120)]
    public string Especialidad { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(150)]
    public string Email { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;

    [JsonIgnore]
    public ICollection<Curso> Cursos { get; set; } = [];
}
