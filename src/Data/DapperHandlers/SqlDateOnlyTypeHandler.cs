using Dapper;
using System.Data;

namespace Data.DapperHandlers
{
    public class SqlDateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
    {
        public override void SetValue(IDbDataParameter parameter, DateOnly date)
        {
            parameter.DbType = DbType.Date;
            parameter.Value = date.ToDateTime(new TimeOnly(0, 0));
        }

        public override DateOnly Parse(object value) => DateOnly.FromDateTime((DateTime)value);
    }
}
