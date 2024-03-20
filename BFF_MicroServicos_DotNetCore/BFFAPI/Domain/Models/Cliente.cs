using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BFFAPI.Domain.Models
{
    public class Cliente
    {        
        [Required]
        [StringLength(14)]
        public string CpfOuCnpj { get; set; }
        public string Nome { get; set; }
        public int NumeroDoContrato { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public decimal RendaBruta { get; set; }        
    }
}
