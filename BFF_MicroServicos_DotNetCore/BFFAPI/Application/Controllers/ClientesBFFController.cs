using BFFAPI.Application.Services;
using BFFAPI.Application.Services.ClienteWEB;
using BFFAPI.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BFFAPI.Application.Controllers
{
    [ApiController]    
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class ClientesBFFController : ControllerBase
    {
        private readonly IClienteBFFService _clienteService;
        private readonly IClientePagamentoService _clientePagamentoService;
        public ClientesBFFController(IClienteBFFService clienteService, IClientePagamentoService clientePagamentoService)
        {
            _clienteService = clienteService;
            _clientePagamentoService = clientePagamentoService;
        }

        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {            
            var serviceResponse = await _clientePagamentoService.GetClientesAsync();
            if (serviceResponse.Success)
            {
                return Ok(serviceResponse.Data);
            }
            else
            {
                return BadRequest(serviceResponse.ErrorMessage);
            }
        }

        [HttpGet("{cpfOuCnpj}")]
        public async Task<ActionResult<Cliente>> GetClienteByCpfOuCnpj(string cpfOuCnpj)
        {            
            var serviceResponse = await _clientePagamentoService.GetClienteByCpfOuCnpjAsync(cpfOuCnpj);
            if (serviceResponse.Success)
            {
                return Ok(serviceResponse.Data);
            }
            else
            {
                return BadRequest(serviceResponse.ErrorMessage);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCliente(Cliente cliente)
        {
            var serviceResponse = await _clienteService.AddClienteAsync(cliente);
            if (serviceResponse.Success)
            {
                return CreatedAtAction(nameof(GetClienteByCpfOuCnpj), new { cpfOuCnpj = cliente.CpfOuCnpj }, cliente);
            }
            else
            {
                return BadRequest(serviceResponse.ErrorMessage);
            }
        }

        [HttpPut("{cpfOuCnpj}")]
        public async Task<IActionResult> UpdateCliente(string cpfOuCnpj, Cliente cliente)
        {
            var serviceResponse = await _clienteService.UpdateClienteAsync(cpfOuCnpj, cliente);
            if (serviceResponse.Success)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(serviceResponse.ErrorMessage);
            }
        }

        [HttpDelete("{cpfOuCnpj}")]
        public async Task<IActionResult> DeleteCliente(string cpfOuCnpj)
        {
            var serviceResponse = await _clienteService.DeleteClienteAsync(cpfOuCnpj);
            if (serviceResponse.Success)
            {
                return NoContent();
            }
            else
            {
                return BadRequest(serviceResponse.ErrorMessage);
            }
        }

        [HttpGet("{estado}/{statusPagamento}")]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientesPorEstadoEStatusPagamentoAsync(string estado, EstadoPagamento statusPagamento)
        {
            var clientesResponse = await _clientePagamentoService.GetClientesPorEstadoEStatusPagamentoAsync(estado, statusPagamento);
            if (!clientesResponse.Success)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, clientesResponse.ErrorMessage);
            }

            return Ok(clientesResponse.Data);
        }
    }
}
