using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using Portfolium_Back.Models.ViewModels;

namespace Portfolium_Back.Extensions.Helpers
{
    /// <summary>
    /// Helper para efetuar as ações de paginação e filtragem
    /// </summary>
    public class TipografiaHelper
    {
        /// <summary>
        /// Função para formatar as requisições para o padrão esperado pelo Front-End
        /// </summary>
        /// <typeparam name="T">Classe a ser enviada</typeparam>
        /// <param name="Dados">Dados em si da requisição para serem páginadas.</param>
        /// <param name="Pagina">A Página Atual.</param>
        /// <param name="TamanhoPagina">Quantidade de Registros exibidos por Página. Se for "0", deve se trazer todos os Dados.</param>
        /// <returns>A RequisicaoViewModel com os Dados esperados pelo Front-End</returns>
        public static RequisicaoViewModel<T> FormatarRequisicao<T>(IQueryable<T> Dados, Int32 Pagina, Int32 TamanhoPagina)
        {
            Decimal Total = Dados.Count();
            List<T> Items;
            Decimal TotalPaginas = 0;
            if (TamanhoPagina == 0)
            {
                Items = Dados.ToList();
            }
            else
            {
                Items = Dados.Skip((Pagina - 1) * TamanhoPagina).Take(TamanhoPagina).ToList();
                TotalPaginas = Math.Ceiling(Total / TamanhoPagina);
            }

            return new RequisicaoViewModel<T>()
            {
                Data = Items,
                Page = Pagina,
                PageSize = TamanhoPagina,
                PageCount = (Int32)TotalPaginas,
                Type = typeof(T).Name,
            };
        }

        /// <summary>
        /// Função para formatar as requisições de forma assíncrona para o padrão esperado pelo Front-End
        /// </summary>
        /// <typeparam name="T">Classe a ser enviada</typeparam>
        /// <param name="Dados">Dados em si da requisição para serem páginadas.</param>
        /// <param name="Pagina">A Página Atual.</param>
        /// <param name="TamanhoPagina">Quantidade de Registros exibidos por Página. Se for "0", deve se trazer todos os Dados.</param>
        /// <returns>A RequisicaoViewModel com os Dados esperados pelo Front-End</returns>
        public static async Task<RequisicaoViewModel<T>> FormatarRequisicaoAsync<T>(IQueryable<T> Dados, Int32 Pagina, Int32 TamanhoPagina)
        {
            Decimal Total = await Dados.CountAsync();
            List<T> Items;
            Decimal TotalPaginas = 0;
            if (TamanhoPagina == 0)
            {
                Items = await Dados.ToListAsync();
            }
            else
            {
                Items = await Dados.Skip((Pagina - 1) * TamanhoPagina).Take(TamanhoPagina).ToListAsync();
                TotalPaginas = Math.Ceiling(Total / TamanhoPagina);
            }

            return new RequisicaoViewModel<T>()
            {
                Data = Items,
                Page = Pagina,
                PageSize = TamanhoPagina,
                PageCount = (Int32)TotalPaginas,
                Type = typeof(T).Name,
            };
        }

