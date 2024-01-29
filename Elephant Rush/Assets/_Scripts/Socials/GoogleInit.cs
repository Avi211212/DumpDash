using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

public class GoogleInit : MonoBehaviour
{
    public Text detailsText;
    
    private string pName =  "";

    public void SignIn()
    {
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if(pName != "" && pName != "Sign in Failed!!")
        {
            
        }
        else
        {
            if (status == SignInStatus.Success)
            {
                string name = PlayGamesPlatform.Instance.GetUserDisplayName();
                string id = PlayGamesPlatform.Instance.GetUserId();
                string ImgUrl = PlayGamesPlatform.Instance.GetUserImageUrl();

                pName = name;
                detailsText.text = name;
            }
            else
            {
                detailsText.text = "Sign in Failed!!";
                // Disable your integration with Play Games Services or show a login button
                // to ask users to sign-in. Clicking it should call
                // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
            }
        }
    }
}
