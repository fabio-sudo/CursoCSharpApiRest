using Microsoft.AspNetCore.Mvc;
using WebApplicationAPI.Interfaces;
using WebApplicationAPI.Model;

namespace WebApplicationAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : Controller
    {

        private readonly IProdutoRepository _produtoRepository;

        public ProdutosController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get()
        {
            var produtos = await _produtoRepository.GetAllAsync();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Produto>> Get(int id)
        {
            var produto = await _produtoRepository.GetByIdAsync(id);
            if (produto == null)
                return NotFound();

            return Ok(produto);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Produto produto)
        {
            await _produtoRepository.AddAsync(produto);
            return CreatedAtAction(nameof(Get), new { id = produto.Id }, produto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Produto produto)
        {
            if (id != produto.Id)
                return BadRequest();

            await _produtoRepository.UpdateAsync(produto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _produtoRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
