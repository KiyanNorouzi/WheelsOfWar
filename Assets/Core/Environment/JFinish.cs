using UnityEngine;
using System.Collections;

public class JFinish : MonoBehaviour {
	public GUISkin guiskin;
	public JSpawnGhosts spawnGhosts;
	public Rigidbody who;
	
	public JPort[] ports;
	
	public int round = 0;
	bool started = false;
	float startTime;
	float bestTime = -1f;
	float roundTime = -1f;
	float passTime = 0f;

	int targetPort = 0;
	
	// Use this for initialization
	void Start () {
		for (int i = 0; i < ports.Length; i++) {
			JPort p = ports[i];
			p.finish = this;
			p.nr = i;
		}
	}
	
	void ClearPorts() {
		foreach (JPort p in ports) {
			p.passed = false;
		}
		targetPort = 0;
	}
	
	public bool PassPort(int nr) {
		if (!started) return false;
		if (targetPort == nr) {
			targetPort = nr + 1;
			return true;
		}
		return (nr < targetPort);
	}
	
	public Vector3 Target() {
		if (started && (targetPort < ports.Length)) {
			return ports[targetPort].transform.position;
		}
		return transform.position;
	}
		
	void OnTriggerEnter(Collider colInfo) {
		if (colInfo.attachedRigidbody != who) return;
		if (!started) {
			started = true;
			startTime = Time.time;
			passTime = startTime;
			if (spawnGhosts != null) {
				spawnGhosts.DoSpawn();
			} 
		}
		else {
			if (targetPort >= ports.Length) {
				ClearPorts();
				round++;
				float t = Time.time;
				roundTime = t - passTime;
				passTime = t;
				if ((roundTime < bestTime) || (bestTime <= 0f)) {
					bestTime = roundTime;
				}
				if (spawnGhosts != null) {
					spawnGhosts.DoSpawn();
				} 
			}
		}
	}
	
	string FmtTime(float t) {
		float sec = Mathf.Repeat(t, 60f);
		int min = (int) (t / 60f);
		return "" + min + ":"  + ((sec < 10)?"0":"") + sec.ToString("f2");
	}
	
	void OnGUI() {
		if (guiskin != null) GUI.skin = guiskin;
		if (!started) {
			GUILayout.BeginArea(new Rect(Screen.width -128 - 16, 16, 128, 128), "Controls:", GUI.skin.window);
			GUILayout.Label("Use left-right arrow keys to steer, up-down to accelerate, decelerate and space to brake.");
			GUILayout.EndArea();
			return;
		}

		GUILayout.BeginArea(new Rect(Screen.width -128 - 16, 16, 128, 128), "Round " + (round + 1), GUI.skin.window);
		float t = Time.time;
		GUILayout.Label("Total time: " + FmtTime(t - startTime));
		GUILayout.Label("Round time: " + FmtTime(t - passTime));
		if (roundTime > 0f) {
			GUILayout.Label("Last round: " + FmtTime(roundTime));
		}
		if (bestTime > 0f) {
			GUILayout.Label("Best round: " + FmtTime(bestTime));
		}
		GUILayout.EndArea();		
	}
}
