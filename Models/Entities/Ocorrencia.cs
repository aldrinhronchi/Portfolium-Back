using System.ComponentModel.DataAnnotations.Schema;

namespace Portfolium_Back.Models.Entities
{
    [Table("OcorrenciasLog")]
    public class Ocorrencia
    {
        public Int32 ID { get; set; }
        public Int32 IDErro { get; set; }
        public String? Aplicacao { get; set; }
        public DateTime Data { get; set; }

        public virtual Erro? Erro { get; set; }
    }
} 