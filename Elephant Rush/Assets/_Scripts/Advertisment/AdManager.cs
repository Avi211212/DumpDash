using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener
{
    static protected AdManager s_Instance;
    static public AdManager instance { get { return s_Instance; } }

    [SerializeField] private string androidGameId;
    [SerializeField] private string iosGameId;

    [SerializeField] private bool isTestingMode = true;

    private string gameId;

    private void Awake()
    {
        if (s_Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        s_Instance = this;

        DontDestroyOnLoad(gameObject);

        InitializeAds();
    }

    private void InitializeAds()
    {
#if UNITY_IOS
        gameId = iosGameId;
#elif UNITY_ANDROID
        gameId = androidGameId;
#elif UNITY_EDITOR
        gameId = androidGameId;//for testing
#endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, isTestingMode, this); //ONLY ONCE
        }
    }

    public void OnInitializationComplete()
    {
        print("Ads initialized!!");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        print("failed to initialize!!");
    }
}
