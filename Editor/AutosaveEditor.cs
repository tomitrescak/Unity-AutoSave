using UnityEngine;
using UnityEditor;
using System;

public class AutoSaveEditor : EditorWindow {

	private string scenePath;    
	
	[MenuItem ("Window/AutoSave")]
	static void Init () {
		AutoSaveEditor saveWindow = (AutoSaveEditor) EditorWindow.GetWindow (typeof (AutoSaveEditor));
		saveWindow.Show();
	}
	
	void OnGUI () {	
		EditorGUILayout.BeginVertical ();
		Autosaver.Interval = EditorGUILayout.IntSlider ("Interval (minutes): ", (int) Autosaver.Interval, 1, 30);
		Autosaver.AutoSave = EditorGUILayout.Toggle ("Auto save: ", Autosaver.AutoSave);
		Autosaver.ShowMessage = EditorGUILayout.BeginToggleGroup ("Show Message: ", Autosaver.ShowMessage);

		EditorGUILayout.LabelField ("Last save:", Autosaver.LastSave.ToString(), GUILayout.ExpandWidth(true), GUILayout.MinWidth(300));
		EditorGUILayout.EndVertical ();
	}
}