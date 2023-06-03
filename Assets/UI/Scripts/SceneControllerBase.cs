using UnityEngine;
using System.Collections;

public abstract class SceneControllerBase : MonoBehaviour 
{
    public Scenes scene;

    protected virtual void Awake()
    {
        if (scene != Scenes.Loading && CommonUI.Instance == null)
            SceneManager.LoadGame(scene);
        else
            CommonUI.Instance.currentScene = this;
    }


    public abstract void BackButton_Click();
}
