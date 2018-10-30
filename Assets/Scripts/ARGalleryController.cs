using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
public class ARGalleryController : MonoBehaviour {

	public Button _backButtonBtn, 
				_screenShotBtn,
				_OnScreenGalleryBtn,
				_CloseBtn,
				_OpenLinkBtn;
	public Canvas _OnScreenCanvas,
				_worldCanvas;
	public bool _isOnScreenCanvas;
	public string _URL;
	

	// Use this for initialization
	void Start () {
		_backButtonBtn.onClick.AddListener(()=> BackOnePage());
		_screenShotBtn.onClick.AddListener(()=> TakeScreenshot());
		_OnScreenGalleryBtn.onClick.AddListener(()=> EnableOnScreenGallery());
		_CloseBtn.onClick.AddListener(()=> CloseAR());
		_OpenLinkBtn.onClick.AddListener(()=> OpenWebPage());

		_isOnScreenCanvas = false;
		_OnScreenCanvas.gameObject.SetActive(false);
		_worldCanvas.gameObject.SetActive(true);

	}

	void OpenWebPage()
	{
		Application.OpenURL(_URL);
		//Application.Quit();
	}
	
	void CloseAR () {
		if (!_isOnScreenCanvas)
		{
			_isOnScreenCanvas = false;
			_worldCanvas.gameObject.SetActive(true);
			_OnScreenCanvas.gameObject.SetActive(false);
		}else
		{
			return;
		}
	}

	void ShowMoreSettings () {
		
	}

	void BackOnePage()
	{
		if (_isOnScreenCanvas)
		{
			_isOnScreenCanvas = false;
			_OnScreenCanvas.gameObject.SetActive(false);
			_worldCanvas.gameObject.SetActive(true);
		}
	}

	void TakeScreenshot()
	{

	}

	void EnableOnScreenGallery()
	{
		if(!_isOnScreenCanvas)
		{
			_worldCanvas.gameObject.SetActive(false);
			_OnScreenCanvas.gameObject.SetActive(true);
			_isOnScreenCanvas  = true;
		}
	}
}
