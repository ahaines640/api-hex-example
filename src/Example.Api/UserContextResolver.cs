
using Example.Domain;

namespace Example.Api
{
    public class UserContextResolver : IUserContextResolver
    {
        public string CurrentUser => "test@example.com";
    }
}
