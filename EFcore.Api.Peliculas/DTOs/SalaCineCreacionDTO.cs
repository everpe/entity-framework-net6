using EFcore.Api.Peliculas.Entidades;

namespace EFcore.Api.Peliculas.DTOs
{
    //Dto para Crear SalaCine dentro de Cine.
    public class SalaCineCreacionDTO
    {
        public int Id { get; set; }
        public decimal Precio { get; set; }
        public TipoSalaCine TipoSalaCine { get; set; }
    }
}
