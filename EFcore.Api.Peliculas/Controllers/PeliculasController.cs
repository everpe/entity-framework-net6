using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFcore.Api.Peliculas.Application;
using EFcore.Api.Peliculas.DTOs;
using EFcore.Api.Peliculas.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFcore.Api.Peliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public PeliculasController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        //CARGAR RELACIONES CON EagerLaoding Include() 
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PeliculaDTO>> Get(int id)
        {
            //EAGER LOADING
            var pelicula = await context.Peliculas
                .Include(p => p.Generos.OrderByDescending(g => g.Nombre))//Ordenar Lista
                .Include(p => p.SalasCine)
                    .ThenInclude(sala => sala.Cine) //para incluir el Cine que tiene cad SalaCiene dentro de Pelicula
                .Include(p => p.PeliculasActores.Where(pa => pa.Actor.FechaNacimiento.Value.Year >= 1980))//Filtrar Actores mayores a 1980
                    .ThenInclude(pAc => pAc.Actor) //Incluir Actores dentro de PeliculasActores
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula is null)
            {
                return NotFound();
            }
            var peliculaDto = mapper.Map<PeliculaDTO>(pelicula);

            //PAra quitar cines repetidos
            peliculaDto.Cines = peliculaDto.Cines.DistinctBy(x => x.Id).ToList();
            return peliculaDto;
        }



        //Cargar entidades relacionadas con ProjectTo
        [HttpGet("conprojectto/{id:int}")]
        public async Task<ActionResult<PeliculaDTO>> GetProjectTo(int id)
        {
            var pelicula = await context.Peliculas
                .ProjectTo<PeliculaDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula is null)
            {
                return NotFound();
            }

            pelicula.Cines = pelicula.Cines.DistinctBy(x => x.Id).ToList();

            return pelicula;
        }

        //Cargar entidades relacionadas con Select
        [HttpGet("cargadoselectivo/{id:int}")]
        public async Task<ActionResult> GetSelectivo(int id)
        {
            var pelicula = await context.Peliculas.Select(p =>
            new
            {
                Id = p.Id,
                Titulo = p.Titulo,
                Generos = p.Generos.OrderByDescending(g => g.Nombre).Select(g => g.Nombre).ToList(),
                NombresActores = p.PeliculasActores.Select(pa => pa.Actor).Select(g => g.Nombre).ToList(),
                CantidadCines = p.SalasCine.Select(s => s.CineId).Distinct().Count()
            }).FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula is null)
            {
                return NotFound();
            }

            return Ok(pelicula);
        }

        //Explicit loading
        [HttpGet("cargadoexplicito/{id:int}")]
        public async Task<ActionResult<PeliculaDTO>> GetExplicito(int id)
        {
            var pelicula = await context.Peliculas.AsTracking().FirstOrDefaultAsync(p => p.Id == id);
            //...

            //await context.Entry(pelicula).Collection(p => p.Generos).LoadAsync();

            var cantidadGeneros = await context.Entry(pelicula).Collection(p => p.Generos).Query().CountAsync();

            if (pelicula is null)
            {
                return NotFound();
            }

            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);

            return peliculaDTO;
        }


        [HttpGet("lazyloading/{id:int}")]
        public async Task<ActionResult<List<PeliculaDTO>>> GetLazyLoading()
        {
            var peliculas = await context.Peliculas.AsTracking().ToListAsync();

            foreach (var pelicula in peliculas)
            {
                // cargar los generos de la pelicula

                // Problema n + 1
                pelicula.Generos.ToList();
            }

            var peliculasDTOs = mapper.Map<List<PeliculaDTO>>(peliculas);
            return peliculasDTOs;
        }






        //Consultas Agrupadas, 
        //Agrupar Peliculas dependiendo si están en Cartelera o no("2 Posibles Grupos")
        [HttpGet("agrupadasPorEstreno")]
        public async Task<ActionResult> GroupPeliculasByCartelera()
        {
            var peliculasAgrupadas = await context.Peliculas.GroupBy(p => p.EnCartelera)//Agrupa en un mismo lugar los registros q esten o No, en cartelera
                                            .Select(group => new //Selecciona lo que se quiere mostrar en cada registro agrupado
                                            {
                                                EnCartelera = group.Key,//propiedad EnCartelera de Pelicula del GroupBy.
                                                TotalAgrupado = group.Count(),//Total de Peliculas  de cada agrupación
                                                GrupoPeliculasCartelera = group.Select( pg => new { titulo = pg.Titulo, salasCine = pg.SalasCine }).ToList()//Todas las peliculas q están en cada grupo y de cada una saco el titulo y sus Salas.
                                            }).ToListAsync();
            return Ok(peliculasAgrupadas);
        }


        //Agrupación por el número de géneros de cada pelicula.
        [HttpGet("agrupadasPorCantidadGeneros")]
        public async Task<ActionResult> GroupPeliculasByCantidadGeneros()
        {
            var peliculasAgrupadas = await context.Peliculas.GroupBy(p => p.Generos.Count())//Agrupa Pelis por Cantidad de generos
                .Select(group => new
                {
                    CantidadPeliculas = group.Key,
                    Titulos = group.Select( pGroup => pGroup.Titulo),
                    Generos = group.Select( pGroup => pGroup.Generos).SelectMany( gList => gList).Select( genero => new {genero.Nombre}).Distinct(),
                }).ToListAsync();
            return Ok(peliculasAgrupadas);
        }



        //Filtros de Pelicula con Ejecucion Diferida
        [HttpGet("filtros")]
        public async Task<ActionResult<List<PeliculaDTO>>> FiltrosToPelicula(
                    [FromQuery]FiltrosPeliculaDTO peliculasFiltroDTO)
        {
            //Instacia Quyery que pérmitira agregar filtros y ejecutarse una sola vez luego
            var peliculasQueryable = context.Peliculas.AsQueryable();

            if (!string.IsNullOrEmpty(peliculasFiltroDTO.Titulo))
            {
                //Lista de peliculas que contengas en Titulo que manden en Filtro
                peliculasQueryable = peliculasQueryable.Where( p => p.Titulo.Contains(peliculasFiltroDTO.Titulo));
            }

            if (peliculasFiltroDTO.EnCartelera)
            {
                peliculasQueryable = peliculasQueryable.Where(p => p.EnCartelera);
            }

            if (peliculasFiltroDTO.ProximosEstrenos)
            {
                var hoy = DateTime.Today;
                peliculasQueryable = peliculasQueryable.Where(p => p.FechaEstreno > hoy);
            }

            if (peliculasFiltroDTO.GeneroId != 0)
            {
                //BUSCA EN LA RELACION INTEREDIA DE N:N, aquellas peliculas que en alguno de sus generos tenga el del filtro.
                peliculasQueryable = peliculasQueryable.Where( p =>
                     p.Generos.Select(genero => genero.Identificador)//selecciona identificadores de cada genero
                                .Contains(peliculasFiltroDTO.GeneroId)//devuelve todos los generos si alguno de esos contiene
                );
            }

            var peliculas = await peliculasQueryable.Include(p => p.Generos).ToListAsync();
            return mapper.Map<List<PeliculaDTO>>(peliculas);
        }




        //Crear pelicula con generos y Salas ya en BD
        [HttpPost]
        public async Task<ActionResult> Post(PeliculaCreacionDTO peliculaCreacionDTO)
        {
            var pelicula = mapper.Map<Pelicula>(peliculaCreacionDTO);
            pelicula.Generos.ForEach(g => context.Entry(g).State = EntityState.Unchanged);//marca sin seguimiento generos (Desconectado)
            pelicula.SalasCine.ForEach(s => context.Entry(s).State = EntityState.Unchanged);//marca sin seguimiento SalasCines

            if (pelicula.PeliculasActores is not null)
            {
                for (int i = 0; i < pelicula.PeliculasActores.Count; i++)
                {
                    pelicula.PeliculasActores[i].Orden = i + 1;
                }
            }

            context.Add(pelicula);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
