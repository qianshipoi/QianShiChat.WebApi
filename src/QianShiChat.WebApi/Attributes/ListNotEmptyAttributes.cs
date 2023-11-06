namespace QianShiChat.WebApi.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class ListNotEmptyAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return value is IEnumerable list ? list.GetEnumerator().MoveNext() : false;
    }
}
