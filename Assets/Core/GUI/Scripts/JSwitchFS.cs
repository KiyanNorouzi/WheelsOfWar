using UnityEngine;
using System.Collections;

public class JSwitchFS : MonoBehaviour {
	public Texture2D toFS;
	public Texture2D toWin;
	
	int winx;
	int winy;
	int fsx;
	int fsy;
	int refresh;
	
	void Start() {
		Resolution res = Screen.currentResolution;
		fsx = res.width;
		fsy = res.height;
		refresh = res.refreshRate;
		winx = Screen.width;
		winy = Screen.height;
		if (Screen.fullScreen) {
			winx = (winx < 680)?(winx - 8):640;
			winy = (winy < 520)?(winy - 40):480; 
		}
	}
	
	void OnGUI() {
		if (GUI.Button(new Rect(16, 16, 34, 24), (Screen.fullScreen)?toWin:toFS, GUIStyle.none)) {
			if (Screen.fullScreen) {
				Screen.SetResolution(winx, winy, false, refresh);
			}
			else {
				Screen.SetResolution(fsx, fsy, true, refresh);
			}
		}
	}
}
