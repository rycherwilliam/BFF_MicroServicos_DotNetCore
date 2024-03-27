using Microsoft.AspNetCore.Mvc;
using RotasService.Interfaces;
using RotasService.Entities;

[ApiController]
[Route("api/[controller]")]
public class RotasController : ControllerBase
{
    private readonly IRotaService _rotaService;

    public RotasController(IRotaService rotaService)
    {        
        _rotaService = rotaService;
    }

    [HttpGet("rotas-disponiveis")]
    public ActionResult<IEnumerable<string>> ObterRotasDisponiveis()
    {
        var rotasDisponiveis = _rotaService.ObterRotas();
        return Ok(rotasDisponiveis);
    }

    [HttpGet("{origem}/{destino}")]
    public ActionResult<RotaResultadoEntity> ObterMelhorRota(string origem, string destino)
    {
        var rotas = _rotaService.ObterRotas();
        var melhorRota = _rotaService.EncontrarMelhorRota(rotas,origem, destino);
        if (melhorRota == null)
        {
            return NotFound();
        }
        return Ok(melhorRota);
    }   
}