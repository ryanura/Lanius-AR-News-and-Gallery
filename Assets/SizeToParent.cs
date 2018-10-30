using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizeToParent : MonoBehaviour {

	public RawImage rawImage;
	

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		rawImage.SizeToParent();
		Debug.Log("sizetoparent");

	}

}
