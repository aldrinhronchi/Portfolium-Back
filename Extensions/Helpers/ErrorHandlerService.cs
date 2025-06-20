using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using Portfolium_Back.Context;
using Portfolium_Back.Models.Entities;
using Portfolium_Back.Extensions.Helpers;

namespace Portfolium_Back.Extensions.Helpers
{
    public class ErrorHandlerService
    {
        public ILogger _logger;

        public ErrorHandlerService(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<ErrorInfo> FormatExceptionAsync(Exception exception)
        {
            ErrorInfo errorInfo = new ErrorInfo();

            switch (exception)
            {
                case KeyNotFoundException ex:
                    {
                        errorInfo.StatusCode = (Int32)HttpStatusCode.NotFound;
                        errorInfo.Message = ex.Message;
                    }
                    break;

                case ValidationException ex:
                    {
                        errorInfo.StatusCode = (Int32)HttpStatusCode.BadRequest;
                        errorInfo.Message = ex.Message;
                    }
                    break;

                case UnauthorizedAccessException ex:
                    {
                        errorInfo.StatusCode = (Int32)HttpStatusCode.Unauthorized;
                        errorInfo.Message = ex.Message;
                    }
                    break;

                default:
                    {
                        errorInfo.StatusCode = (Int32)HttpStatusCode.InternalServerError;
                        try
                        {
                            String[] Aplicacao = SearchMethodNameAndClass(exception);
                            DateTime DataAtual = TimeZoneManager.GetTimeNow();
                            Erro Falha = new Erro()
                            {
                                Aplicacao = Aplicacao.Count() == 2 ? $"{Aplicacao[0]} | {Aplicacao[1]}" : "Portfolium | Geral",
                                Data = DataAtual,
                                Tipo = exception.GetType().ToString(),
                                Nome = exception.TargetSite != null ? exception.TargetSite.Name : String.Empty,
                                Mensagem = exception.Message,
                                Stack = !String.IsNullOrWhiteSpace(exception.StackTrace) ? exception.StackTrace : String.Empty,
                                Arquivo = exception.Source?.ToString(),
                            };
                            using (var scope = Portfolium_Back.Extensions.Middleware.ServiceLocator.Current.BuscarServico<IServiceScopeFactory>()?.CreateScope())
                            using (PortfoliumContext? database = scope?.ServiceProvider.GetService<PortfoliumContext>())
                            {
                                if (database != null)
                                {
                                    Erro? Error = database.Erros.FirstOrDefault(x => x.Aplicacao == Falha.Aplicacao
                                                                           && x.Mensagem == Falha.Mensagem
                                                                           && x.Nome == Falha.Nome &&
                                                                           x.Data.Day == DataAtual.Day && x.Data.Month == DataAtual.Month && x.Data.Year == DataAtual.Year);
                                if (Error != null)
                                {
                                    await database.Ocorrencia.AddAsync(new Ocorrencia()
                                    {
                                        Aplicacao = Falha.Aplicacao,
                                        Data = Falha.Data,
                                        IDErro = Error.ID
                                    });
                                    Falha = Error;
                                }
                                else
                                {
                                    await database.Erros.AddAsync(Falha);
                                }
                                    await database.SaveChangesAsync();
                                }
                            }

                            errorInfo.Message = $"Houve um erro, Protocolo: {Falha.ID},\n Mensagem: {exception.Message}";
                        }
                        catch (Exception ex)
                        {
                            DebugInFile(exception, "ErroNaoLogado", exception.GetFullExceptionMessage());
                            DebugInFile(ex, "ErrorForLog", ex.GetFullExceptionMessage());
                            errorInfo.Message = $"Houve um erro, Protocolo: 0,\n Mensagem: {exception.Message} \n Favor Contactar o suporte.";
                        }
                    }
                    break;
            }
            return errorInfo;
        }

        private static String[] SearchMethodNameAndClass(Exception ex)
        {
            if (!String.IsNullOrWhiteSpace(ex.StackTrace))
            {
                StackFrame? Frame = new StackTrace(ex).GetFrame(0);
                if (Frame != null)
                {
                    System.Reflection.MethodBase? Method = Frame.GetMethod();
                    if (Method != null)
                    {
                        String[] Array = new String[]
                        {
                            Method.GetMethodName(),
                            Method.GetClassName()
                        };
                        return Array;
                    }
                }
            }
            return new String[] { };
        }

        private void DebugInFile(Exception ex, String place, String responseMessage)
        {
            _logger.LogError("*****************************************************");
            _logger.LogError("Erro no método: {Metodo}", MethodBase.GetCurrentMethod()?.Name);
            _logger.LogError("Aplicação: {Aplicacao}", place);
            _logger.LogError("Data/Hora: {Timestamp}", DateTime.UtcNow);
            _logger.LogError("-----------------------------------------------------");
            _logger.LogError("Mensagem do Erro: {Mensagem}", ex.Message);
            _logger.LogError("Tipo do Erro: {Tipo}", ex.GetType().Name);
            _logger.LogError("Stack Trace: {StackTrace}", ex.StackTrace);
            _logger.LogError("Inner Exception: {InnerException}", ex.InnerException?.Message);
            _logger.LogError("Response: {ResponseMessage}", responseMessage);
            _logger.LogError("Máquina/Host: {Host}", Environment.MachineName);
            _logger.LogError("=====================================================");
        }
    }

    public static class ExceptionExtensions
    {
        public static String GetFullExceptionMessage(this Exception ex)
        {
            return ex.InnerException == null 
                ? ex.Message 
                : $"{ex.Message} --> {ex.InnerException.GetFullExceptionMessage()}";
        }

        public static String GetMethodName(this MethodBase method)
        {
            return method.Name;
        }

        public static String GetClassName(this MethodBase method)
        {
            return method.DeclaringType?.Name ?? "Unknown";
        }
    }
} 