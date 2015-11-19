using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using ServiceStack.OrmLite;

namespace PictureAuction.Api.ServiceInterface
{
    public static class DbConnectionEx
    {
        public static bool Any<T>(this IDbConnection db, Expression<Func<T, bool>> predicate) where T : new()
        {
            return db.Select<T>(q => q.Where(predicate).Limit(1)).Any();
        }

        public static T FirstOrDefaultById<T>(this IDbConnection db, object id) where T : new()
        {
            var key = OrmLiteConfig.DialectProvider.GetQuotedColumnName(ModelDefinition<T>.PrimaryKeyName);
            var value = OrmLiteConfig.DialectProvider.GetQuotedValue(id, id.GetType());
            return db.Select<T>(x => x.Limit(1).Where($"{key} = {value}")).FirstOrDefault();
        }
    }
}