﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistemas.Datos;
using Sistemas.Entidades.Almacen;
using Sistemas.Web.Models.Almacen.Categoria;

namespace Sistemas.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly DBContextSistema _context;

        public CategoriasController(DBContextSistema context)
        {
            _context = context;
        }

        // GET: api/Categorias/Listar
        [HttpGet("[action]")]
        public async Task <IEnumerable<CategoriaViewModel>> Listar()
        {
            var categoria = await _context.Categorias.ToListAsync();

            return categoria.Select(C => new CategoriaViewModel
            {
                idcategoria =C.idcategoria,
                nombre = C.nombre,
                descripcion = C.descripcion,
                condicion = C.condicion
            });
        }

        // GET: api/Categorias/Mostrar/1
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Mostrar([FromRoute] int id)
        {
                

            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return Ok(new CategoriaViewModel {

                idcategoria = categoria.idcategoria,
                nombre = categoria.nombre,
                descripcion = categoria.descripcion,
                condicion = categoria.condicion

            });
        }

        // PUT: api/Categorias/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] ActualizarViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.idcategoria <= 0)
            {
                return BadRequest();
            }

            var categoria = await _context.Categorias.FirstOrDefaultAsync(c=> c.idcategoria == model.idcategoria);
            if(categoria == null)
            {
                return NotFound(); //pagina error
            }

            categoria.nombre = model.nombre;
            categoria.descripcion = model.descripcion;



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

        // POST: api/Categorias/CrearCategoria
        [HttpPost("[action]")]
        public async Task<IActionResult> CrearCategoria([FromBody] CrearViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Categoria categoria = new Categoria {

                nombre = model.nombre,
                descripcion =model.descripcion,
                condicion = true
               
            };
            _context.Categorias.Add(categoria);
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

        // DELETE: api/Categorias/Eliminar
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Eliminar([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categorias.Remove(categoria);
            try
            {

            }catch(Exception ex)
            {
                return BadRequest();
            }
            await _context.SaveChangesAsync();

            return Ok(categoria);
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.idcategoria == id);
        }
    }
}