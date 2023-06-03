using UnityEngine;
using System.Collections;
using System;

public class ServerTime : MonoBehaviour 
{
    #region Singleton

    static ServerTime _instance;
    public static ServerTime Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    #endregion




    public delegate void dayChanged();
    public event dayChanged OnDayChanged;

    DateTime syncTime;


    int dayIndex;
    public int DayIndex
    {
        get { return dayIndex; }
    }

    int hour;
    public int Hour
    {
        get { return hour; }
    }


    int minute;
    public int Minute
    {
        get { return minute; }
    }

    int seconds;
    public int Seconds
    {
        get { return seconds; }
    }




    public void SetTime(int dayIndex, int hour, int minute)
    {
        this.dayIndex = dayIndex;
        this.hour = hour;
        this.minute = minute;
        this.seconds = 0;

        syncTime = DateTime.Now;
    }


    public void RefreshTimeLocally()
    {
        TimeSpan t = DateTime.Now - syncTime;

        hour += t.Hours;
        minute += t.Minutes;
        seconds += t.Seconds;

        if (seconds >= 60)
        {
            seconds -= 60;
            minute++;
        }

        if (minute >= 60)
        {
            minute -= 60;
            hour++;
        }

        if (hour >= 24)
        {
            hour -= 24;
            dayIndex++;

            if (OnDayChanged != null)
                OnDayChanged();
        }


        syncTime = DateTime.Now;
    }


    Action done;
    public void RefreshTimeFromServer(Action done)
    {
        this.done = done;
        Accounting.Instance.GetServerTime(_ServerTimeFetched, _ServerTimeFailed);
    }

    private void _ServerTimeFetched(int dayIndex, int hour, int minute)
    {
        bool isDayChanged = (dayIndex != this.dayIndex);

        SetTime(dayIndex, hour, minute);
        if (done != null)
            done();

        if (OnDayChanged != null)
            OnDayChanged();
    }

    private void _ServerTimeFailed(bool isServerError, string errorText)
    {
        Debug.Log("failed to sync the time with server, server error: " + isServerError + "=" + errorText);
    }
}
