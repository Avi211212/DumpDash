#if PLATFORM_ANDROID
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System;

public class GoogleInit : MonoBehaviour
{
    [SerializeField] private Text detailsText;
    
    private PlayGamesClientConfiguration clientConfiguration;

    private bool IsSignedIn => PlayGamesPlatform.Instance.IsAuthenticated();

    private void Start()
    {
        ConfigureGPGS();

        if(IsSignedIn)
        {
            detailsText.text = Social.localUser.userName;
        }
        else
        {
            detailsText.text = "Login";
        }
    }

    internal void ConfigureGPGS()
    {
        clientConfiguration = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
    }

    internal void SignIntoGPGS(SignInInteractivity interactivity, PlayGamesClientConfiguration configuration)
    {
        configuration = clientConfiguration;
        PlayGamesPlatform.InitializeInstance(configuration);
        PlayGamesPlatform.Activate();

        PlayGamesPlatform.Instance.Authenticate(interactivity, (code) =>
        {
            Debug.Log("Authenticating..");

            if (code == SignInStatus.Success)
            {
                Debug.Log("Sign in successful");
                detailsText.text = Social.localUser.userName;
            }
            else
            {
                Debug.Log("Sign in failed" + code);
                detailsText.text = "Failed";
            }
        });
    }

    public void LoginLogoutButton()
    {
        if (IsSignedIn)
        {
            SignOut();
        }
        else
        {
            SignIn();
        }
    }

    private void SignIn()
    {
        SignIntoGPGS(SignInInteractivity.CanPromptAlways, clientConfiguration);
    }

    private void SignOut()
    {
        PlayGamesPlatform.Instance.SignOut();
        detailsText.text = "Login";
    }

}
#endif
