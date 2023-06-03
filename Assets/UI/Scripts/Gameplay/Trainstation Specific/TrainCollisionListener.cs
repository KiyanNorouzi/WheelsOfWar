using UnityEngine;
using System.Collections;

public class TrainCollisionListener : MonoBehaviour
{
    public DestructionCause cause;

    void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < collision.contacts.Length; i++)
        {
            ScrCarController car = collision.contacts[i].otherCollider.GetComponent<ScrCarController>() as ScrCarController;
            if (car != null && car.IsAlive)
            {
                if (cause == DestructionCause.Train)
                {
                    GameplayUI.Instance.newsWall.SubmitText("* " + car.Username, ExtraSigns.Train, true);
                    GameplayUI.Instance.logPanel.SubmitText(car.Username, LogPanelMessages.TrainCome);
                }
                else
                {
                    GameplayUI.Instance.newsWall.SubmitText("* " + car.Username, ExtraSigns.Worm, true);
                    GameplayUI.Instance.logPanel.SubmitText(car.Username, LogPanelMessages.WormCome);
                }

                car.CrashedByNaturalCause(cause);
            }
        }
    }
}

public enum DestructionCause
{
    Train,
    Worm
}