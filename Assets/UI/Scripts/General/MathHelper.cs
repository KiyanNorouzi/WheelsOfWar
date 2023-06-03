using UnityEngine;
using System.Collections;

public class MathHelper : MonoBehaviour 
{
	public static float ToRadians(float degrees)
	{
		return (float)(degrees * (Mathf.PI / 180));
	}
	
	public static float ToDegrees(float radians)
	{
		return (float)(radians * (180 / Mathf.PI));
	}
	
	public static float AngleBetween(Vector2 p1, Vector2 p2, bool inDegrees)
	{
		float degrees = Mathf.Atan2((p1.y - p2.y), (p1.x - p2.x));
		return (inDegrees) ? MathHelper.ToDegrees(degrees) : degrees;
	}
	
	public static float AngleBetween(Vector3 p1, Vector3 p2, bool inDegrees)
	{
		float degrees = Mathf.Atan2((p1.y - p2.y), (p1.x - p2.x));
		return (inDegrees) ? MathHelper.ToDegrees(degrees) : degrees;
	}

    public static float GetDegreeStraight(float degrees)
    {
        degrees %= 360;
        if (degrees < 0)
            degrees += 360;

        return degrees;
    }

    public static string GetStringWithComma(int n)
    {
        string coinsText = n.ToString();
        int length = coinsText.Length;
        int loopCount = length / 3;

        if (length % 3 == 0)
            loopCount--;

        for (int i = 0; i < loopCount; i++)
        {
            coinsText = coinsText.Insert(length - (3 * (i + 1)) - i, ",");
            length = coinsText.Length;
        }

        return coinsText;
    }

    public static string GetTimeString(float totalSeconds)
    {
        int hours, minutes, seconds;
        hours = minutes = seconds = 0;

        if (totalSeconds >= 3600)
        {
            hours = (int)(totalSeconds / 3600);
            totalSeconds %= 3600;
        }

        if (totalSeconds >= 60)
        {
            minutes = (int)(totalSeconds / 60);
            totalSeconds %= 60;
        }

        seconds = (int)(totalSeconds);

        if (hours > 0)
            return string.Format("{0}:{1:00}':{2:00}\"", hours, minutes, seconds);
        else
            return string.Format("{0:00}':{1:00}\"", minutes, seconds);
    }

    public static float GetEaseFlow(float flow, NemoEaseMode ease)
    {
        switch (ease)
        {
            case NemoEaseMode.Linear: return flow;
            case NemoEaseMode.CubicIn: return flow * flow * flow;
            case NemoEaseMode.CubicOut: return (flow - 1) * (flow - 1) * (flow - 1) + 1;
            case NemoEaseMode.CubicInOut: return ((flow * 2 < 1) ? flow * flow * flow * 4.0f : ((flow - 1) * (flow - 1) * (flow - 1) * 8 + 1) / 2.0f + 0.5f);
        }
        return 0;
    }
}

public enum NemoEaseMode
{
    Linear,
    CubicIn,
    CubicOut,
    CubicInOut
}