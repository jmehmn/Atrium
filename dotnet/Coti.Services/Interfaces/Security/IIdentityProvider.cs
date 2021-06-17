namespace Coti.Services
{
    public interface IIdentityProvider<T>
    {
        T GetCurrentUserId();
    }
}