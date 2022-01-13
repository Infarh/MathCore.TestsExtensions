using System.Collections;
using System.Reflection;
using System.Text;

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

public class DataRowSourceAttribute : Attribute
{
    /// <summary>Получает данные для вызова метода теста.</summary>
    public object[] Data { get; }

    /// <summary>Получает или задает отображаемое имя в результатах теста для настройки.</summary>
    public string DisplayName { get; set; }

    /// <summary>Добавляемый к имени теста префикс</summary>
    public string Prefix { get; set; }

    /// <summary>
    /// Имя метода-источника данных.<br/>
    /// Метод должен возвращать перечисление объектов,
    /// поля, или свойства которых должны соответствовать именам переменных тестового метода.<br/>
    /// Метод может возвращать перечисление кортежей.<br/>
    /// В этом случае сопоставление данных производится в соответствии с порядком следования параметров в кортеже.<br/>
    /// Если указан тип <see cref="SourceType"/>, то поиск метода осуществляется в нём.<br/>
    /// Иначе выполняется попытка обнаружить статический метод в классе модульного теста.
    /// </summary>
    public string MethodSourceName { get; set; }

    /// <summary>
    /// Тип, содержащий метод-источник данных.<br/>
    /// Если тип не указан, то поиск осуществляется в классе модульного теста.
    /// </summary>
    public Type SourceType { get; set; }

    public DataRowSourceAttribute() => Data = Array.Empty<object>();

    /// <summary>Инициализирует новый экземпляр класса <see cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.DataRowSourceAttribute" />.</summary>
    /// <param name="data">Объект данных.</param>
    public DataRowSourceAttribute(object data) => Data = new[] { data };

    /// <summary>Инициализирует новый экземпляр класса <see cref="T:Microsoft.VisualStudio.TestTools.UnitTesting.DataRowSourceAttribute" />, принимающий массив аргументов.</summary>
    /// <param name="data">Объект данных.</param>
    /// <param name="MoreData">Дополнительные данные.</param>
    public DataRowSourceAttribute(object data, params object[] MoreData)
    {
        if (MoreData is null)
        {
            Data = new[] { data };
            return;
        }

        Data = new object[MoreData.Length + 1];
        Data[0] = data;

        Array.Copy(MoreData, 0, Data, 1, MoreData.Length);
    }

    public IEnumerable<object[]> GetData(MethodInfo TestMethod)
    {
        if (MethodSourceName is not { Length: > 0 } method_name)
        {
            yield return Data;
            yield break;
        }

        var type = SourceType
            ?? TestMethod.DeclaringType 
            ?? throw new InvalidOperationException("Не удалось определить класс модульных тестов");

        var source_method = type.GetMethod(method_name, BindingFlags.Static | BindingFlags.Public)
            ?? type.GetMethod(method_name, BindingFlags.Static | BindingFlags.NonPublic);

        object instance = null;
        if (source_method is null)
        {
            source_method = type.GetMethod(method_name, BindingFlags.Instance | BindingFlags.Public)
                ?? type.GetMethod(method_name, BindingFlags.Instance | BindingFlags.NonPublic);

            if (source_method is null)
                throw new InvalidOperationException("Не удалось найти статический метод-источник данных в классе модульного теста");

            instance = Activator.CreateInstance(type);
        }

        var test_data = source_method.Invoke(instance, null) as IEnumerable
            ?? throw new InvalidOperationException("Не удалось получить результат вызова метода-источника данных");

        var test_method_parameters = TestMethod.GetParameters();

        foreach (var data in test_data)
        {
            if (data is null) continue;

            var data_type = data.GetType();
            var value_tuple_fields = data_type.FullName!.StartsWith("System.ValueTuple")
                ? data_type.GetFields()
                : null;

            var result = new object[test_method_parameters.Length];
            for (var i = 0; i < result.Length; i++)
            {
                var parameter_name = test_method_parameters[i].Name!;

                if (value_tuple_fields is not null)
                    result[i] = value_tuple_fields[i].GetValue(data);
                else if (data is IDictionary dictionary)
                    result[i] = dictionary[parameter_name];
                else if (data_type.GetProperty(parameter_name) is { } property)
                    result[i] = property.GetValue(data);
                else if (data_type.GetField(parameter_name) is { } field)
                    result[i] = field.GetValue(data);
            }

            yield return result;
        }
    }

    public string GetDisplayName(MethodInfo TestMethod, object[] data)
    {
        if (!string.IsNullOrWhiteSpace(DisplayName)) return DisplayName;
        if (data is null) return null;

        var result = new StringBuilder(Prefix ?? TestMethod.Name);
        result.Append("(");
        var parameters = TestMethod.GetParameters();
        for (var i = 0; i < parameters.Length; i++)
        {
            result.Append(parameters[i].Name);
            result.Append(": ");
            var value = data[i];
            if (value is IEnumerable e)
            {
                result.Append("{ ");
                var any = false;
                foreach (var e_value in e)
                {
                    any = true;
                    result.Append(e_value);
                    result.Append(", ");
                }

                if (any)
                    result.Length -= 2;
                result.Append(" }");
            }
            else
                result.Append(value);

            result.Append(", ");
        }

        if (parameters.Length > 0)
            result.Length -= 2;
        result.Append(")");


        return result.ToString();
    }
}