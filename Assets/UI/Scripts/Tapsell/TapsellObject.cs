using UnityEngine;
using System.Collections;

public class TapsellObject{
	public TapsellWrapper wrapper;
	public GameObject gameObject;

	public TapsellObject(){
		gameObject = new GameObject ("TapsellObject");
		Object.DontDestroyOnLoad (gameObject);
		wrapper = gameObject.AddComponent<TapsellWrapper>();
	}
}
