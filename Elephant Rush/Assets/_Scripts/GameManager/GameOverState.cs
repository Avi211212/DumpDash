using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
#if UNITY_ANALYTICS
using UnityEngine.Analytics;
#endif
using System.Collections.Generic;
 
/// <summary>
/// state pushed on top of the GameManager when the player dies.
/// </summary>
public class GameOverState : AState
{
    public TrackManager trackManager;
    public Canvas canvas;
    public MissionUI missionPopup;

	public AudioClip gameOverTheme;

	public Leaderboard miniLeaderboard;
	public Leaderboard fullLeaderboard;

    public GameObject addButton;

    public ShareScreen shareScreen;
    [SerializeField] private SharingServices sharingServices;

    [SerializeField] private Image gameOverImage;
    [SerializeField] private GameObject gameOverObjects;

    private GameObject shareScreenImage;

    public override void Enter(AState from)
    {
        Camera.main.transform.SetParent(null, true);

        canvas.gameObject.SetActive(true);

        PopulateShareScreen();
    }

	public override void Exit(AState to)
    {
        canvas.gameObject.SetActive(false);
        FinishRun();
    }

    public override string GetName()
    {
        return "GameOver";
    }

    public override void Tick() { }

    public void PopulateShareScreen()
    {
        gameOverObjects.SetActive(false);
        gameOverImage.enabled = false;

        Camera.main.transform.position = new Vector3(0.0f, 13.0f, -20.0f);
        shareScreen.gameObject.SetActive(true);
        string characterName = trackManager.characterController.character.characterName;
        shareScreen.characterName.text = characterName;
        shareScreen.score.text = "Score : " + trackManager.score.ToString();
        if(PlayerData.instance.charactersName.ContainsKey(characterName))
        {
            shareScreen.playerName.text = PlayerData.instance.charactersName[characterName];
        }
        else
        {
            shareScreen.playerName.text = characterName;
        }
        PlayerData.instance.previousName = PlayerData.instance.charactersName[characterName];
        shareScreenImage = Instantiate(shareScreen.shareScreenImage, Camera.main.transform, false);
        shareScreenImage.transform.localPosition = new Vector3(0.0f, 0.0f, 13.66f);

        Character character = trackManager.characterController.character;
        character.SwitchEyes();
        character.animator.SetTrigger("ShareScreen");
        character.gameObject.transform.SetParent(Camera.main.transform, false);
        character.gameObject.transform.localPosition = new Vector3(0.0f, -0.5f, 4.0f);
        character.gameObject.transform.localRotation = Quaternion.Euler(4.0f,-180.0f,0.0f);
        character.gameObject.transform.localScale = Vector3.one * 0.75f;
        
    }

    public void Share()
    {
        sharingServices.Share(trackManager.score);
    }
    public void DoubleScoreByAd()
    {
        AdManager.instance.rewardedLoader.ShowAd();
        trackManager.score *= 2;
        shareScreen.score.text = "Score : " + trackManager.score.ToString();
    }

    public void Return()
    {
        trackManager.characterController.character.gameObject.transform.SetParent(null, false);
        shareScreenImage.SetActive(false);
        shareScreen.gameObject.SetActive(false);

        gameOverImage.enabled = true;
        gameOverObjects.SetActive(true);

        miniLeaderboard.playerEntry.inputName.text = PlayerData.instance.previousName;

        miniLeaderboard.playerEntry.score.text = trackManager.score.ToString();
        miniLeaderboard.Populate();

        if (PlayerData.instance.AnyMissionComplete())
            StartCoroutine(missionPopup.Open());
        else
            missionPopup.gameObject.SetActive(false);

        CreditCoins();

        trackManager.characterController.characterCollider.hitAttackableCount = 0;

        if (MusicPlayer.instance.GetStem(0) != gameOverTheme)
        {
            MusicPlayer.instance.SetStem(0, gameOverTheme);
            StartCoroutine(MusicPlayer.instance.RestartAllStems());
        }
    }

	public void OpenLeaderboard()
	{
		fullLeaderboard.forcePlayerDisplay = false;
		fullLeaderboard.displayPlayer = true;
		fullLeaderboard.playerEntry.playerName.text = miniLeaderboard.playerEntry.inputName.text;
		fullLeaderboard.playerEntry.score.text = trackManager.score.ToString();

		fullLeaderboard.Open();
    }

	public void GoToStore()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("shop", UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }


    public void GoToLoadout()
    {
        trackManager.isRerun = false;
		manager.SwitchState("Loadout");
    }

    public void RunAgain()
    {
        trackManager.isRerun = false;
        manager.SwitchState("Game");
    }

    protected void CreditCoins()
	{
		PlayerData.instance.Save();

#if UNITY_ANALYTICS // Using Analytics Standard Events v0.3.0
        var transactionId = System.Guid.NewGuid().ToString();
        var transactionContext = "gameplay";
        var level = PlayerData.instance.rank.ToString();
        var itemType = "consumable";
        
        if (trackManager.characterController.coins > 0)
        {
            AnalyticsEvent.ItemAcquired(
                AcquisitionType.Soft, // Currency type
                transactionContext,
                trackManager.characterController.coins,
                "fishbone",
                PlayerData.instance.coins,
                itemType,
                level,
                transactionId
            );
        }

        if (trackManager.characterController.premium > 0)
        {
            AnalyticsEvent.ItemAcquired(
                AcquisitionType.Premium, // Currency type
                transactionContext,
                trackManager.characterController.premium,
                "anchovies",
                PlayerData.instance.premium,
                itemType,
                level,
                transactionId
            );
        }
#endif 
	}

	protected void FinishRun()
    {
		if(miniLeaderboard.playerEntry.inputName.text == "")
		{
			miniLeaderboard.playerEntry.inputName.text = "Chota Hathi";
		}
		else
		{
			PlayerData.instance.previousName = miniLeaderboard.playerEntry.inputName.text;
		}

        PlayerData.instance.InsertScore(trackManager.score, miniLeaderboard.playerEntry.inputName.text );

        CharacterCollider.DeathEvent de = trackManager.characterController.characterCollider.deathData;
        //register data to analytics
#if UNITY_ANALYTICS
        AnalyticsEvent.GameOver(null, new Dictionary<string, object> {
            { "coins", de.coins },
            { "premium", de.premium },
            { "score", de.score },
            { "distance", de.worldDistance },
            { "obstacle",  de.obstacleType },
            { "theme", de.themeUsed },
            { "character", de.character },
        });
#endif

        PlayerData.instance.Save();

        trackManager.End();
    }

    //----------------
}
