using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text.Json;

namespace Portfolium_Back.Extensions
{
    /// <summary>
    /// Extensões diversas para o sistema Portfolium
    /// </summary>
    public static class Extensions
    {
        public static T To<T>(this IConvertible value)
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        public static DateTime ToDateTime(this string value)
        {
            var data = value.Split("-");
            return new DateTime(
                                int.Parse(data[0]),
                                int.Parse(data[1]),
                                int.Parse(data[2]));
        }

        public static decimal ToDecimal(this string value)
        {
            decimal number;

            Decimal.TryParse(value, out number);

            return number;
        }

        public static bool IsNull(this object source)
        {
            return source == null;
        }

        public static bool IsNotNullOrEmptyOriginal<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }

        public static bool IsNullOrEmptyOriginal<T>(this IEnumerable<T> source)
        {
            return !source.IsNotNullOrEmptyOriginal();
        }

        public static bool IsNullOrEmptyOrWhiteSpace(this string input)
        {
            return string.IsNullOrEmpty(input) || input.Trim() == string.Empty;
        }

        public static IEnumerable<T> AlphaLengthWise<T, L>(this IEnumerable<T> names, Func<T, L> lengthProvider)
        {
            return names.OrderBy(a => lengthProvider(a)).ThenBy(a => a);
        }

        public static int Compare<T>(this T value, T value2) where T : IComparable
        {
            return value.CompareTo(value2);
        }

