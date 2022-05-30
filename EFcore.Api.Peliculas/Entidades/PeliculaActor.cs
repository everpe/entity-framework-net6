namespace EFcore.Api.Peliculas.Entidades
{
    //Tabla intermedia entre la relación N:N
    public class PeliculaActor
    {
        public int PeliculaId { get; set; }
        public int ActorId { get; set; }
        public string Personaje { get; set; }
        public int Orden { get; set; }
        //Propiedades de navegación para entidades Relacionadas
        public Pelicula Pelicula { get; set; }
        public Actor Actor { get; set; }

    }
}
