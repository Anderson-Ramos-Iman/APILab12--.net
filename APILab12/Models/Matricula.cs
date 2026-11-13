using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APILab12.Models;

public class Matricula
{
    [Key]
    public int IdMatricula { get; set; }

    public int IdEstudiante { get; set; }

    public int IdCurso { get; set; }

    public DateTime FechaMatricula { get; set; } = DateTime.UtcNow;

    [Required, StringLength(30)]
    public string Estado { get; set; } = "Pendiente";

    [Range(0, double.MaxValue)]
    public decimal MontoTotal { get; set; }

    [JsonIgnore]
    public Estudiante? Estudiante { get; set; }

    [JsonIgnore]
    public Curso? Curso { get; set; }

    [JsonIgnore]
    public ICollection<Pago> Pagos { get; set; } = [];
}
