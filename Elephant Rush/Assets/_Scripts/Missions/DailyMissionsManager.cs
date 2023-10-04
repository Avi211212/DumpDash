using System;
using UnityEngine;
using UnityEngine.UI;

public class DailyMissionsManager : MonoBehaviour
{
    [SerializeField] private MissionUI missionUI;
    [SerializeField] private Text refreshTime;
    private DateTime lastRefreshDate;
    private const string LastRefreshDateKey = "LastRefreshDate";

    void Start()
    {
        LoadLastRefreshDate();
    }

    void Update()
    {
        CheckForMissionRefresh();
    }

    private void LoadLastRefreshDate()
    {
        if (PlayerPrefs.HasKey(LastRefreshDateKey))
        {
            lastRefreshDate = DateTime.FromBinary(Convert.ToInt64(PlayerPrefs.GetString(LastRefreshDateKey)));
        }
        else
        {
            lastRefreshDate = DateTime.UtcNow.Date.AddDays(-1);
        }
    }

    private void CheckForMissionRefresh()
    {
        if (DateTime.UtcNow.Date > lastRefreshDate)
        {
            RefreshMission();
            lastRefreshDate = DateTime.UtcNow.Date;
            PlayerPrefs.SetString(LastRefreshDateKey, lastRefreshDate.ToBinary().ToString());
            PlayerPrefs.Save();
        }
        else
        {
            float timeLeft = (float)(lastRefreshDate.AddDays(1) - DateTime.UtcNow).TotalSeconds;
            float hours = Mathf.FloorToInt(timeLeft / 3600);
            float minutes = Mathf.FloorToInt((timeLeft - hours * 3600) / 60);
            float seconds = Mathf.FloorToInt(timeLeft - hours * 3600 - minutes * 60);
            if(refreshTime != null)
            {
                refreshTime.text = "Refreshes in " + hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
            }
        }
    }

    private void RefreshMission()
    {
        PlayerData.instance.missions.Clear();
        PlayerData.instance.CheckMissionsCount();
        missionUI.StartCoroutine(missionUI.Open());
    }
}
