using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistemas.Datos;
using Sistemas.Entidades.Usuarios;
using Sistemas.Web.Models.Usuarios.Persona;

namespace Sistemas.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly DBContextSistema _context;

        public PersonasController(DBContextSistema context)
        {
            _context = context;
        }

        // GET: api/Personas/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<PersonaViewModel>> Listar()
        {
            var persona = await _context.Personas.ToListAsync();

            return persona.Select(P => new PersonaViewModel
            {
                idpersona = P.idpersona,
                tipopersona = P.tipopersona,
                nombre = P.nombre,
                tipodocumento =P.tipodocumento,
                num_documentoi=P.num_documentoi,
                direccion =P.direccion,
                telefono=P.telefono,
                email=P.email
            });
        }

        // GET: api/Personas/Mostrar/1
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Mostrar([FromRoute] int id)
        {
          

            var persona = await _context.Personas.FindAsync(id);

            if (persona == null)
            {
                return NotFound();
            }

            return Ok(new PersonaViewModel
            {

                idpersona = persona.idpersona,
                tipopersona = persona.tipopersona,
                nombre = persona.nombre,
                tipodocumento = persona.tipodocumento,
                num_documentoi = persona.num_documentoi,
                direccion = persona.direccion,
                telefono = persona.telefono,
                email = persona.email

            });
        }

        // PUT: api/Personas/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.idpersona <=0)
            {
                return BadRequest();
            }

            var persona = await _context.Personas.FirstOrDefaultAsync(p => p.idpersona == model.idpersona);
            if (persona == null)
            {
                return NotFound(); //pagina error
            }

            persona.tipopersona = model.tipopersona;
            persona.nombre = model.nombre;
            persona.tipodocumento = model.tipodocumento;
            persona.num_documentoi = model.num_documentoi;
            persona.direccion = model.direccion;
            persona.telefono = model.telefono;
            persona.email = model.email;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //guardar la excepcio 
                return BadRequest();
            }

            return Ok();
        }

        // POST: api/Personas/CrearPersona
        [HttpPost("[action]")]
        public async Task<IActionResult> CrearPersona([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Persona persona = new Persona
            {

             
               tipopersona = model.tipopersona,
               nombre = model.nombre,
               tipodocumento = model.tipodocumento,
               num_documentoi = model.num_documentoi,
               direccion = model.direccion,
               telefono = model.telefono,
               email = model.email

        };
            _context.Personas.Add(persona);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok();
        }

        // DELETE: api/Personas/Eliminar
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Eliminar([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var persona = await _context.Personas.FindAsync(id);
            if (persona == null)
            {
                return NotFound();
            }

            _context.Personas.Remove(persona);
            try
            {

            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            await _context.SaveChangesAsync();

            return Ok(persona);
        }

        private bool PersonaExists(int id)
        {
            return _context.Personas.Any(e => e.idpersona == id);
        }
    }
}