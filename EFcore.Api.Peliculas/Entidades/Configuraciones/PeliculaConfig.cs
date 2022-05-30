using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFcore.Api.Peliculas.Entidades.Configuraciones
{
    public class PeliculaConfig : IEntityTypeConfiguration<Pelicula>
    {
        public void Configure(EntityTypeBuilder<Pelicula> builder)
        {
            builder.Property(p => p.Titulo)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(p => p.PosterUrl)
               .HasMaxLength(500)
               .IsUnicode(false);//tildes, ñ etc...
        }
    }
}
