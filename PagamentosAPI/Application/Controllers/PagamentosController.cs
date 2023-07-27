using Microsoft.AspNetCore.Mvc;
using PagamentosAPI.Domain.Models;
using Newtonsoft.Json;
using Pagamentos.Application.Services;
using PagamentosAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using PagamentosAPI.Application.Services.Autenticacao;
using Microsoft.Extensions.Logging;

namespace PagamentosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class PagamentosController : ControllerBase
    {
        private readonly IPagamentoService _pagamentoService;
        private readonly ILogger<PagamentosController> _logger;
        private readonly IJwtAuthService _jwtAuthService;        
        public PagamentosController(IPagamentoService pagamentoService, IJwtAuthService jwtAuthService, ILogger<PagamentosController> logger)
        {
            _pagamentoService = pagamentoService;
            _logger = logger;
            _jwtAuthService = jwtAuthService;            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pagamento>>> GetPagamentosAsync()
        {
            try
            {
                // Antes de prosseguir com a lógica do endpoint, faz a validação do token
                var authToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(authToken) || !_jwtAuthService.ValidateToken(authToken))
                {
                    return Unauthorized();
                }
                var pagamentos = await _pagamentoService.GetPagamentosAsync();
                return Ok(pagamentos);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao buscar os pagamentos. " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pagamento>> GetPagamentoByIdAsync(int id)
        {
            try
            {
                // Antes de prosseguir com a lógica do endpoint, faz a validação do token
                var authToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(authToken) || !_jwtAuthService.ValidateToken(authToken))
                {
                    return Unauthorized();
                }
                var pagamento = await _pagamentoService.GetPagamentoByIdAsync(id);
                if (pagamento == null)
                {
                    return NotFound();
                }

                return Ok(pagamento);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao buscar o pagamento. " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPagamentoAsync(Pagamento pagamento)
        {
            try
            {
                // Antes de prosseguir com a lógica do endpoint, faz a validação do token
                var authToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(authToken) || !_jwtAuthService.ValidateToken(authToken))
                {
                    return Unauthorized();
                }
                await _pagamentoService.AddPagamentoAsync(pagamento);
                return CreatedAtAction(nameof(GetPagamentoByIdAsync), new { id = pagamento.Id }, pagamento);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao cadastrar o pagamento. " + ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePagamentoAsync(int id, Pagamento pagamento)
        {
            try
            {
                // Antes de prosseguir com a lógica do endpoint, faz a validação do token
                var authToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(authToken) || !_jwtAuthService.ValidateToken(authToken))
                {
                    return Unauthorized();
                }
                await _pagamentoService.UpdatePagamentoAsync(id, pagamento); 
                return Ok(pagamento);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao atualizar o pagamento. " + ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePagamentoAsync(int id)
        {
            try
            {
                // Antes de prosseguir com a lógica do endpoint, faz a validação do token
                var authToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(authToken) || !_jwtAuthService.ValidateToken(authToken))
                {
                    return Unauthorized();
                }
                var pagamento = await _pagamentoService.GetPagamentoByIdAsync(id);
                if (pagamento == null)
                {
                    return NotFound();
                }

                await _pagamentoService.DeletePagamentoAsync(id);                        

                return NoContent();
            }
            catch (HttpResponseException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao deletar o pagamento. " + ex.Message);
            }
        }

        [HttpGet("{clienteId}")]
        public async Task<ActionResult<IEnumerable<Pagamento>>> GetPagamentosDoCliente(string cpfOuCnpj)
        {
            try
            {
                // Antes de prosseguir com a lógica do endpoint, faz a validação do token
                var authToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(authToken) || !_jwtAuthService.ValidateToken(authToken))
                {
                    return Unauthorized();
                }
                var pagamentos = await _pagamentoService.GetPagamentosDoCliente(cpfOuCnpj);
                return Ok(pagamentos);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Pagamentos não encontrados para o cliente. " + ex.Message);
            }          
        }
    }
}
