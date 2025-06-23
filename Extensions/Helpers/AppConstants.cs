namespace Portfolium_Back.Extensions.Helpers
{
    /// <summary>
    /// Constantes da aplicação 
    /// </summary>
    public static class AppConstants
    {
        /// <summary>
        /// ID do único usuário do sistema (single-user application)
        /// Este ID é usado automaticamente em todas as operações que requerem UserID
        /// </summary>
        public static readonly Guid SINGLE_USER_ID = Guid.Parse("94508897-5ed5-40d4-bf91-cdde65082fbf");

        /// <summary>
        /// Nome do sistema
        /// </summary>
        public const string SYSTEM_NAME = "Portfolium";
        
        /// <summary>
        /// Versão da API
        /// </summary>
        public const string API_VERSION = "1.0";
    }
} 