using UnityEngine;
using Unity.Notifications.Android;

public class Notifications : MonoBehaviour
{
    void Start()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        var notification = new AndroidNotification()
        {
            Title = "Oye Tere Missions Reh gye hain",
            Text = "Missions kar",
            SmallIcon = "icon",
            LargeIcon = "logo",
            FireTime = System.DateTime.Now.AddSeconds(5),
        };

        AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }
}
