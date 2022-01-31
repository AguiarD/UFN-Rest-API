using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationAulaRestAPI.Data;
using WebApplicationAulaRestAPI.Models;

namespace WebApplicationAulaRestAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api")]  // url base (url base-> http://localhost:5852; controller-> api/pessoas; endpoint-> todas)
    [ApiController]
    public class PessoaController : ControllerBase
    {
        //Contexto contexto; // Gerar uma instancia de contexto mas como criei em ... não precisa mais

        [HttpGet]
        [Route("pessoas")]
        public async Task<IActionResult> getAllAsync([FromServices] Contexto contexto)
        {
            var pessoas = await contexto
                .Pessoas
                .AsNoTracking() //Regra para colocar em consultas. Vai trazer os dados de forma mais limpa. Usa menos recurso - Altamente recomendado por questoões de desempenho
                .ToListAsync();  //De imediato não vai retornar os dados. So retorna quando tiverem prontos.

            //if (pessoas == null)
            //{
            //    return NotFound();
            //}
            //else
            //{
            //    return Ok(pessoas);
            //}

            return pessoas == null ? NotFound() : Ok(pessoas);
        }

        [HttpGet]
        [Route("pessoas/{id}")]
        public async Task<IActionResult> getByIdAsync(
            [FromServices] Contexto contexto,
            [FromRoute] int id
            )
        {
            var pessoa = await contexto
                .Pessoas.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            return pessoa == null ? NotFound() : Ok(pessoa); //OPERADOR TERNARIO
        }

        [HttpPost]
        [Route("pessoas")]
        public async Task<IActionResult> PostAsync(
            [FromServices] Contexto contexto,
            [FromBody] Pessoa pessoa
            )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                await contexto.Pessoas.AddAsync(pessoa);
                await contexto.SaveChangesAsync();
                return Created($"api/pessoas/{pessoa.Id}", pessoa);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("pessoas/{id}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] Contexto contexto,
            [FromBody] Pessoa pessoa,
            [FromRoute] int id
            )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var p = await contexto.Pessoas
                .FirstOrDefaultAsync(x => x.Id == id);

            if (p == null)
            {
                return NotFound("Pessoa não encontrada!");
            }

            try
            {
                p.Nome = pessoa.Nome;

                contexto.Pessoas.Update(p);
                await contexto.SaveChangesAsync();
                return Ok(p);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("pessoas/{id}")]
        public async Task<IActionResult> DeleteAsync(
            [FromServices] Contexto contexto,
            [FromRoute] int id
            )
        {
            var p = await contexto.Pessoas.FirstOrDefaultAsync(x => x.Id == id);

            if (p == null)
            {
                return BadRequest("Pessoa não encontrada");
            }

            try
            {
                contexto.Pessoas.Remove(p);
                await contexto.SaveChangesAsync();

                return Ok(p);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


    }
}
