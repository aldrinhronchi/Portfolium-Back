namespace Portfolium_Back.Models.Entities
{
    /// <summary>
    /// Classe Pai contendo o básico para boa parte dos cadastros
    /// </summary>
    public class Entity
    {
        /// <summary>
        /// Identificador Único, de Um em Um
        /// </summary>
        public Int32 ID { get; set; }

        /// <summary>
        /// Identificador Único Global (GUID)
        /// </summary>
        public Guid GuidID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Registro Ativo ou Inativo
        /// </summary>
        public Boolean IsActive { get; set; } = true;

        /// <summary>
        /// Data de Criação, tendo valor padrão no banco de GetDate()
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Data da Última Alteração
        /// </summary>
        public DateTime? DateUpdated { get; set; }

        /// <summary>
        /// Identificador de quem gerou o registro sendo inserido no padrão: "IDUsuario - NomeUsuario", vindo formatado do Front-End
        /// </summary>
        public String? UserCreated { get; set; }

        /// <summary>
        /// Identificador de quem fez a ultima alteração do registro, sendo inserido no padrão: "IDUsuario - NomeUsuario", vindo formatado do Front-End
        /// </summary>
        public String? UserUpdated { get; set; }
    }
}