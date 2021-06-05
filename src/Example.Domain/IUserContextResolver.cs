namespace Example.Domain
{
    public interface IUserContextResolver
    {
        string CurrentUser { get; }
    }
}