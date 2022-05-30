using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFcore.Api.Peliculas.Application;
using EFcore.Api.Peliculas.DTOs;
using EFcore.Api.Peliculas.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace EFcore.Api.Peliculas.Controllers
{
    [Route("api/cines")]
    [ApiController]
    public class CinesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public CinesController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;   
        }
        [HttpGet]
        public async Task<IEnumerable<CineDTO>> Get()
        {
            return await context.Cines.ProjectTo<CineDTO>(mapper.ConfigurationProvider).ToListAsync();
        }

        [HttpGet("cercanos")]
        public async Task<ActionResult> Get(double latitud, double longitud)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            var miUbicacion = geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate(longitud, latitud));

            var distanciaMaxima = 2000;
            var cines = await context.Cines
                        .OrderBy(c => c.Ubicacion.Distance(miUbicacion))
                        .Where(c => c.Ubicacion.IsWithinDistance(miUbicacion, distanciaMaxima))
                        .Select( c => new
                        {
                            Nombre = c.Nombre,
                            Distacia = Math.Round(c.Ubicacion.Distance(miUbicacion)) //Calcula la distancia entre miUbicacion y el Cine
                        }).ToListAsync();
            return Ok(cines);
        }


        //Crear cine con su data Relacionada(Simulando inputs de cliente)
        [HttpPost("createCine")]
        public async Task<ActionResult> Post()
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var ubicacionCine = geometryFactory.CreatePoint(new Coordinate(-69.896979, 18.476276));

            var cine = new Cine()
            {
                Nombre = "Mi cine",
                Ubicacion = ubicacionCine,
                OfertaCine = new OfertaCine()
                {
                    PorcentajeDescuento = 5,
                    FechaInicio = DateTime.Today,
                    FechaFin = DateTime.Today.AddDays(7)
                },
                SalasCine = new HashSet<SalaCine>()
                {
                    new SalaCine()
                    {
                        Precio = 200,
                        TipoSalaCine = TipoSalaCine.DosDimensiones
                    },
                    new SalaCine()
                    {
                        Precio = 350,
                        TipoSalaCine = TipoSalaCine.TresDimensiones
                    }
                }
            };

            context.Add(cine);
            await context.SaveChangesAsync();
            return Ok();
        }
        //Crear Cine con DTO
        [HttpPost("conDTO")]
        public async Task<ActionResult> Post(CineCreacionDTO cineCreacionDTO)
        {
            //Mapea de un Dto hacia un Cine() para guardar
            var cine = mapper.Map<Cine>(cineCreacionDTO);
            context.Add(cine);
            await context.SaveChangesAsync();
            return Ok();
        }

    }
}
