using System.ComponentModel.DataAnnotations;

namespace EFcore.Api.Peliculas.DTOs
{
    //DTo para creación de Cine con su Oferta.
    public class OfertaCineCreacionDTO
    {

        [Range(1, 100)]
        public double PorcentajeDescuento { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
