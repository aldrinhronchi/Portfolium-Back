namespace Portfolium_Back.Models.ViewModels
{
    /// <summary>
    /// ViewModel para requisições paginadas
    /// </summary>
    /// <typeparam name="T">Tipo de dados retornados</typeparam>
    public class RequisicaoViewModel<T>
    {
        /// <summary>
        /// Dados da página atual
        /// </summary>
        public List<T>? Data { get; set; }

        /// <summary>
        /// Página atual
        /// </summary>
        public Int32 Page { get; set; }

        /// <summary>
        /// Quantidade de registros por página
        /// </summary>
        public Int32 PageSize { get; set; }

        /// <summary>
        /// Total de páginas
        /// </summary>
        public Int32 PageCount { get; set; }

        /// <summary>
        /// Tipo da classe dos dados
        /// </summary>
        public String? Type { get; set; }
    }
} 