using System.Threading.Tasks;
using Unity.Services.Authentication;

public class GameLoginService:IService
{
    public bool Initialized { get { return AuthenticationService.Instance.IsSignedIn; } }

    public async Task Initialize()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public void Clear()
    {
    }
}
