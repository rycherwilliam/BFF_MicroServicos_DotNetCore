using System.Threading.Tasks;
using BFFAPI.Application.Services;
using BFFAPI.Application.Services.PagamentoWEB;
using BFFAPI.Domain.Models;
using BFFAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BFFAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class PagamentosBFFController : ControllerBase
    {
        private readonly IPagamentoService _pagamentoService;
        private readonly IClientePagamentoService _clientePagamentoService;

        public PagamentosBFFController(IPagamentoService pagamentoService, IClientePagamentoService clientePagamentoService)
        {
            _pagamentoService = pagamentoService;
            _clientePagamentoService = clientePagamentoService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<Pagamento>>>> GetPagamentosAsync()
        {            
            var pagamentosResponse = await _clientePagamentoService.GetPagamentosAsync();
            if (!pagamentosResponse.Success)
            {
                return BadRequest(pagamentosResponse.ErrorMessage);
            }

            return Ok(pagamentosResponse.Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Pagamento>>> GetPagamentoByIdPagamentoAsync(int id)
        {            
            var pagamentoResponse = await _clientePagamentoService.GetPagamentoByIdPagamentoAsync(id);
            if (!pagamentoResponse.Success)
            {
                return NotFound(pagamentoResponse.ErrorMessage);
            }

            return Ok(pagamentoResponse.Data);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> AddPagamentoAsync(Pagamento pagamento)
        {
            var response = await _pagamentoService.AddPagamentoAsync(pagamento);
            if (!response.Success)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse>> UpdatePagamentoAsync(int id, Pagamento pagamento)
        {
            var response = await _pagamentoService.UpdatePagamentoAsync(id, pagamento);
            if (!response.Success)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse>> DeletePagamentoAsync(int id)
        {
            var response = await _pagamentoService.DeletePagamentoAsync(id);
            if (!response.Success)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok();
        }

        [HttpGet("cliente/{cpfOuCnpj}")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<Pagamento>>>> GetPagamentosDoCliente(string cpfOuCnpj)
        {
            var pagamentosDoClienteResponse = await _clientePagamentoService.GetPagamentosDoCliente(cpfOuCnpj);
            if (!pagamentosDoClienteResponse.Success)
            {
                return BadRequest(pagamentosDoClienteResponse.ErrorMessage);
            }

            return Ok(pagamentosDoClienteResponse.Data);
        }    
                
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<int>>> GetTotalPagamentosAsync()
        {
            var response = await _clientePagamentoService.GetTotalPagamentosAsync();
            if (!response.Success)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(response.Data);
        }
                
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<int>>> GetTotalPagamentosAtrasadosEAVencerAsync()
        {
            var response = await _clientePagamentoService.GetTotalPagamentosAtrasadosEAVencerAsync();
            if (!response.Success)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(response.Data);
        }
                
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<MediaRendaPorEstado>>>> GetMediaRendaBrutaPorEstadoAsync()
        {
            var response = await _clientePagamentoService.GetMediaRendaBrutaPorEstadoAsync();
            if (!response.Success)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(response.Data);
        }
                
        [HttpGet("{estado}/{statusPagamento}")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<Cliente>>>> GetClientesPorEstadoEStatusPagamentoAsync(string estado, EstadoPagamento statusPagamento)
        {
            var response = await _clientePagamentoService.GetClientesPorEstadoEStatusPagamentoAsync(estado, statusPagamento);
            if (!response.Success)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(response.Data);
        }
                
        [HttpGet("{estado}")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<Pagamento>>>> GetPagamentosPorEstadoAsync(string estado)
        {
            var response = await _clientePagamentoService.GetPagamentosPorEstadoAsync(estado);
            if (!response.Success)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(response.Data);
        }
    }
}
