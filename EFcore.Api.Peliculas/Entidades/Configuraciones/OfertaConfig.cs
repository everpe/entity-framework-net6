using Microsoft.EntityFrameworkCore;

namespace EFcore.Api.Peliculas.Entidades.Configuraciones
{
    public class OfertaConfig : IEntityTypeConfiguration<OfertaCine>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<OfertaCine> builder)
        {
            builder.Property(p => p.PorcentajeDescuento)
                .HasPrecision(precision: 5, scale: 2);
        }
    }
}
