using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using crud_web_api_mvc.Data;
using crud_web_api_mvc.Models;
using crud_web_api_mvc.ViewModels;

namespace crud_web_api_mvc.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ClientesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<Cliente>>> GetAll()
    {
        try
        {
            var clientes = await _context.Clientes.ToListAsync();
            
            return Ok(new ResultViewModel<List<Cliente>>(clientes));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Cliente>>("Falha interna no servidor!"));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cliente>> Get(int id)
    {
        try
        {
            var cliente = await _context.Clientes.FindAsync(id);
            
            if (cliente is null) return NotFound(new ResultViewModel<Cliente>("Cliente não encontrado!"));

            return Ok(new ResultViewModel<Cliente>(cliente));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Cliente>>("Falha interna no servidor!"));
        }
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> Post(Cliente model)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new ResultViewModel<Cliente>(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));

            await _context.Clientes.AddAsync(model);
            await _context.SaveChangesAsync();

            return Created($"/v1/cliente/{model.Id}", new ResultViewModel<Cliente>(model));
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Cliente>("Não foi possível incluir o cliente!"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<Cliente>("Falha interna no servidor!"));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Cliente>> Put(int id, Cliente model)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(new ResultViewModel<Cliente>(ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));

            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente is null) return NotFound(new ResultViewModel<Cliente>("Cliente não encontrado!"));

            cliente.Nome = model.Nome;
            cliente.Email = model.Email;

            _context.Update(cliente);
            await _context.SaveChangesAsync();

            return Ok(new ResultViewModel<Cliente>(cliente));
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Cliente>("Não foi possível incluir o cliente!"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<Cliente>("Falha interna no servidor!"));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            if (await _context.Clientes.FindAsync(id) is Cliente cliente)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
                
                return Ok(new ResultViewModel<Cliente>(cliente));
            }

            return NotFound(new ResultViewModel<Cliente>("Cliente não encontrado!"));
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Cliente>("Não foi possível excluir o cliente!"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<Cliente>("Falha interna no servidor!"));
        }
    }
}
