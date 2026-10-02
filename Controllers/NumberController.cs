using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumberController (NumbersService numbersService): ControllerBase
    {
       // private readonly NumbersService _numbersService;
        //servicio
       // public NumberController(NumbersService numbersService)
        //{  _numbersService = numbersService; }   

        [HttpPost("calcular/{numero:int}")]
        public async Task <IActionResult> Calcular(int numero)
        {
            var resultado = numero + numero;
            var calculo = new NumberRecordSet(numero, resultado);

            int nuevoId = await numbersService.SaveAsync(calculo);
            var record = await numbersService.GetByIdAsync(nuevoId);
            return CreatedAtAction(nameof(GetById), new { id = nuevoId }, record);
        }

        //historial completo
        [HttpGet("historial")]
        public async Task<ActionResult<IEnumerable<NumberRecordGet>>> GetHistorial()
        {
            var records = await numbersService.GetListAsync();
            return Ok(records);
        }

        //consulta de calculo por id
        [HttpGet("{id:int}")]
        public async Task<ActionResult<NumberRecordGet>> GetById(int id)
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
        public async Task<ActionResult> Update(int id, [FromBody] NumberRecordSet record)
        {

            var resultado = record.Numero + record.Numero;
            var registroActualizado = new NumberRecordSet(record.Numero, resultado);

            var updated = await numbersService.UpdateAsync(id, registroActualizado);
            if (!updated)
            { 
                return NotFound(new { message = $"No se encontró el registro con Id {id} para actualizar." }); 
            }

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
