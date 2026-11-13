using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APILab12.Models;

public class Pago
{
    [Key]
    public int IdPago { get; set; }

    public int IdMatricula { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Monto { get; set; }

    public DateTime FechaPago { get; set; } = DateTime.UtcNow;

    [Required, StringLength(50)]
    public string MetodoPago { get; set; } = string.Empty;

    [Required, StringLength(30)]
    public string EstadoPago { get; set; } = "Pendiente";

    [JsonIgnore]
    public Matricula? Matricula { get; set; }
}
