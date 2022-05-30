using EFcore.Api.Peliculas.Application;
using EFcore.Api.Peliculas.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFcore.Api.Peliculas.Controllers
{
    [Route("api/generos/[action]")]
    [ApiController]
    public class GenerosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public GenerosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<Genero>> GetAllGeneros()
        {
            context.Logs.Add(new Log
            {
                Id = Guid.NewGuid(),
                Mensaje = "Ejecutando el método GenerosController.Get al consultar generos"
            });
            await context.SaveChangesAsync();
            return await context.Generos.OrderBy(x => x.Nombre).ToListAsync();
        }
        [HttpGet]
        public async Task<ActionResult<Genero>> FirstGenero()
        {
            //Primer Género cuyo Nombre comienza con C
            //return await context.Generos.FirstAsync(x => x.Nombre.StartsWith("C"));//devuelve error sino encuentra

            //devuelve null sino encuentra
            var genero = await context.Generos.FirstOrDefaultAsync(x => x.Nombre.StartsWith("Z"));
            if (genero is null)
            {
                return NotFound();
            }
            return genero;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Genero>> GetGeneroById(int id)
        {
            var genero = await context.Generos.FirstOrDefaultAsync(x => x.Identificador == id);
            if (genero is null)
            {
                return NotFound();
            }
            return genero;
        }
        //Order By
        [HttpGet]
        public async Task<IEnumerable<Genero>> AllGenerosByContainName(string nombre)
        {
            return await context.Generos
                .Where(x => x.Nombre.Contains(nombre))
                .OrderBy(x => x.Nombre)
                .ToListAsync();
        }

        //Paginación
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genero>>> Pagination(int pagina)
        {
            //Salte el primer registro al buscar y tome 2 registros siguientes.
            //var generos = await context.Generos.Skip(1).Take(2).ToListAsync();

            //Implementation Pagination simple
            var registersPerPage = 2;
            var generos = await context.Generos
                .Skip((pagina - 1) * registersPerPage)
                .Take(registersPerPage)
                .ToListAsync();

            if (generos is null)
            {
                return NotFound();
            }
            return generos;
        }


        //Agregar un genero(M.Desconectado)
        [HttpPost]
        public async Task<ActionResult> CreateGenero(Genero genero)
        {
            var existName = await context.Generos.AnyAsync(g => g.Nombre == genero.Nombre);  //Detached sin seguimiento

            if (existName)
            {
                return BadRequest("Ya existe  un genero con el mismo nombre.");
            }
    
            
            context.Add(genero);//<==>this.context.Generos.Add(genero);  
            await context.SaveChangesAsync();
            return Ok(genero);
        }

        //Agregar Varios generos
        [HttpPost]
        public async Task<ActionResult> CreateManyGeneros(Genero[] generos)
        {
            context.AddRange(generos);//Los marca todos added
            await context.SaveChangesAsync();
            return Ok();
        }


        //Actualizar nombre genero(Modelo Conectado)
        [HttpPut]
        public async Task<ActionResult> UpdateNameGeneroConectado(int id)
        {
            var genero = await context.Generos.AsTracking().FirstOrDefaultAsync(g => g.Identificador == id);

            if (genero is null)
            {
                return NotFound();
            }

            genero.Nombre += " 3";
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var genero = await context.Generos.FirstOrDefaultAsync(g => g.Identificador == id);

            if (genero is null)
            {
                return NotFound();
            }

            context.Remove(genero);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("borradoSuave/{id:int}")]
        public async Task<ActionResult> DeleteSuave(int id)
        {
            var genero = await context.Generos.AsTracking().FirstOrDefaultAsync(g => g.Identificador == id);

            if (genero is null)
            {
                return NotFound();
            }

            genero.EstaBorrado = true;
            await context.SaveChangesAsync();
            return Ok();
        }
        //Quitar borrado suave

        [HttpPost("Restaurar/{id:int}")]
        public async Task<ActionResult> Restaurar(int id)
        {
            var genero = await context.Generos.AsTracking()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(g => g.Identificador == id);

            if (genero is null)
            {
                return NotFound();
            }

            genero.EstaBorrado = false;
            await context.SaveChangesAsync();
            return Ok();
        }

    }
}
