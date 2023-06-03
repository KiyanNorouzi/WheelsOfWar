using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JMessages : MonoBehaviour {

	public GUISkin skin;
	List<JMessageData> messages;
	public string version;

	// Use this for initialization
	void Start () {
		messages = new List<JMessageData>();
		ShowHelp();		
	}
	
	public void ShowHelp() {
		messages.Clear();
		AddMessage("r to reset car", 10);
		AddMessage("page up/page down to switch gear", 10);
		AddMessage("g toggles manual/automatic gears", 10);
		AddMessage("t cycles through all-, back- and frontdrive", 10);
		AddMessage("spacebar for brake", 10);
		AddMessage("arrows for steering, gas", 10);
		AddMessage("JCar: " + version, 60);
		AddMessage("press h to get short help", 60, 60);
	}
	
	public void AddMessage(string msg, float timeout) {
		float t = Time.time;
		JMessageData d = new JMessageData();
		d.str = msg;
		d.startTime = t;
		d.endTime = t + timeout;
		messages.Insert(0, d);
	}

	public void AddMessage(string msg, float delay, float timeout) {
		float t = Time.time;
		JMessageData d = new JMessageData();
		d.str = msg;
		d.startTime = t + delay;
		d.endTime = t + delay + timeout;
		messages.Insert(0, d);
	}

	void OnGUI() {
		if (skin != null) {
			GUI.skin = skin;
		}
		if (Input.GetKeyDown("h")) {
			ShowHelp();
		}
		
		JMessageData toRemove = null;
		if (messages.Count != 0) {
			GUILayout.BeginArea(new Rect(128 + 32, 16, 420, Screen.height - 32));
			float t = Time.time;
			foreach (JMessageData d in messages) {
				if (d.startTime <= t) {
					GUILayout.Label(d.str);
				}
				if (d.endTime < t) {
					toRemove = d;
				}
			}
			if (toRemove != null) {
				messages.Remove(toRemove);
			}
			GUILayout.EndArea();
		}
	}	
}
