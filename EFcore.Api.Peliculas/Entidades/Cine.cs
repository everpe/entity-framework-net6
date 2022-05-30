using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace EFcore.Api.Peliculas.Entidades
{
    public class Cine
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
      
        public Point Ubicacion { get; set; }

        //1:1 Propiedad de navegación para cuando se necesite mostrar oferta de un cine.
        public OfertaCine OfertaCine { get; set; }    
        
        //Relación 1:N con  Salas
        public HashSet<SalaCine> SalasCine { get; set; }
    }
}
