using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;
using UnityEngine;

public class SharingServices : MonoBehaviour
{
    private int gameScore;
    private string text;
    private string url;

    public void Share(int score)
    {
        gameScore = score;
        text = "I just scored " + gameScore + " points in Haathi Daaud! Can you beat me? ";
        url = "https://play.google.com/store/apps/details?id=com.supercell.clashroyale";

        ShareTextWithScreenshot();
    }

    private void ShareTextWithScreenshot()
    {
        ShareSheet shareSheet = ShareSheet.CreateInstance();
        shareSheet.AddText(text + url);
        shareSheet.AddScreenshot();
        shareSheet.SetCompletionCallback((result, error) => {
            Debug.Log("Share Sheet was closed. Result code: " + result.ResultCode);
        });
        shareSheet.Show();
    }
}
