using Microsoft.EntityFrameworkCore;

namespace EFcore.Api.Peliculas.Entidades
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public bool EnCartelera { get; set; }
        public DateTime FechaEstreno { get; set; }
        [Unicode(false)]
        public string PosterUrl  { get; set; }
        //Relación N:N de forma Autómatica
        public List<Genero> Generos { get; set; }

        //Relación N:N forma Autómatica
        public List<SalaCine> SalasCine { get; set; }


        //Relación N:N De forma Manual
        public List<PeliculaActor> PeliculasActores { get; set; }
    }
}
