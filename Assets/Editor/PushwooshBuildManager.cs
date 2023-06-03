﻿
//Thank you Unity for the best defines ever 
#define UNITY_3_PLUS
#define UNITY_4_PLUS
#define UNITY_5_PLUS
#if UNITY_2_6
#define UNITY_2_X
#undef UNITY_3_PLUS
#undef UNITY_4_PLUS
#undef UNITY_5_PLUS
#elif UNITY_3_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
#define UNITY_3_X
#undef UNITY_4_PLUS
#undef UNITY_5_PLUS
#elif UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
#define UNITY_4_X
#undef UNITY_5_PLUS
#elif UNITY_5_0 || UNITY_5_1 || UNITY_5_2 || UNITY_5_3 || UNITY_5_4 || UNITY_5_5 || UNITY_5_6 || UNITY_5_7 || UNITY_5_8 || UNITY_5_9
#define UNITY_5_X
#endif

using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;

public class PushwooshBuildManager : MonoBehaviour 
{
	[PostProcessBuild]
	private static void onPostProcessBuildPlayer(BuildTarget target, string pathToBuiltProject) {
#if UNITY_4_X
		if (target == BuildTarget.iPhone) {
#else
		if (target == BuildTarget.iOS) {
#endif
			var scriptPath = Path.Combine (Application.dataPath, "Editor/PushwooshPostProcessoriOS.py");
			var args = string.Format ("\"{0}\" \"{1}\" \"{2}\" \"{3}\"", scriptPath, pathToBuiltProject, target.ToString (), Pushwoosh.APP_CODE);
			runScript(scriptPath, args, "python");
		}
		
		if (target == BuildTarget.WP8Player) {
			postProcessWP8Build(pathToBuiltProject);
		}
	}

	private static void postProcessWP8Build(string pathToBuiltProject) {
		string manifestFilePath = Path.Combine( Path.Combine (pathToBuiltProject, PlayerSettings.productName), "Properties/WMAppManifest.xml");

		if (!File.Exists (manifestFilePath)) {
			UnityEngine.Debug.LogError ("Windows Phone manifest not found: " + manifestFilePath);
			return;
		}

		XmlDocument manifest = new XmlDocument ();
		manifest.Load(manifestFilePath);

		XmlNode capabilities = manifest.SelectSingleNode ("//Capabilities");
		XmlNodeList matchingCapability = manifest.SelectNodes("//Capability[@Name='ID_CAP_IDENTITY_DEVICE']");
		if (matchingCapability.Count == 0) {
			XmlElement newCapability = manifest.CreateElement("Capability");
			newCapability.SetAttribute("Name", "ID_CAP_IDENTITY_DEVICE");
			capabilities.AppendChild(newCapability);
		}

		matchingCapability = manifest.SelectNodes("//Capability[@Name='ID_CAP_PUSH_NOTIFICATION']");
		if (matchingCapability.Count == 0) {
			XmlElement newCapability = manifest.CreateElement("Capability");
			newCapability.SetAttribute("Name", "ID_CAP_PUSH_NOTIFICATION");
			capabilities.AppendChild(newCapability);
		}

		manifest.Save (manifestFilePath);

		UnityEngine.Debug.Log ("Windows Phone manifest sucessfully patched");
	}

	private static void runScript(string scriptPath, string args, string executor) {
		if (!File.Exists (scriptPath)) {
			UnityEngine.Debug.LogError ("Editor script not found: " + scriptPath + ". Did you accidentally delete it?");
			return;
		}

		var proc = new Process
		{
			StartInfo = new ProcessStartInfo
			{
				FileName = executor,
				Arguments = args,
				UseShellExecute = false,
				RedirectStandardOutput = true
			}
		};
		
		proc.Start ();
		
		string output = proc.StandardOutput.ReadToEnd ();
		proc.WaitForExit ();
		
		UnityEngine.Debug.Log (scriptPath + ": " + output);
	}
}
