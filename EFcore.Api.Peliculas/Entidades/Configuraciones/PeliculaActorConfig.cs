using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFcore.Api.Peliculas.Entidades.Configuraciones
{
    public class PeliculaActorConfig : IEntityTypeConfiguration<PeliculaActor>
    {
        public void Configure(EntityTypeBuilder<PeliculaActor> builder)
        {
            builder.HasKey(p => new { p.PeliculaId, p.ActorId });
            builder.Property(p => p.Personaje).HasMaxLength(150);
        }
    }
}
