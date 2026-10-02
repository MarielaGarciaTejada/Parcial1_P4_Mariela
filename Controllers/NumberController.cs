using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Namespace
{
    [Route("api/[controller]")]
    [ApiController]  // Primary constructor
    public class NumberController(NumbersService numbersService) : ControllerBase
    {
        //private readonly NumbersService _numbersService;
        // servicio
        //public NumberController(NumbersService numbersService)
        //{  _numbersService = numbersService; }   

        [HttpPost("calcular/{numero:int}")]
        public async Task <IActionResult> Calcular(int numero)
        {
            var record = new NumberRecord
            {
                Numero = numero,
                Resultado = numero + numero,
                Fecha = DateTime.Now
            };

            int nuevoId = await numbersService.SaveAsync(record);
            record.Id = nuevoId;
            return CreatedAtAction(nameof(GetById), new { id = nuevoId }, record);
        }

        //historial completo
        [HttpGet("historial")]
        public async Task<ActionResult<IEnumerable<NumberRecord>>> GetHistorial()
        {
            var records = await numbersService.GetListAsync();
            return Ok(records);
        }

        //consulta de calculo por id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<NumberRecord>> GetById(int id)
        {
            var record = await numbersService.GetByIdAsync(id);
            if(record is null)
            {
                return NotFound(new { message = $"No se encontró el registro con ID {id}." });
            }
            return Ok(record);
        }


        //actualiza
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update(int id, [FromBody] NumberRecord record)
        {
            if (id != record.Id)
            { return BadRequest(new { message = "El Id de la URL no coincide con el id de la petición." }); }

            record.Resultado = record.Numero + record.Numero;
            record.Fecha = DateTime.Now;

            var updated = await numbersService.UpdateAsync(record);
            if (!updated)
            { return NotFound(new { message = $"No se encontró el registro con Id {id} para actualizar." }); }

            return Ok(new { message = $"El registro con Id {id} fue actualizado correctamente." }); 
        }

        //elimina
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await numbersService.GetByIdAsync(id);
            if (record is null)
            { return NotFound(new { message = $"No se encontró el registro con Id {id} para eliminar." }); }

            await numbersService.DeleteAsync(id);
            return Ok(new { message = $"El registro con Id {id} fue eliminado correctamente." });
        }

        
    }
}
