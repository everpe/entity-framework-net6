using System.ComponentModel.DataAnnotations.Schema;

namespace EFcore.Api.Peliculas.Entidades
{
    public class Log
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]//Para q no genere el Id al insertarse automaticamente.
        public Guid Id { get; set; }
        public string Mensaje { get; set; }
    }
}
