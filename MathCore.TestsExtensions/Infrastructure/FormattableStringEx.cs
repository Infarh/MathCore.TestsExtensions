using System.Runtime.CompilerServices;

namespace Microsoft.VisualStudio.TestTools.UnitTesting.Infrastructure;

internal static class FormattableStringEx
{
    extension(FormattableString str)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToStringInvariant() => str.ToString(CultureInfo.InvariantCulture);
    }
}
