using EFcore.Api.Peliculas.Entidades;
using EFcore.Api.Peliculas.Entidades.Configuraciones;
using EFcore.Api.Peliculas.Entidades.Seeding;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EFcore.Api.Peliculas.Application
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }


        //Convenciones de comportamiento Default de EntityFrameworkCore
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
            //SIEMPRE QUE SE AGREGUE UN CAMPO DATETIME SE MAEPA A DATE POR DEFECTO
            configurationBuilder.Properties<DateTime>().HaveColumnType("date");
        }



        //Configuración ApiFluent de EFCore
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<OfertaCine>().Property(p => p.FechaInicio)
            //    .HasColumnType("date");//Solo fecha sin hora
            //Genero (Aplicando config de validaciones) individualmente
            modelBuilder.ApplyConfiguration(new GeneroConfig());
            //Lee Config de Validaciones que haya en todo el proyecto(Grupal)
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());


            modelBuilder.Entity<Log>().Property(l => l.Id).ValueGeneratedNever();//para q no genere La clave primaria automaticamente BD


            SeedingModuloConsulta.Seed(modelBuilder);
            //modelBuilder.Entity<Log>().Property(l => l.Id).ValueGeneratedNever();//Ignore esa propiedad´para BD
            //modelBuilder.Ignore<Direccion>();//Ignore Esa Clase para BD


        }

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Cine> Cines { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<OfertaCine> OfertasCines { get; set; }
        public DbSet<SalaCine> SalasCine { get; set; }
        public DbSet<PeliculaActor> PeliculasActores { get; set; }
        public DbSet<Log> Logs { get; set; }


    }
}
