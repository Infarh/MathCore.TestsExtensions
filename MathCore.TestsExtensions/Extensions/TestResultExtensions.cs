using System.Collections;
using System.Runtime.CompilerServices;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Microsoft.VisualStudio.TestTools.UnitTesting.Extensions;

public static class TestResultExtensions
{
    extension(TestResult result)
    {
        public TestResult LogWriteLine(string Text)
        {
            var log = new StringBuilder(result.LogOutput).AppendLine(Text);
            result.LogOutput = log.ToString();
            return result;
        }

        public TestResult ToDebug<T>(T value, [CallerArgumentExpression(nameof(value))] string? Prefix = null)
        {
            if (Prefix is { Length: > 0 })
            {
                FormattableString msg = $"{Prefix} = {value}";
                result.LogWriteLine(msg.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                FormattableString msg = $"{value}";
                result.LogWriteLine(msg.ToString(CultureInfo.InvariantCulture));
            }

            return result;
        }

        public TestResult ToDebugEnum(IEnumerable items, [CallerArgumentExpression(nameof(items))] string? Name = null)
        {
            var log = new StringBuilder(result.LogOutput);
            if (Name is { Length: > 0 })
                log.AppendFormat("object[] {0} = {{\r\n", Name);
            var i = 0;
            var culture = CultureInfo.InvariantCulture;
            foreach (var item in items)
            {
                if (i > 0)
                    log.AppendLine(",");

                FormattableString msg = $"            /*[{i,2}]*/ {item}";
                log.AppendLine(msg.ToString(culture));

                i++;
            }
            log.AppendLine("");
            log.AppendLine("}");

            result.LogOutput = log.ToString();
            return result;
        }

        public TestResult ToDebugEnum<T>(IEnumerable<T> items, [CallerArgumentExpression(nameof(items))] string? Name = null)
        {
            var log = new StringBuilder(result.LogOutput);
            if (Name is { Length: > 0 })
                log.AppendFormat("{0}[] {1} = {{\r\n", typeof(T).Name, Name);
            var i = 0;
            var culture = CultureInfo.InvariantCulture;
            foreach (var item in items)
            {
                if (i > 0)
                    log.AppendLine(",");

                FormattableString msg = $"            /*[{i,2}]*/ {item}";
                log.Append(msg.ToString(culture));

                i++;
            }
            log.AppendLine("");
            log.AppendLine("}");

            result.LogOutput = log.ToString();
            return result;
        }
    }
}
