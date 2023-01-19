namespace MyStore.Common.Infrastructure
{
    using MyStore.Common.Application.Models;
    using RExtension;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    using System.Reflection;

    public static class PageFilterHelper
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> source, IList<PageFilter> filters)
        {
            var whereClause = string.Empty;
            var parameters = new List<object>();
            for (var i = 0; i < filters.Count(); i++)
            {
                var f = filters[i];
                if (i == 0)
                {
                    whereClause += BuildWhereClause<T>(f, parameters) + " ";
                }
                if (i != 0)
                {
                    whereClause += ToLinqOperator("and") + BuildWhereClause<T>(f, parameters) + " ";
                }
                if (i == filters.Count - 1)
                {
                    whereClause = CleanUp(whereClause);
                    source = source.Where(whereClause, parameters.ToArray());
                }
            }
            return source;
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string colName, string order)
        {
            var list = new List<OrderModel> { new OrderModel(colName, order) };
            return source.OrderBy(list);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, OrderModel orderModal)
        {
            var list = new List<OrderModel> { orderModal };
            return source.OrderBy(list);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, IList<OrderModel> orderModals)
        {
            var list = new List<string>();
            foreach (var orderModal in orderModals)
            {
                var (_, path) = GetPropertyInfo(typeof(T), orderModal.Column);
                list.Add($"@{path.FullTrim('.')} {orderModal.Order}");
            }
            return source.OrderBy(list.Join(", "));
        }

        private static string BuildWhereClause<T>(PageFilter filter, List<object> parameters)
        {
            if (filter.Operator.IsEmpty() || filter.Value.IsEmpty() || filter.Field.IsEmpty())
            {
                return string.Empty;
            }
            var entityType = typeof(T);
            var (propertyInfo, path) = GetPropertyInfo(entityType, filter.Field);
            path = $"@{path.FullTrim('.')}";
            var parameterIndex = parameters.Count;

            switch (filter.Operator.ToLower())
            {
                case "eq":
                case "neq":
                case "gte":
                case "gt":
                case "lte":
                case "lt":
                {
                    if (typeof(DateTime).IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        parameters.Add(DateTime.Parse(filter.Value).Date);
                        return string.Format($"{path}.Date" + ToLinqOperator(filter.Operator) + "@" + parameterIndex);
                    }
                    if (typeof(int).IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        parameters.Add(int.Parse(filter.Value));
                        return string.Format(path + ToLinqOperator(filter.Operator) + "@" + parameterIndex);
                    }
                    if (typeof(long).IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        parameters.Add(long.Parse(filter.Value));
                        return string.Format(path + ToLinqOperator(filter.Operator) + "@" + parameterIndex);
                    }
                    if (typeof(long?).IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        parameters.Add(long.Parse(filter.Value));
                        return string.Format(path + ToLinqOperator(filter.Operator) + "@" + parameterIndex);
                    }
                    if (typeof(bool).IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        parameters.Add(bool.Parse(filter.Value));
                        return string.Format(path + ToLinqOperator(filter.Operator) + "@" + parameterIndex);
                    }
                    if (typeof(Enum).IsAssignableFrom(propertyInfo.PropertyType))
                    {
                        parameters.Add(CastEnum(int.Parse(filter.Value), propertyInfo.PropertyType));
                        return string.Format(path + ToLinqOperator(filter.Operator) + "@" + parameterIndex);
                    }
                    parameters.Add(filter.Value);
                    return string.Format(path + ToLinqOperator(filter.Operator) + "@" + parameterIndex);
                }
                case "startswith":
                {
                    parameters.Add(filter.Value);
                    return path + ".StartsWith(" + "@" + parameterIndex + ")";
                }
                case "endswith":
                {
                    parameters.Add(filter.Value);
                    return path + ".EndsWith(" + "@" + parameterIndex + ")";
                }
                case "contains":
                {
                    parameters.Add(filter.Value);
                    return path + ".Contains(" + "@" + parameterIndex + ")";
                }
                case "doesnotcontain":
                {
                    parameters.Add(filter.Value);
                    return "!" + path + ".Contains(" + "@" + parameterIndex + ")";
                }
                default:
                {
                    throw new ArgumentException("This operator is not yet supported for this Grid", filter.Operator);
                }
            }
        }

        private static object CastEnum(int value, Type destType)
        {
            var enumType = GetEnumType(destType);
            if (enumType != null)
            {
                return Enum.ToObject(enumType, value);
            }
            throw new Exception("not found Enum type ");
        }

        private static string CleanUp(string whereClause)
        {
            switch (whereClause.Trim().Substring(0, 2).ToLower())
            {
                case "&&":
                {
                    return whereClause.Trim().Remove(0, 2);
                }
                case "||":
                {
                    return whereClause.Trim().Remove(0, 2);
                }
                default:
                {
                    return whereClause;
                }
            }
        }

        private static Type? GetEnumType(Type type)
        {
            if (type.IsEnum)
            {
                return type;
            }

            if (type.IsGenericType)
            {
                var genericDef = type.GetGenericTypeDefinition();
                if (genericDef == typeof(Nullable<>))
                {
                    var genericArgs = type.GetGenericArguments();
                    return genericArgs[0].IsEnum ? genericArgs[0] : null;
                }
            }
            return null;
        }

        private static (PropertyInfo propertyInfo, string path) GetPropertyInfo(Type type, string propName, string parentPath = "")
        {
            if (propName.Contains("."))
            {
                var temp = propName.Split(new char[] { '.' }, 2);
                var propertyInfo = type.GetProperty(temp[0], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                parentPath += (parentPath.IsEmpty() ? string.Empty : ".") + propertyInfo.Name;
                return GetPropertyInfo(propertyInfo.PropertyType, temp[1], parentPath);
            }
            else
            {
                var propertyInfo = type.GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                parentPath += "." + propertyInfo.Name;
                return (propertyInfo, parentPath);
            }
        }

        private static string ToLinqOperator(string @operator)
        {
            return @operator.ToLower() switch
            {
                "eq" => " == ",
                "neq" => " != ",
                "gte" => " >= ",
                "gt" => " > ",
                "lte" => " <= ",
                "lt" => " < ",
                "or" => " || ",
                "and" => " && ",
                _ => string.Empty,
            };
        }
    }
}
