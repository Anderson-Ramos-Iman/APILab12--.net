using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APILab12.Models;

public class Curso
{
    [Key]
    public int IdCurso { get; set; }

    [Required, StringLength(120)]
    public string Nombre { get; set; } = string.Empty;

    [Required, StringLength(500)]
    public string Descripcion { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Precio { get; set; }

    [Range(1, int.MaxValue)]
    public int DuracionHoras { get; set; }

    public int IdInstructor { get; set; }

    public bool Activo { get; set; } = true;

    [JsonIgnore]
    public Instructor? Instructor { get; set; }

    [JsonIgnore]
    public ICollection<Matricula> Matriculas { get; set; } = [];
}