        public static bool AnyOfType<T>(this IEnumerable source)
        {
            foreach (var obj in source)
            {
                if (obj is T)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool Like(this string value, string search)
        {
            return value.Contains(search) || value.StartsWith(search) || value.EndsWith(search);
        }

        /// <summary>
        /// Formats the string according to the specified mask
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="mask">The mask for formatting. Like "A##-##-T-###Z"</param>
        /// <returns>The formatted string</returns>
        public static string FormatWithMask(this string input, string mask)
        {
            if (input.IsNullOrEmpty()) return input;
            var output = string.Empty;
            var index = 0;
            foreach (var m in mask)
            {
                if (m == '#')
                {
                    if (index < input.Length)
                    {
                        output += input[index];
                        index++;
                    }
                }
                else
                    output += m;
            }
            return output;
        }

        public static string StripHtml(this string input)
        {
            // Will this simple expression replace all tags???
            var tagsExpression = new Regex(@"</?.+?>");
            return tagsExpression.Replace(input, " ");
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        public static IEnumerable<t> Randomize<t>(this IEnumerable<t> target)
        {
            Random r = new Random();

            return target.OrderBy(x => (r.Next()));
        }

        public static bool IsValidEmailAddress(this string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }

        public static bool IsWeekday(this DayOfWeek dow)
        {
            switch (dow)
            {
                case DayOfWeek.Sunday:
                case DayOfWeek.Saturday:
                    return false;

                default:
                    return true;
            }
        }

        public static bool IsWeekend(this DayOfWeek dow)
        {
            return !dow.IsWeekday();
        }

        public static DateTime AddWorkdays(this DateTime startDate, int days)
        {
            // start from a weekday
            while (startDate.DayOfWeek.IsWeekend())
                startDate = startDate.AddDays(1.0);

            for (int i = 0; i < days; ++i)
            {
                startDate = startDate.AddDays(1.0);

                while (startDate.DayOfWeek.IsWeekend())
                    startDate = startDate.AddDays(1.0);
            }
            return startDate;
        }

        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        public static bool IsPrime(this int number)
        {
            if ((number % 2) == 0)
            {
                return number == 2;
            }
            int sqrt = (int)Math.Sqrt(number);
            for (int t = 3; t <= sqrt; t = t + 2)
            {
                if (number % t == 0)
                {
                    return false;
                }
            }
            return number != 1;
        }

        public static string SerializeToXml(this object obj)
        {
            XDocument doc = new XDocument();
            using (XmlWriter xmlWriter = doc.CreateWriter())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
                xmlSerializer.Serialize(xmlWriter, obj);
                xmlWriter.Close();
            }
            return doc.ToString();
        }

        public static string ToCsv<T>(this IEnumerable<T> instance, bool includeColumnHeader, string[] properties)
        {
            if (instance == null)
                return null;

            var csv = new StringBuilder();

            if (includeColumnHeader)
            {
                var header = new StringBuilder();
                foreach (var property in properties)
                    header.AppendFormat("{0},", property);

                csv.AppendLine(header.ToString(0, header.Length - 1));
            }

            foreach (var item in instance)
            {
                var row = new StringBuilder();

                foreach (var property in properties)
                    //        row.AppendFormat("{0},", item.GetPropertyValue<object>(property));

                    csv.AppendLine(row.ToString(0, row.Length - 1));
            }

            return csv.ToString();
        }

        public static string ToCsv<T>(this IEnumerable<T> instance, bool includeColumnHeader)
        {
            if (instance == null)
                return null;

            var properties = (from p in typeof(T).GetProperties()
                              select p.Name).ToArray();

            return ToCsv(instance, includeColumnHeader, properties);
        }

        public static IEnumerable<T> RemoveDuplicates<T>(this ICollection<T> list, Func<T, int> Predicate)
        {
            var dict = new Dictionary<int, T>();

            foreach (var item in list)
            {
                if (!dict.ContainsKey(Predicate(item)))
                {
                    dict.Add(Predicate(item), item);
                }
            }

            return dict.Values.AsEnumerable();
        }

        public static int Increment(this int i)
        {
            return ++i;
        }

        //public static string ToJson(this object obj)
        //{
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    return serializer.Serialize(obj);
        //}

        //public static string ToJson(this object obj, int recursionDepth)
        //{
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    serializer.RecursionLimit = recursionDepth;
        //    return serializer.Serialize(obj);
        //}

        //public static T FromJson<T>(this object obj)
        //{
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    return serializer.Deserialize<T>(obj as string);
        //}

        #region String Extensions

        /// <summary>
        /// Remove acentos de uma string
        /// </summary>
        public static String RemoverAcentos(this String texto)
        {
            if (String.IsNullOrEmpty(texto))
                return texto;

            StringBuilder sb = new StringBuilder();
            var normalizedString = texto.Normalize(NormalizationForm.FormD);
            
            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Formata string para URL amigável (slug)
        /// </summary>
        public static String ToSlug(this String texto)
        {
            if (String.IsNullOrEmpty(texto))
                return String.Empty;

            String slug = texto.RemoverAcentos()
                .ToLowerInvariant()
                .Trim();

            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = Regex.Replace(slug, @"\s+", " ");
            slug = Regex.Replace(slug, @"\s", "-");

            return slug;
        }

        /// <summary>
        /// Verifica se a string é um email válido
        /// </summary>
        public static Boolean IsValidEmail(this String email)
        {
            if (String.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var emailAttribute = new EmailAddressAttribute();
                return emailAttribute.IsValid(email);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Verifica se a string é um CPF válido
        /// </summary>
        public static Boolean IsValidCPF(this String cpf)
        {
            if (String.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = cpf.Replace(".", "").Replace("-", "").Trim();

            if (cpf.Length != 11)
                return false;

            if (cpf.All(c => c == cpf[0]))
                return false;

            Int32[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            Int32[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            String tempCpf = cpf.Substring(0, 9);
            Int32 soma = 0;

            for (Int32 i = 0; i < 9; i++)
                soma += Int32.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            Int32 resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            String digito = resto.ToString();
            tempCpf += digito;
            soma = 0;

            for (Int32 i = 0; i < 10; i++)
                soma += Int32.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }

        /// <summary>
        /// Verifica se a string é um CNPJ válido
        /// </summary>
        public static Boolean IsValidCNPJ(this String cnpj)
        {
            if (String.IsNullOrWhiteSpace(cnpj))
                return false;

            cnpj = cnpj.Replace(".", "").Replace("/", "").Replace("-", "").Trim();

            if (cnpj.Length != 14)
                return false;

            if (cnpj.All(c => c == cnpj[0]))
                return false;

            Int32[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            Int32[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            String tempCnpj = cnpj.Substring(0, 12);
            Int32 soma = 0;

            for (Int32 i = 0; i < 12; i++)
                soma += Int32.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            Int32 resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            String digito = resto.ToString();
            tempCnpj += digito;
            soma = 0;

            for (Int32 i = 0; i < 13; i++)
                soma += Int32.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();

            return cnpj.EndsWith(digito);
        }

        /// <summary>
        /// Mascara string para exibição (ex: email, cpf)
        /// </summary>
        public static String MaskString(this String input, Int32 startVisible = 2, Int32 endVisible = 2, Char maskChar = '*')
        {
            if (String.IsNullOrEmpty(input) || input.Length <= startVisible + endVisible)
                return input;

            String start = input.Substring(0, startVisible);
            String end = input.Substring(input.Length - endVisible);
            String middle = new String(maskChar, input.Length - startVisible - endVisible);

            return start + middle + end;
        }

        /// <summary>
        /// Capitaliza primeira letra de cada palavra
        /// </summary>
        public static String ToTitleCase(this String input)
        {
            if (String.IsNullOrEmpty(input))
                return input;

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(input.ToLower());
        }

        /// <summary>
        /// Converte string para formato de moeda brasileira
        /// </summary>
        public static String ToMoedaBrasil(this Decimal valor)
        {
            return valor.ToString("C", new CultureInfo("pt-BR"));
        }

        /// <summary>
        /// Converte string para formato de moeda brasileira
        /// </summary>
        public static String ToMoedaBrasil(this Double valor)
        {
            return valor.ToString("C", new CultureInfo("pt-BR"));
        }

        #endregion

        #region DateTime Extensions

        /// <summary>
        /// Converte DateTime para timestamp Unix
        /// </summary>
        public static Int64 ToUnixTimestamp(this DateTime dateTime)
        {
            return ((DateTimeOffset)dateTime).ToUnixTimeSeconds();
        }

        /// <summary>
        /// Converte timestamp Unix para DateTime
        /// </summary>
        public static DateTime FromUnixTimestamp(this Int64 timestamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
        }

        /// <summary>
        /// Verifica se a data está no período especificado
        /// </summary>
        public static Boolean IsInRange(this DateTime dateTime, DateTime startDate, DateTime endDate)
        {
            return dateTime >= startDate && dateTime <= endDate;
        }

        /// <summary>
        /// Retorna o primeiro dia do mês
        /// </summary>
        public static DateTime FirstDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        /// <summary>
        /// Retorna o último dia do mês
        /// </summary>
        public static DateTime LastDayOfMonth(this DateTime dateTime)
        {
            return dateTime.FirstDayOfMonth().AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Verifica se é fim de semana
        /// </summary>
        public static Boolean IsWeekend(this DateTime dateTime)
        {
            return dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Retorna data em português brasileiro
        /// </summary>
        public static String ToPortugueseBrazil(this DateTime dateTime, String format = "dd/MM/yyyy")
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            return dateTime.ToString(format, culture);
        }

        #endregion

        #region Collection Extensions

        /// <summary>
        /// Verifica se a coleção não é nula e não está vazia
        /// </summary>
        public static Boolean IsNotNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection != null && collection.Any();
        }

        /// <summary>
        /// Verifica se a coleção é nula ou está vazia
        /// </summary>
        public static Boolean IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        /// <summary>
        /// Executa ação para cada item da coleção
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if (collection != null && action != null)
            {
                foreach (T item in collection)
                {
                    action(item);
                }
            }
        }

        /// <summary>
        /// Divide coleção em lotes do tamanho especificado
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, Int32 size)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            
            if (size <= 0)
                throw new ArgumentException("Size must be greater than zero.", nameof(size));

            return BatchImpl(source, size);
        }

        private static IEnumerable<IEnumerable<T>> BatchImpl<T>(IEnumerable<T> source, Int32 size)
        {
            T[]? bucket = null;
            Int32 count = 0;

            foreach (T item in source)
            {
                if (bucket == null)
                    bucket = new T[size];

                bucket[count++] = item;

                if (count != size)
                    continue;

                yield return bucket;

                bucket = null;
                count = 0;
            }

            if (bucket != null && count > 0)
                yield return bucket.Take(count);
        }

        #endregion

        #region Object Extensions

        /// <summary>
        /// Converte objeto para JSON
        /// </summary>
        public static String ToJson<T>(this T obj, JsonSerializerOptions? options = null)
        {
            if (obj == null)
                return String.Empty;

            options ??= new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            return JsonSerializer.Serialize(obj, options);
        }

        /// <summary>
        /// Clona objeto via JSON serialization
        /// </summary>
        public static T? DeepClone<T>(this T obj) where T : class
        {
            if (obj == null)
                return null;

            String json = obj.ToJson();
            return JsonSerializer.Deserialize<T>(json);
        }

        /// <summary>
        /// Verifica se objeto possui propriedade
        /// </summary>
        public static Boolean HasProperty(this Object obj, String propertyName)
        {
            return obj?.GetType().GetProperty(propertyName) != null;
        }

        /// <summary>
        /// Obtém valor da propriedade via reflexão
        /// </summary>
        public static Object? GetPropertyValue(this Object obj, String propertyName)
        {
            return obj?.GetType()
                .GetProperty(propertyName)?
                .GetValue(obj);
        }

        /// <summary>
        /// Define valor da propriedade via reflexão
        /// </summary>
        public static void SetPropertyValue(this Object obj, String propertyName, Object? value)
        {
            PropertyInfo? property = obj?.GetType().GetProperty(propertyName);
            if (property != null && property.CanWrite)
            {
                property.SetValue(obj, value);
            }
        }

        #endregion

        #region Security Extensions

        /// <summary>
        /// Gera hash SHA256
        /// </summary>
        public static String ToSHA256(this String input)
        {
            if (String.IsNullOrEmpty(input))
                return String.Empty;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                Byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(bytes);
            }
        }

        /// <summary>
        /// Gera hash MD5
        /// </summary>
        public static String ToMD5(this String input)
        {
            if (String.IsNullOrEmpty(input))
                return String.Empty;

            using (var md5 = MD5.Create())
            {
                Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                Byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (Int32 i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return sb.ToString().ToLower();
            }
        }

        /// <summary>
        /// Gera string aleatória
        /// </summary>
        public static String GenerateRandomString(Int32 length = 10, Boolean includeNumbers = true, Boolean includeSpecialChars = false)
        {
            String chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            
            if (includeNumbers)
                chars += "0123456789";
            
            if (includeSpecialChars)
                chars += "!@#$%^&*()_+-=[]{}|;:,.<>?";

            Random random = new Random();
            return new String(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #endregion

        #region Entity Framework Extensions

        /// <summary>
        /// Aplica filtros dinâmicos a uma query
        /// </summary>
        public static IQueryable<T> ApplyFilters<T>(this IQueryable<T> query, Dictionary<String, Object> filters)
        {
            if (filters == null || !filters.Any())
                return query;

            foreach (var filter in filters)
            {
                if (filter.Value != null)
                {
                    var parameter = Expression.Parameter(typeof(T), "x");
                    var property = Expression.Property(parameter, filter.Key);
                    var constant = Expression.Constant(filter.Value);
                    var equals = Expression.Equal(property, constant);
                    var lambda = Expression.Lambda<Func<T, Boolean>>(equals, parameter);
                    
                    query = query.Where(lambda);
                }
            }

            return query;
        }

        /// <summary>
        /// Aplica ordenação dinâmica a uma query
        /// </summary>
        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, String orderBy, Boolean ascending = true)
        {
            if (String.IsNullOrEmpty(orderBy))
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, orderBy);
            var lambda = Expression.Lambda(property, parameter);

            String methodName = ascending ? "OrderBy" : "OrderByDescending";

            var result = typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                    && method.IsGenericMethodDefinition
                    && method.GetGenericArguments().Length == 2
                    && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), property.Type)
                .Invoke(null, new Object[] { query, lambda });

            return (IQueryable<T>)result!;
        }

        #endregion

        #region Http Extensions

        /// <summary>
        /// Obtém o IP real do cliente considerando proxies
        /// </summary>
        public static String GetClientIpAddress(this Microsoft.AspNetCore.Http.HttpContext context)
        {
            String? ipAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            
            if (String.IsNullOrEmpty(ipAddress))
                ipAddress = context.Request.Headers["X-Real-IP"].FirstOrDefault();
            
            if (String.IsNullOrEmpty(ipAddress))
                ipAddress = context.Connection.RemoteIpAddress?.ToString();

            return ipAddress ?? "Unknown";
        }

        /// <summary>
        /// Verifica se a requisição é AJAX
        /// </summary>
        public static Boolean IsAjaxRequest(this Microsoft.AspNetCore.Http.HttpContext context)
        {
            return context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        /// <summary>
        /// Obtém User Agent da requisição
        /// </summary>
        public static String GetUserAgent(this Microsoft.AspNetCore.Http.HttpContext context)
        {
            return context.Request.Headers["User-Agent"].ToString();
        }

        #endregion
    }
}