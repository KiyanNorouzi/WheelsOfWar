using UnityEngine;
using System.Collections;

public class LoaderSceneController : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.5f);

        if (ShouldPlayTutorial())
            SceneManager.LoadScene(Scenes.TutorialStartScene);
        else
            SceneManager.LoadInitialScene();
    }

    bool ShouldPlayTutorial()
    {
        /*if (PlayerPrefs.GetInt("skiptutorial", 0) == 0 &&  string.IsNullOrEmpty(Data.LastLoginedUsername))
            return true;
        else
            return false;*/
        return (!Application.isEditor && Accounting.Instance.currentUser.statistics.Battles == 0);
    }
}