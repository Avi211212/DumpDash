using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;

public class GoogleInit : MonoBehaviour
{
    public Text detailsText;
    Task task;

    private string pName = "";

    async void Awake()
    {
        await UnityServices.InitializeAsync();
    }

    public void SignIn()
    {
        PlayGamesPlatform.Activate();
    }

    public void LoginGooglePlayGames()
    {
        PlayGamesPlatform.Instance.Authenticate((success) =>
        {
            if(pName == "")
            {
                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    task = UnlinkGooglePlayGamesAsync(code);
                });
            }
            else if (success == SignInStatus.Success)
            {
                Debug.Log("Login with Google Play games successful.");

                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    Debug.Log("Authorization code: " + code);

                    detailsText.text = PlayGamesPlatform.Instance.GetUserDisplayName();
                    pName = PlayGamesPlatform.Instance.GetUserDisplayName();

                    task = SignInWithGooglePlayGamesAsync(code);
                });
            }
            else
            {             
                Debug.LogError("Login Unsuccessful");
                Debug.LogError(success);
            }
        });
    }

    async Task SignInWithGooglePlayGamesAsync(string authCode)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode);
            Debug.Log("SignInWithGooglePlayGamesAsync is successful.");
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }

    async Task UnlinkGooglePlayGamesAsync(string idToken)
    {
        try
        {
            await AuthenticationService.Instance.UnlinkGooglePlayGamesAsync();
            Debug.Log("Unlink is successful.");
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
    }
}
