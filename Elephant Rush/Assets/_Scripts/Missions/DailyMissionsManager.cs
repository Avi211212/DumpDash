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
            lastRefreshDate = DateTime.UtcNow.AddHours(5).AddMinutes(30).Date.AddDays(-1); // Convert to IST and subtract one day
        }
    }

    private void CheckForMissionRefresh()
    {
        // Convert current UTC time to IST
        DateTime currentIST = DateTime.UtcNow.AddHours(5).AddMinutes(30);

        if (currentIST.Date > lastRefreshDate)
        {
            RefreshMission();
            lastRefreshDate = currentIST.Date;
            PlayerPrefs.SetString(LastRefreshDateKey, lastRefreshDate.ToBinary().ToString());
            PlayerPrefs.Save();
        }
        else
        {
            float timeLeft = (float)(lastRefreshDate.AddDays(1) - currentIST).TotalSeconds;
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
