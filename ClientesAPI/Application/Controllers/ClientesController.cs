using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using ClientesAPI.Application.Services;
using ClientesAPI.Application.Services.Autenticacao;
using ClientesAPI.Domain.Models;
using ClientesAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClientesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly ILogger<ClientesController> _logger;
        private readonly IJwtAuthService _jwtAuthService;
        public ClientesController(IClienteService clienteService, ILogger<ClientesController> logger, IJwtAuthService jwtAuthService)
        {
            _clienteService = clienteService;
            _logger = logger;
            _jwtAuthService = jwtAuthService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            try
            {
                // Antes de prosseguir com a lógica do endpoint, faz a validação do token
                var authToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(authToken) || !_jwtAuthService.ValidateToken(authToken))
                {
                    return Unauthorized();
                }

                var clientes = await _clienteService.GetClientesAsync();
                return Ok(clientes);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao buscar os clientes. " + ex.Message);
            }
        }

        [HttpGet("{cpfOuCnpj}")]
        public async Task<ActionResult<Cliente>> GetClienteByCpfOuCnpj(string cpfOuCnpj)
        {

            try
            {
                // Antes de prosseguir com a lógica do endpoint, faz a validação do token
                var authToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(authToken) || !_jwtAuthService.ValidateToken(authToken))
                {
                    return Unauthorized();
                }

                var cliente = await _clienteService.GetClienteByCpfOuCnpjAsync(cpfOuCnpj);
                return Ok(cliente);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao buscar o cliente. " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCliente(Cliente cliente)
        {
            try
            {
                // Antes de prosseguir com a lógica do endpoint, faz a validação do token
                var authToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(authToken) || !_jwtAuthService.ValidateToken(authToken))
                {
                    return Unauthorized();
                }

                await _clienteService.AddClienteAsync(cliente);
                return CreatedAtAction(nameof(GetClienteByCpfOuCnpj), new { cpfOuCnpj = cliente.CpfOuCnpj }, cliente);
            }
            catch (HttpResponseException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao cadastrar o cliente. " + ex.Message);
            }
        }

        [HttpPut("{cpfOuCnpj}")]
        public async Task<IActionResult> UpdateCliente(string cpfOuCnpj, Cliente cliente)
        {
            try
            {
                // Antes de prosseguir com a lógica do endpoint, faz a validação do token
                var authToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(authToken) || !_jwtAuthService.ValidateToken(authToken))
                {
                    return Unauthorized();
                }
                await _clienteService.UpdateClienteAsync(cpfOuCnpj, cliente);
                return Ok();
            }
            catch (HttpResponseException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao atualizar o cliente. " + ex.Message);
            }
        }

        [HttpDelete("{cpfOuCnpj}")]
        public async Task<IActionResult> DeleteCliente(string cpfOuCnpj)
        {
            try
            {
                // Antes de prosseguir com a lógica do endpoint, faz a validação do token
                var authToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(authToken) || !_jwtAuthService.ValidateToken(authToken))
                {
                    return Unauthorized();
                }

                await _clienteService.DeleteClienteAsync(cpfOuCnpj);
                return Ok();
            }
            catch (HttpResponseException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest("Ocorreu um erro ao deletar o cliente. " + ex.Message);
            }
        }
    }
}
