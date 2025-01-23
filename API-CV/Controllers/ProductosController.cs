using API_CV.BdContext;
using API_CV.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_CV.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("[controller]")]
    [Authorize]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly PruebaApiRestContext _context;
        public ProductosController(PruebaApiRestContext context)
        {
            _context = context;
            
        }

        [HttpGet]
        [Route("ListarProductos")]

        public async Task<ActionResult<IEnumerable<Producto>>> GetProducts()
        {
            try
            {
                var respuesta = await _context.Productos.ToListAsync();


                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = respuesta });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });

            }
        }

        [HttpPost]
        [Route("CrearProductos")]
        public async Task<ActionResult<IEnumerable<Producto>>> PostProducts(List<Producto> products)
        {
            try
            {
                _context.Productos.AddRange(products);
                var responses = await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProduct), new { id = products[0].IdProducto }, products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
            }
        }

        [HttpGet]
        [Route("GetById/{id:int}")]
        public async Task<ActionResult<Producto>> GetProduct(int id)
        {
            var product = await _context.Productos.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
        [HttpPut]
        [Route("Edit/{id:int}")]
        public async Task<IActionResult> PutProduct(int id, Producto product)
        {
            var existingProduct = await _context.Productos.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound(new { mensaje = "Producto no encontrado." });
            }
            try
            {

                if (!string.IsNullOrEmpty(product.CodigoBarra))
                    existingProduct.CodigoBarra = product.CodigoBarra;

                if (!string.IsNullOrEmpty(product.Marca))
                    existingProduct.Marca = product.Marca;

                if (!string.IsNullOrEmpty(product.Nombre))
                    existingProduct.Nombre = product.Nombre;

                if (!string.IsNullOrEmpty(product.Categoria))
                    existingProduct.Categoria = product.Categoria;

                if (product.Precio.HasValue)
                    existingProduct.Precio = product.Precio;

                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Si se edito prro" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });



            }
        }

        [HttpDelete]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Productos.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { mensaje = "producto no emcontrado" });
            }
            try
            {
                _context.Productos.Remove(product);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "Si se elimino prro" });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });


            }
        }




    }
}

