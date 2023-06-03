using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HitDirection : MonoBehaviour 
{
    public Image[] effectImages;
    public float stayDuration, fadeDuration;
    public Transform testTransform;


    float[] deactiveTimes, fadeTimes;




	void Start()
    {
        deactiveTimes = new float[effectImages.Length];
        fadeTimes = new float[effectImages.Length];

        for (int i = 0; i < effectImages.Length; i++)
        {
            effectImages[i].enabled = false;
            deactiveTimes[i] = fadeTimes[i] = -1;
        }
	}

    public void Clear()
    {
        if (deactiveTimes == null)
            return;

        for (int i = 0; i < effectImages.Length; i++)
        {
            deactiveTimes[i] = -1;
            fadeTimes[i] = -1;

            Color c = effectImages[i].color;
            c.a = 0;
            effectImages[i].color = c;
        }
    }

	public void Show(float degrees)
    {
        degrees = MathHelper.GetDegreeStraight(degrees + 22.5f);
        //int dirIndex = Mathf.RoundToInt(degrees / 45f);
        int dirIndex = (int)(degrees / 45f);
        dirIndex %= 8;

        _Show(dirIndex);
    }

    public void Show(Direction8 dir)
    {
        int dirIndex = (int)dir;
        _Show(dirIndex);
    }

    void _Show(int dirIndex)
    {
        effectImages[dirIndex].enabled = true;
        deactiveTimes[dirIndex] = Time.time + stayDuration;
        fadeTimes[dirIndex] = -1;

        Color c = effectImages[dirIndex].color;
        c.a = 1;
        effectImages[dirIndex].color = c;
    }

    void Update()
    {
        for (int i = 0; i < deactiveTimes.Length; i++)
        {
            if (deactiveTimes[i] != -1)
            {
                if (deactiveTimes[i] <= Time.time)
                {
                    deactiveTimes[i] = -1;
                    fadeTimes[i] = fadeDuration;
                }
            }

            if (fadeTimes[i] != -1)
            {
                fadeTimes[i] -= Time.deltaTime;

                if (fadeTimes[i] <= 0)
                {
                    fadeTimes[i] = -1;
                    effectImages[i].enabled = false;
                }
                else
                {
                    Color c = effectImages[i].color;
                    c.a = Mathf.Clamp01(fadeTimes[i] / fadeDuration);
                    effectImages[i].color = c;
                }
            }
        }
    }

    public void ShowRelative(Vector3 myPosition, Vector3 myForwardDirection, Vector3 enemyPosition)
    {
        float degrees = MathHelper.ToDegrees(Mathf.Atan2(myForwardDirection.z, myForwardDirection.x));

        Vector3 diff = enemyPosition - myPosition;
        float diffDegrees = MathHelper.ToDegrees(Mathf.Atan2(diff.z, diff.x));

        float final = diffDegrees - degrees;

        final += 90;
        Show(final);
    }

    public void Test()
    {
        if (ScrCarController.Instance != null)
            ShowRelative(ScrCarController.Instance.transform.position, ScrCarController.Instance.transform.forward, testTransform.position);
    }
}

public enum Direction8
{
    East,
    NorthEast,
    North,
    NorthWest,
    West,
    SouthWest,
    South,
    SouthEast
}