using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class AppManager : MonoBehaviour {

	AsyncOperation ao;
	[SerializeField]public GameObject loadingScreenBG, ToolbarGO;
	[SerializeField]public Slider loadingSlider;
	[SerializeField]public Text loadingText;
	public static AppManager instance;	
	public static Canvas AppCanvas;
	public RawImage BG;
	public Button NewsFeedBtn, 
				ARModeBtn, 
				SettingsBtn, 
				NavDrawerBtn, 
				NavDrawBG, 
				ProfileBtn, 
				GalleryBtn,
				GalleryDetailBtn,
				SignOutBtn;
				
	public GameObject SettingsView, 
				NewsFeedView, 
				CategoriesBar, 
				NavDrawer, 
				Profile,
				Gallery,
				GalleryFull,
				GalleryDetail,
				NewsArticle;

	public TMP_Text topbarTxt;
	private const float	LOAD_READY_PERCENTAGE = 0.9F;
	private float timeOutTimer = 0f;
	private float timeOut = 60f;
	
	private string currentScene;
	//bool isNavActive = false;

	void Awake()
	{
		if (instance == null)
	 	{
	 		instance = this;
	 	}else{
	 		Destroy(this.gameObject);
	 		return;
	 	}
	 }

	// Use this for initialization
	void Start () {

		ARModeBtn.onClick.AddListener(()=> ChangeToARMode());
		NewsFeedBtn.onClick.AddListener(()=> EnableUIPanel(NewsFeedView));
		SettingsBtn.onClick.AddListener(()=> EnableUIPanel(SettingsView));
		NavDrawerBtn.onClick.AddListener(()=> OpenNavigationDrawer(true));
		NavDrawBG.onClick.AddListener(()=> OpenNavigationDrawer(false));
		ProfileBtn.onClick.AddListener(()=> EnableUIPanel(Profile));
		GalleryBtn.onClick.AddListener(()=> EnableUIPanel(Gallery));
		SignOutBtn.onClick.AddListener(()=> ExitApplication());
		
		NavDrawer.SetActive(false);

		loadingScreenBG.SetActive(false);
		loadingSlider.gameObject.SetActive(false);
		loadingText.gameObject.SetActive(false);

		currentScene = SceneManager.GetActiveScene().name;
		
	}

	void ChangeToARMode()
	{
		if(currentScene != "Vuforia-3-VideoPlayback"){
			DisableAllUI();

			ChangeScene("Vuforia-3-VideoPlayback");

			topbarTxt.text = "AR";
		}else{
			return;
		}
	}

	void ExitApplication(){
		Application.Quit();
	}

	public void EnableUIPanel(GameObject _PanelObject)
	{
		if(currentScene != "NewsFeed"){
			ChangeScene ("NewsFeed");
		}

		DisableAllUI();

		BG.gameObject.SetActive(true);
		_PanelObject.SetActive(true);
		topbarTxt.text = _PanelObject.name;
	}

	public void EnableDisableSomeUIPanel(GameObject _PanelObject) 
	{
		bool activate;
		if (_PanelObject.activeSelf == false){
			activate = true;
		}else{
			activate = false;
		}

		_PanelObject.SetActive(activate);
	}
	

	void OpenNavigationDrawer(bool isNavActive)
	{
		NavDrawer.SetActive(isNavActive);
	}

	void DisableAllUI()
	{
		GameObject[] Allpanel = GameObject.FindGameObjectsWithTag("Panel");
	
		foreach (GameObject panel in Allpanel)
		{
			panel.SetActive(false);
		}

		 this.gameObject.SetActive(true);
		 ToolbarGO.SetActive(true);
	}

	public void ChangeScene(string sceneName)
	{
		loadingText.text = "loading...";

		loadingScreenBG.SetActive(true);
		loadingSlider.gameObject.SetActive(true);
		loadingText.gameObject.SetActive(true);

		StartCoroutine(LoadingSceneRealProgress(sceneName));
	}

	/// <summary>
	/// Callback sent to all game objects when the player gets or loses focus.
	/// </summary>
	/// <param name="focusStatus">The focus state of the application.</param>
	void OnApplicationFocus(bool focusStatus)
	{
	}

	/// <summary>
	/// Callback sent to all game objects when the player pauses.
	/// </summary>
	/// <param name="pauseStatus">The pause state of the application.</param>
	void OnApplicationPause(bool pauseStatus)
	{
	}

	IEnumerator LoadingSceneRealProgress(string sceneName)
	{
		yield return new WaitForSeconds (1);
		ao = SceneManager.LoadSceneAsync(sceneName);
		ao.allowSceneActivation = false;
		
		while (!ao.isDone)
		{
			loadingSlider.value = ao.progress;
			if(ao.progress >= LOAD_READY_PERCENTAGE)
			{
				loadingSlider.value = 1f;
				loadingText.text = "scene loaded";
				ao.allowSceneActivation = true;
			}
		
			yield return null;
		}

		loadingScreenBG.SetActive(false);
		loadingSlider.gameObject.SetActive(false);
		loadingText.gameObject.SetActive(false);

		currentScene = SceneManager.GetActiveScene().name;
	}

	IEnumerator timeOutCounting (float maxtimeout)
	{
		while (currentScene == "Vuforia-3-VideoPlayback" )
		{
			timeOutTimer += Time.deltaTime;
			yield return new WaitForSeconds(1);

			loadingText.gameObject.SetActive(true);
			loadingText.text = timeOutTimer.ToString();

			if(Input.GetMouseButtonDown(0))
			{
				timeOutTimer = 0f;
			}
		}
	}
}
