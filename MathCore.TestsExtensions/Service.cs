namespace Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>Сервисные методы</summary>
internal static class Service
{
    /// <summary>Добавить разделитель к сообщению в конце в случае наличия сообщения</summary>
    /// <param name="Str">Строка сообщения</param>
    /// <param name="Separator">Разделитель</param>
    /// <returns>Доработанная строка сообщения</returns>
    public static string? AddSeparator(this string? Str, string Separator = "|") =>
        string.IsNullOrEmpty(Str) || Str!.EndsWith(Separator) 
            ? Str 
            : Str + Separator;

    /// <summary>Проверить строку сообщения на предмет наличия в ней разделителя в конце и добавить разделитель в случае его отсутствия</summary>
    /// <param name="Str">Проверяемая строка сообщения</param>
    /// <param name="Separator">Разделитель сообщения</param>
    public static void CheckSeparator(ref string? Str, string Separator = "|")
    {
        if (string.IsNullOrEmpty(Str) || Str!.EndsWith(Separator)) return;
        Str += Separator;
    }
}