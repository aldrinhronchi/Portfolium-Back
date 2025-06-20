using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolium_Back.Models.Entities
{
    [Table("ErrosLog")]
    public class Erro
    {
        public Int32 ID { get; set; }

        public String? Aplicacao { get; set; }
        public String? Nome { get; set; }

        public DateTime Data { get; set; }

        public String? Tipo { get; set; }

        public String? Mensagem { get; set; }

        public String? Arquivo { get; set; }

        public String? Stack { get; set; }
    }
} 