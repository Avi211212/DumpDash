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
using GooglePlayGames.BasicApi.SavedGame;

public class GoogleInit : MonoBehaviour
{
    [SerializeField] private Text detailsText;

    private PlayerData playerData;

    private PlayGamesClientConfiguration clientConfiguration;

    private bool IsSignedIn => PlayGamesPlatform.Instance.IsAuthenticated();

    private void Start()
    {
        ConfigureGPGS();

        if (IsSignedIn)
        {
            detailsText.text = Social.localUser.userName;
            LoadGameData(); // Load game data if the player is signed in
        }
        else
        {
            detailsText.text = "Google";
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
                LoadGameData(); // Load game data after successful sign-in
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
            SaveGameData(); // Save game data before signing out
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
        detailsText.text = "Google";
    }

    // Save game data to the cloud
    private void SaveGameData()
    {
        if (IsSignedIn)
        {
            string dataToSave = JsonUtility.ToJson(playerData);

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.OpenWithAutomaticConflictResolution("SaveGame", DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, (status, metadata) => OnSavedGameOpened(status, metadata, dataToSave));
        }
        else
        {
            Debug.Log("Google Play Games: Not authenticated. Cannot save data.");
        }
    }

    // Callback when the saved game is opened
    private void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game, string dataToSave)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(dataToSave);

            // Use SavedGameMetadataUpdate without the unnecessary Builder()
            SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
            PlayGamesPlatform.Instance.SavedGame.CommitUpdate(game, update, data, OnSavedGameWritten);
        }
        else
        {
            Debug.LogError("Google Play Games: Failed to open saved game for writing.");
        }
    }

    // Callback when the saved game is written
    private void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("Google Play Games: Game data saved successfully.");
        }
        else
        {
            Debug.LogError("Google Play Games: Failed to write game data.");
        }
    }

    // Load game data from the cloud
    private void LoadGameData()
    {
        if (IsSignedIn)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.OpenWithAutomaticConflictResolution("SaveGame", DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpenedForRead);
        }
        else
        {
            Debug.Log("Google Play Games: Not authenticated. Cannot load data.");
        }
    }

    // Callback when the saved game is opened for reading
    private void OnSavedGameOpenedForRead(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(game, OnSavedGameDataRead);
        }
        else
        {
            Debug.LogError("Google Play Games: Failed to open saved game for reading.");
        }
    }

    // Callback when the saved game data is read
    private void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            string jsonData = System.Text.Encoding.UTF8.GetString(data);
            playerData = JsonUtility.FromJson<PlayerData>(jsonData);

            Debug.Log("Google Play Games: Game data loaded successfully.");
            // Do something with loaded data, e.g., update UI
        }
        else
        {
            Debug.LogError("Google Play Games: Failed to read game data.");
        }
    }


}
#endif


