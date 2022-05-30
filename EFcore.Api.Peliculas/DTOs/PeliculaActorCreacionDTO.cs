namespace EFcore.Api.Peliculas.DTOs
{
    public class PeliculaActorCreacionDTO
    {
        public int ActorId { get; set; }
        public string Personaje { get; set; }//Nombre de Tabla intermedia N:N  manual
    }
}
