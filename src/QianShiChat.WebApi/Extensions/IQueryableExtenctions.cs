namespace QianShiChat.WebApi.Extensions;

public static class IQueryableExtenctions
{
    /// <summary>
    /// 条件成立则执行
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="ifFunc"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IQueryable<T> IfWhere<T>(this IQueryable<T> source, Func<bool> ifFunc, Expression<Func<T, bool>> predicate)
    {
        if (ifFunc.Invoke())
        {
            return source.Where(predicate);
        }
        else
        {
            return source;
        }
    }
}