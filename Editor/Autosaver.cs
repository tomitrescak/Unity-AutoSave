using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Timers;
using System;

[InitializeOnLoad]
public class Autosaver {

	static Timer tim;
	static bool showMessage;
	static bool autosave;
	static DateTime lastSave;

	const string IntervalPref = "AutoSaveInterval";
	const string ShowMessagePref = "ShowMessage";
	const string EnableAutosavePref = "EnableAutosave";
	const int interval = 60000;

	public static int Interval {
		get {
			return PlayerPrefs.GetInt(IntervalPref);
		}
		set {
			if (PlayerPrefs.GetInt(IntervalPref) != value)
			{
				tim.Interval = value * interval;
				// save options
				PlayerPrefs.SetInt(IntervalPref, value);
				PlayerPrefs.Save();
			}
		}
	}

	public static bool ShowMessage { 
		get {
			return PlayerPrefs.GetInt(ShowMessagePref) == 1;
		} 
		set {
			if (value && PlayerPrefs.GetInt(ShowMessagePref) == 0 ||
			    !value && PlayerPrefs.GetInt(ShowMessagePref) == 1) 
			{
				PlayerPrefs.SetInt(ShowMessagePref, value ? 1 : 0);
				PlayerPrefs.Save();
			}
		}
	}

	public static bool AutoSave { 
		get {
			return PlayerPrefs.GetInt(EnableAutosavePref) == 1;
		} 
		set {
			if (value && PlayerPrefs.GetInt(EnableAutosavePref) == 0 ||
			    !value && PlayerPrefs.GetInt(EnableAutosavePref) == 1) 
			{
				PlayerPrefs.SetInt(EnableAutosavePref, value ? 1 : 0);
				PlayerPrefs.Save();
				ToggleAutoSave();
			}
		}
	}

	public static DateTime LastSave { get; set; }

	static Autosaver() {

		// initialise options
		if(!PlayerPrefs.HasKey(EnableAutosavePref))
		{
			PlayerPrefs.SetInt(EnableAutosavePref, 0);
			PlayerPrefs.SetInt(ShowMessagePref, 1);
			PlayerPrefs.SetInt(IntervalPref, 1);
			PlayerPrefs.Save();
			Debug.Log("Autosaver options created");
		}

		// initialise multi threading
	
		Threader.Enable ();

		tim = new Timer ();
		tim.Interval = Interval * interval;
		tim.Elapsed += (object sender, ElapsedEventArgs e) => {

			Threader.RunOnMain (() => {
				var scenePath = EditorApplication.currentScene;

				// save scene
				EditorApplication.SaveScene (scenePath);

				// save assets
				AssetDatabase.SaveAssets();

				LastSave = DateTime.Now;

				if(ShowMessage){
					Debug.Log ("AutoSave saved: " + scenePath + " on " + DateTime.Now.ToString ());
				}
			});
		};


		ToggleAutoSave();

	}

	static void ToggleAutoSave() {
		tim.Enabled = AutoSave;


		if (ShowMessage) {
			if (AutoSave) { Debug.Log ("Autosaver started"); }
			else { Debug.Log ("Autosaver stopped"); }
		}
	}
}


