using System;
using UnityEngine;

public class DailyMissionsManager : MonoBehaviour
{
    private DateTime lastRefreshDate;
    private const string LastRefreshDateKey = "LastRefreshDate";

    void Start()
    {
        LoadLastRefreshDate();
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
    }

    private void RefreshMission()
    {
        PlayerData.instance.missions.Clear();
        PlayerData.instance.CheckMissionsCount();
    }
}
