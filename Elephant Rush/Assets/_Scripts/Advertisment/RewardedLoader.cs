using UnityEngine;
using UnityEngine.Advertisements;

public class RewardedLoader : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private string androidAdUnitId;
    [SerializeField] private string iosAdUnitId;

    string adUnitId;

    public void Initialize()
    {
#if UNITY_IOS
        adUnitId = iosAdUnitId;
#elif UNITY_ANDROID
        adUnitId = androidAdUnitId;
#elif UNITY_EDITOR
        adUnitId = androidAdUnitId;//for testing
#endif
    }

    public void LoadAd()
    {
        print("Loading Rewarded!!");
        Advertisement.Load(adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId.Equals(adUnitId))
        {
            print("Rewarded loaded!!");
            ShowAd();
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        print("Rewarded failed to load");
    }

    public void ShowAd()
    {
        print("showing Rewarded ad!!");
        Advertisement.Show(adUnitId, this);
        Time.timeScale = 0;
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        print("Rewarded clicked");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Time.timeScale = 1;
        if (placementId.Equals(adUnitId) && showCompletionState.Equals(UnityAdsCompletionState.COMPLETED))
        {
            print("Rewarded show complete , Distribute the rewards");
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        print("Rewarded show failure");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        print("Rewarded show start");
    }
}