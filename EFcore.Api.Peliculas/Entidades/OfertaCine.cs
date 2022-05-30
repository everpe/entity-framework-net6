namespace EFcore.Api.Peliculas.Entidades
{
    public class OfertaCine//Promoción
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        //Relation 1:1
        public int CineId { get; set; }

    }
}
