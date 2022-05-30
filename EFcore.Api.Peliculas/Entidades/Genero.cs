using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EFcore.Api.Peliculas.Entidades
{
    //[Index(nameof(Nombre), IsUnique = true)]
    public class Genero
    {
        //[key] // configuración primaryKey por atributo
        public int Identificador { get; set; }
        //[StringLength(150)] o [MaxLength(150)]
        //[Required]
        //[Column("NombreGenero")]
        public string Nombre { get; set; }

        //Relación N:N de forma Autómatica
        public HashSet<Pelicula> Peliculas { get; set; }
        //PAra borrado suave
        public bool EstaBorrado { get; set; }
    }
}
