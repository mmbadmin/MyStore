namespace MyStore.Common.Utilities
{
    using RExtension;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;

    public static class Helper
    {
        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source,
                                                                  Func<TSource, TSource> nextItem,
                                                                  Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem)
            where TSource : class
        {
            return source.FromHierarchy(nextItem, s => s != null);
        }

        public static string GetMessage(this Exception exception)
        {
            var messages = exception.FromHierarchy(ex => ex.InnerException).Select(ex => ex.Message);
            return string.Join(Environment.NewLine, messages);
        }

        public static IEnumerable<string> GetMessages(this Exception exception)
        {
            return exception.FromHierarchy(ex => ex.InnerException).Select(ex => ex.Message);
        }

        public static bool IsEmpty<TSource>([NotNullWhen(false)] this Expression<Func<TSource, bool>>? predicate)
        {
            if (predicate == null)
            {
                return true;
            }
            if (predicate.Body.ToString().ToLower().FullTrim() == "false")
            {
                return true;
            }
            return false;
        }

        public static bool IsNotEmpty<TSource>([NotNullWhen(false)] this Expression<Func<TSource, bool>>? predicate)
        {
            return predicate.IsEmpty() == false;
        }

        public static bool CheckDuplicate(this IEnumerable<string> list, string title)
        {
            title = RemoveSpaces(title).ToLower().FullTrim();
            foreach (var item in list)
            {
                var temp = RemoveSpaces(item).ToLower().FullTrim();
                if (temp == title)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetName(this Enum @enum)
        {
            if (@enum == null)
            {
                return string.Empty;
            }
            var field = @enum.GetType().GetField(@enum.ToString());
            if (field == null)
            {
                return string.Empty;
            }
            var attributes = field.GetCustomAttributes(typeof(DisplayAttribute), false);
            DisplayAttribute? displayAttribute = null;
            if (attributes.Any())
            {
                if (attributes.ElementAt(0) is DisplayAttribute temp)
                {
                    displayAttribute = temp;
                }
            }
            return displayAttribute?.Name ?? string.Empty;
        }

        public static string GetDescription(this Enum @enum)
        {
            if (@enum == null)
            {
                return string.Empty;
            }
            var field = @enum.GetType().GetField(@enum.ToString());
            if (field == null)
            {
                return string.Empty;
            }
            var attributes = field.GetCustomAttributes(typeof(DisplayAttribute), false);
            DisplayAttribute? displayAttribute = null;
            if (attributes.Any())
            {
                if (attributes.ElementAt(0) is DisplayAttribute temp)
                {
                    displayAttribute = temp;
                }
            }
            return displayAttribute?.Description ?? string.Empty;
        }

        public static string RemoveSpaces(string str)
        {
            if (str.IsEmpty())
            {
                return string.Empty;
            }
            var pattern = @"[\s]{1,}";
            var options = RegexOptions.None;
            var regex = new Regex(pattern, options);
            return regex.Replace(str, string.Empty);
        }

        public static byte[] StreamToByte(Stream stream)
        {
            using (var fileStream = stream)
            {
                using (var ms = new MemoryStream())
                {
                    fileStream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        public static string GetFileExtension(string fileName, bool withDot = false)
        {
            var ext = Path.GetExtension(fileName);
            if (!withDot)
            {
                return ext.Replace(".", string.Empty);
            }
            return ext;
        }

        public static string GetFileName(string fileName)
        {
            return Path.GetFileNameWithoutExtension(fileName);
        }
    }
}
