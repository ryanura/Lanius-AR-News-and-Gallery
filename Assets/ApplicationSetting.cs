using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationSetting : MonoBehaviour {

	// Use this for initialization
	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		Application.targetFrameRate = 60;
		
		#if UNITY_EDITOR
		Debug.unityLogger.logEnabled = true;
		#else
		Debug.unityLogger.logEnabled = false;
		#endif
		
	} 

	// Update is called once per frame
	void Update () {
		
	}
}
