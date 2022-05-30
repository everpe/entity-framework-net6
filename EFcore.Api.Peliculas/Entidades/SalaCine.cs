namespace EFcore.Api.Peliculas.Entidades
{
    public class SalaCine
    {
        public int Id { get; set; }
        public TipoSalaCine TipoSalaCine { get; set; }
        //[Precision(precision: 9, scale:2)]
        public decimal Precio { get; set; }
        //Foranea de Cine 1:N
        public int CineId { get; set; }
        public Cine Cine { get; set; }
        //Relación N:N
        public HashSet<Pelicula> Peliculas { get; set; }
    }
}
