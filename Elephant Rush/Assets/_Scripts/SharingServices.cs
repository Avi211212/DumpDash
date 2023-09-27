using System.Collections;
using System.Collections.Generic;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;
using UnityEngine.UI;
using UnityEngine;

public class SharingServices : MonoBehaviour
{
    private int gameScore;
    private string text;
    private string url;

    public void Share(int score)
    {
        gameScore = score;
        text = "I just scored " + gameScore + " points in Indian Elephant Rush! Can you beat me? ";
        url = "https://play.google.com/store/apps/details?id=com.supercell.clashroyale";

        ShareScreenshotWhatsApp();
    }

    private void ShareScreenshotFacebook()
    {
        SocialShareComposer composer = SocialShareComposer.CreateInstance(SocialShareComposerType.Facebook);
        composer.AddScreenshot();
        composer.SetCompletionCallback((result, error) => {
            Debug.Log("Social Share Composer was closed. Result code: " + result.ResultCode);
        });
        composer.Show();
    }

    private void ShareScreenshotTwitter()
    {
        SocialShareComposer composer = SocialShareComposer.CreateInstance(SocialShareComposerType.Twitter);
        composer.AddScreenshot();
        composer.SetCompletionCallback((result, error) => {
            Debug.Log("Social Share Composer was closed. Result code: " + result.ResultCode);
        });
        composer.Show();
    }

    private void ShareScreenshotWhatsApp()
    {
        SocialShareComposer composer = SocialShareComposer.CreateInstance(SocialShareComposerType.WhatsApp);
        composer.SetText(text + url);
        composer.AddScreenshot();
        composer.SetCompletionCallback((result, error) => {
            Debug.Log("Social Share Composer was closed. Result code: " + result.ResultCode);
        });
        composer.Show();
    }
}
