using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFcore.Api.Peliculas.Entidades.Configuraciones
{
    public class GeneroConfig : IEntityTypeConfiguration<Genero>
    {
        public void Configure(EntityTypeBuilder<Genero> builder)
        {
            builder.HasKey(prop => prop.Identificador);
            builder.Property(p => p.Nombre)
                .HasMaxLength(150)
                .IsRequired();
            builder.HasQueryFilter(g => !g.EstaBorrado);//Filtro A nivel de Modelo para que nunca muestre los q están Borrados.

            builder.HasIndex(g => g.Nombre).IsUnique().HasFilter("EstaBorrado = 'false'");//Hace Único ese campo solo teniendo en cuenta los NoBorrados
        }
    }
}
