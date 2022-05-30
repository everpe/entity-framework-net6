using System.ComponentModel.DataAnnotations;

namespace EFcore.Api.Peliculas.DTOs
{
    public class CineCreacionDTO
    {
        [Required]
        public string Nombre { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public OfertaCineCreacionDTO OfertaCine { get; set; }
        public SalaCineCreacionDTO[] SalasCine { get; set; }
    }
}
