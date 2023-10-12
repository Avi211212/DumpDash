using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Notifications.Android;


public class DailyMissionsManager : MonoBehaviour
{
    [SerializeField] private MissionUI missionUI;
    [SerializeField] private Text refreshTime;
    private DateTime lastRefreshDate;
    private const string LastRefreshDateKey = "LastRefreshDate";

    void Start()
    {
        LoadLastRefreshDate();

        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
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
        var notification = new AndroidNotification()
        {
            Title = "Your Daily Missions have been Refreshed",
            Text = "Play Game to Complete Missions",
            SmallIcon = "icon",
            LargeIcon = "logo",
            FireTime = System.DateTime.Now.AddSeconds(5),
        };

        AndroidNotificationCenter.SendNotification(notification, "channel_id");

        PlayerData.instance.missions.Clear();
        PlayerData.instance.CheckMissionsCount();
        missionUI.StartCoroutine(missionUI.Open());
    }
}
