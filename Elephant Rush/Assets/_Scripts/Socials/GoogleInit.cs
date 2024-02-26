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
    Task task;

    bool isSignedIn = false;

}
#endif
