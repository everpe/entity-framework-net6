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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ActoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public ActoresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        //Seleccionar solo algunos datos
        [HttpGet]
        public async Task<IEnumerable<ActorDTO>> Get() {
            var actores = await context.Actores
                .ProjectTo<ActorDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
            return actores;
        }

        [HttpPost]
        public async Task<ActionResult> Post(ActorCreacionDTO actorCreacionDTO)
        {
            var actor = mapper.Map<Actor>(actorCreacionDTO);
            context.Add(actor);
            await context.SaveChangesAsync();
            return Ok();
        }


        //Update Actor MODELO CONECTADO "Edita solo columnas q detecta com cambios con respecto a los que consultó"
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(ActorCreacionDTO actorCreacionDTO, int id)
        {
            var actorDB = await context.Actores.AsTracking().FirstOrDefaultAsync(a => a.Id == id);

            if (actorDB is null)
            {
                return NotFound();
            }

            actorDB = mapper.Map(actorCreacionDTO, actorDB);
            await context.SaveChangesAsync();
            return Ok();
        }

        //Update actor modelo Desconectado
        [HttpPut("desconectado/{id:int}")]
        public async Task<ActionResult> PutDesconectado(ActorCreacionDTO actorCreacionDTO, int id)
        {
            //Forma optimizada de verificar si existe un registro.
            var existeActor = await context.Actores.AnyAsync(a => a.Id == id);

            if (!existeActor)
            {
                return NotFound();
            }

            var actor = mapper.Map<Actor>(actorCreacionDTO);
            actor.Id = id;

            context.Update(actor);
            await context.SaveChangesAsync();
            return Ok();
        }     
        
    }
}
