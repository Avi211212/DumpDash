using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;
using UnityEngine;
using System.Collections;

public class SharingServices : MonoBehaviour
{
    [SerializeField] private GameObject shareButton;
    [SerializeField] private GameObject returnButton;

    private int gameScore;
    private string text;
    private string url;

    public void Share(int score)
    {
        shareButton.SetActive(false);
        returnButton.SetActive(false);

        gameScore = score;
        text = "I just scored " + gameScore + " points in Haathi Daaud! Can you beat me? ";
        url = "https://play.google.com/store/apps/details?id=com.supercell.clashroyale";

        StartCoroutine(ShareTextWithScreenshot());
    }

    private IEnumerator ShareTextWithScreenshot()
    {
        yield return new WaitForSeconds(0.5f);

        ShareSheet shareSheet = ShareSheet.CreateInstance();
        shareSheet.AddText(text + url);
        shareSheet.AddScreenshot();
        shareSheet.SetCompletionCallback((result, error) => {
            Debug.Log("Share Sheet was closed. Result code: " + result.ResultCode);
        });
        shareSheet.Show();

        StartCoroutine(EnableButtons());
    }

    private IEnumerator EnableButtons()
    {
        yield return new WaitForSeconds(1.0f);

        shareButton.SetActive(true);
        returnButton.SetActive(true);
    }
}
