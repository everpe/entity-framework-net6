namespace EFcore.Api.Peliculas.DTOs
{
    public class PeliculaCreacionDTO
    {
        public string Titulo { get; set; }
        public bool EnCartelera { get; set; }
        public DateTime FechaEstreno { get; set; }
        public List<int> Generos { get; set; }//guardará ids de generos
        public List<int> SalasDeCine { get; set; }//guardara ids de salasCine
        public List<PeliculaActorCreacionDTO> PeliculasActores { get; set; }
    }
}
