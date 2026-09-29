using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumberController : ControllerBase
    {
        [HttpGet("numero/{numero:int}")]
        public IActionResult Numero(int numero)
        {
            return Ok(numero + numero);
        }
    }
}