        /// <summary>
        /// Função para filtrar uma coleção de dados com base em um campo e um valor específico.
        /// </summary>
        /// <typeparam name="T">Classe dos dados a serem filtrados.</typeparam>
        /// <param name="Dados">Coleção de dados a ser filtrada.</param>
        /// <param name="Campo">Nome do campo pelo qual será feita a filtragem.</param>
        /// <param name="Valor">Valor utilizado para a filtragem.</param>
        /// <returns>Um IQueryable contendo apenas os registros que correspondem ao filtro.</returns>
        public static IQueryable<T> Filtrar<T>(IQueryable<T> Dados, String Campo, String Valor)
        {
            PropertyInfo? CampoObjeto = typeof(T).GetProperty(Campo);
            if (CampoObjeto == null)
            {
                throw new ValidationException("Campo não encontrado");
            }

            ParameterExpression Linha = Expression.Parameter(typeof(T), "row");
            Expression Expressao = Expression.Property(Linha, CampoObjeto); // x => x.(TIPO)

            if (CampoObjeto.PropertyType == typeof(String))
            {
                Expressao = Expression.Coalesce(Expressao, Expression.Constant(String.Empty));
                MethodInfo? Maisculo = typeof(String).GetMethod("ToUpper", Type.EmptyTypes);
                if (Maisculo != null)
                {
                    Expressao = Expression.Call(Expressao, Maisculo);
                    Valor = Valor?.ToUpper() ?? String.Empty;
                    MethodInfo? Comparador = typeof(String).GetMethod("Contains", new[] { typeof(String) });
                    if (Comparador != null)
                    {
                        Expressao = Expression.Call(Expressao, Comparador, Expression.Constant(Valor));
                    }
                }
            }
            else if (CampoObjeto.PropertyType == typeof(Int32) || CampoObjeto.PropertyType == typeof(Int32?))
            {
                if (Int32.TryParse(Valor, out Int32 valorInt))
                {
                    ConstantExpression constante = Expression.Constant(valorInt, CampoObjeto.PropertyType);
                    Expressao = Expression.Equal(Expressao, constante);
                }
                else
                {
                    throw new ValidationException("Valor inválido para o tipo int");
                }
            }
            else if (CampoObjeto.PropertyType == typeof(Boolean))
            {
                if (Boolean.TryParse(Valor, out Boolean valorBool))
                {
                    Expressao = Expression.Equal(Expressao, Expression.Constant(valorBool));
                }
                else
                {
                    throw new ValidationException("Valor inválido para o tipo bool");
                }
            }
            else
            {
                MethodInfo? ConverterParaString = typeof(Object).GetMethod("ToString", Type.EmptyTypes);
                if (ConverterParaString != null)
                {
                    Expressao = Expression.Call(Expressao, ConverterParaString);
                    MethodInfo? Comparador = typeof(String).GetMethod("Contains", new[] { typeof(String) });
                    if (Comparador != null)
                    {
                        Expressao = Expression.Call(Expressao, Comparador, Expression.Constant(Valor));
                    }
                }
            }

            Func<T, Boolean> func = Expression.Lambda<Func<T, Boolean>>(Expressao, Linha).Compile();
            return Dados.Where(func).AsQueryable();
        }

        /// <summary>
        /// Função para ordenar uma coleção de dados com base em um campo específico.
        /// </summary>
        /// <typeparam name="T">Classe dos dados a serem ordenados.</typeparam>
        /// <param name="Dados">Coleção de dados a ser ordenada.</param>
        /// <param name="Campo">Nome do campo pelo qual será feita a ordenação.</param>
        /// <param name="Ordem">Define a direção da ordenação. Se "true", ordena de forma crescente; se "false", ordena de forma decrescente.</param>
        /// <returns>Um IOrderedQueryable contendo os registros ordenados conforme o campo especificado.</returns>
        public static IOrderedQueryable<T> Ordenar<T>(IQueryable<T> Dados, String Campo, Boolean Ordem = false)
        {
            Type Classe = typeof(T);
            ParameterExpression arg = Expression.Parameter(Classe, "x");
            Expression Expressao = arg;
            
            String[] props = Campo.Split('.');
            foreach (String prop in props)
            {
                PropertyInfo? pi = Classe.GetProperty(prop);
                if (pi != null)
                {
                    Expressao = Expression.Property(Expressao, pi);
                    Classe = pi.PropertyType;
                }
                else
                {
                    throw new ValidationException($"Propriedade '{prop}' não encontrada");
                }
            }
            
            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), Classe);
            LambdaExpression lambda = Expression.Lambda(delegateType, Expressao, arg);

            String methodName = Ordem ? "OrderBy" : "OrderByDescending";
            Object? result = typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 2
                        && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), Classe)
                .Invoke(null, new Object[] { Dados, lambda });

            return (IOrderedQueryable<T>)result!;
        }
    }
} 