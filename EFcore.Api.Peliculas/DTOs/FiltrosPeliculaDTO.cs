namespace EFcore.Api.Peliculas.DTOs
{
    public class FiltrosPeliculaDTO
    {
        public string Titulo { get; set; }
        public int GeneroId { get; set; }
        public bool EnCartelera { get; set; }
        public bool ProximosEstrenos { get; set; }
    }
}
