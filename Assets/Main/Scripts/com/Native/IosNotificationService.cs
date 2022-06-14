using UnityEngine;
using System.Collections;
#if UNITY_IOS
using Unity.Notifications.iOS;
#endif

using System;

namespace com
{
    public class IosNotificationService : MonoBehaviour
    {
#if UNITY_IOS
        public int calendarTrigger_h = 12;
        public int calendarTrigger_m = 40;

        public string notifId = "test-zqt";

        private void Start()
        {
            CheckService();
        }

        public void CheckService()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                StartCoroutine(RequestAuthorization());
            }
        }

        IEnumerator RequestAuthorization()
        {
            var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge | AuthorizationOption.Sound;
            using (var req = new AuthorizationRequest(authorizationOption, true))
            {
                while (!req.IsFinished)
                {
                    yield return null;
                };

                string res = "\n RequestAuthorization:";
                res += "\n finished: " + req.IsFinished;
                res += "\n granted :  " + req.Granted;
                res += "\n error:  " + req.Error;
                res += "\n deviceToken:  " + req.DeviceToken;
                Debug.Log(res);
                if (req.IsFinished && req.Granted)
                {
                    TryPushNotif();
                }
            }
        }


        void TryPushNotif()
        {
            //clear delivered
            iOSNotificationCenter.RemoveAllDeliveredNotifications();

            var notifs = iOSNotificationCenter.GetScheduledNotifications();
            if (notifs != null)
            {
                Debug.Log("has ScheduledNotifications "+ notifs.Length);
                foreach (var notif in notifs)
                {
                    Debug.Log(notif.Identifier);
                    if (notif.Identifier == notifId)
                    {
                        //if exist do nothing
                        return;
                    }
                }

                ScheduleNotif();
            }

            void ScheduleNotif()
            {
                var calendarTrigger = new iOSNotificationCalendarTrigger()
                {
                    Hour = calendarTrigger_h,
                    Minute = calendarTrigger_m,
                    Repeats = true//so everyday
                };

                var notification = new iOSNotification()
                {
                    Identifier = notifId,
                    Title = LocalizationService.instance.GetLocalizedText("notif_tt"),
                    Subtitle = LocalizationService.instance.GetLocalizedText("notif_sb"),
                    Body = LocalizationService.instance.GetLocalizedText("notif_bd"),
             
                    ShowInForeground = false,
                    ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Badge | PresentationOption.Sound),
                    CategoryIdentifier = "category_a",
                    ThreadIdentifier = "thread1",
                    Trigger = calendarTrigger,
                };

                iOSNotificationCenter.ScheduleNotification(notification);
            }

            void IntervalNotif()
            {
                var minutes = 1;
                var seconds = 1;
                var timeTrigger = new iOSNotificationTimeIntervalTrigger()
                {
                    TimeInterval = new TimeSpan(0, minutes, seconds),
                    Repeats = false
                };

                var notification = new iOSNotification()
                {
                    // You can specify a custom identifier which can be used to manage the notification later.
                    // If you don't provide one, a unique string will be generated automatically.
                    Identifier = "_notification_01",
                    Title = "Title",
                    Body = "Scheduled at: " + DateTime.Now.ToShortDateString() + " triggered in 5 seconds",
                    Subtitle = "This is a subtitle, something, something important...",
                    ShowInForeground = true,
                    ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                    CategoryIdentifier = "category_a",
                    ThreadIdentifier = "thread1",
                    Trigger = timeTrigger,
                };

                iOSNotificationCenter.ScheduleNotification(notification);

                //The following code example cancels a notification if it doesn’t trigger.
                iOSNotificationCenter.RemoveScheduledNotification(notification.Identifier);

                //The following code example removes a notification from the Notification Center if it's already delivered.
                iOSNotificationCenter.RemoveDeliveredNotification(notification.Identifier);

                // For example, if you only set the hour and minute fields, 
                //the system automatically triggers the notification on the next specified hour and minute.
                var calendarTrigger = new iOSNotificationCalendarTrigger()
                {
                    // Year = 2020,
                    // Month = 6,
                    //Day = 1,
                    Hour = calendarTrigger_h,
                    Minute = calendarTrigger_m,
                    // Second = 0
                    Repeats = true
                };
            }
        }
#endif
    }
}