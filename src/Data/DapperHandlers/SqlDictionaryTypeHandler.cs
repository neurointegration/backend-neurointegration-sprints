using Dapper;
using System.Data;
using System.Text.Json;

namespace Data.DapperHandlers
{
    public class SqlDictionaryTypeHandler<TKey, TVal> : SqlMapper.TypeHandler<Dictionary<TKey, TVal>?>
        where TKey : notnull
    {
        // Этот метод не работает, когда значение равно null.
        // Это особенность работы Dapper.
        // Получаем такую ошибку: Writing value of 'System.DBNull' is not supported for parameters having DbType 'Object'
        // В этой ситуации можно проблемные типы передавать в параметрах самого запроса, предварительно преобразовав к string?
        public override void SetValue(IDbDataParameter parameter, Dictionary<TKey, TVal>? value)
        {
            parameter.Value = JsonSerializer.Serialize(value);
        }

        // А этот метод работает нормально
        public override Dictionary<TKey, TVal>? Parse(object value)
        {
            if ((string)value == "Null") return null;
            return JsonSerializer.Deserialize<Dictionary<TKey, TVal>?>((string)value);
        }
    }
}
