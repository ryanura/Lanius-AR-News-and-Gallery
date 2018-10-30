using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Newtonsoft.Json;
using TMPro;

public class AuthHandler : MonoBehaviour {
	protected Firebase.Auth.FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;
 	private string logText = "";
	private string uid = "";
	private string email = "";
	private string password = "";
	private string displayName = "";
	private string phoneNumber = "";
	private string receivedCode = "";
	private string idTokenString = "";
	private System.Uri photoUrl;

	private bool fetchingToken = false;
	public bool usePasswordInput = false;
	public GameObject SignUpPanel, 
	SignInPanel; 
	//ForgotPasswordPanel;

    //public InputField UserNameInput, PasswordInput;
    public Button SignupButton, 
	LoginButton, 
	//FacebookLoginButton, 
	//GoogleLoginButton, 
	gotoSignUpButton, 
	gotoSignInButton;
	//SignOutButton;
	public Text StatusText;
	public TMP_InputField UserNameInput, PasswordInput; 
	const int kMaxLogSize = 16382;
	Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
	
	void Start () {
		//SignOut();

		Screen.SetResolution(576, 1024, true);
		Debug.Log (Screen.currentResolution);

		dependencyStatus = Firebase.FirebaseApp.CheckDependencies();
		if (dependencyStatus != Firebase.DependencyStatus.Available)
		{
			Firebase.FirebaseApp.FixDependenciesAsync().ContinueWith (task=>
			{
				if (dependencyStatus == Firebase.DependencyStatus.Available)
				{
					InitializeFirebase();
				} else {
					DebugLog("Could not resolve all Firebase dependencies: " + dependencyStatus);
				}
			});
		} else {
			InitializeFirebase();
		}

		SignupButton.onClick.AddListener(() => SignUpWithEmailPassword(UserNameInput.text, PasswordInput.text));
        LoginButton.onClick.AddListener(() => SigninWithEmailPassword(UserNameInput.text, PasswordInput.text));
       // FacebookLoginButton.onClick.AddListener(() => SigninWithFacebook(null));
		//GoogleLoginButton.onClick.AddListener(() => SigninWithEmailPassword(null, null));
		//SignOutButton.onClick.AddListener(() => SignOut());
		gotoSignUpButton.onClick.AddListener(() => EnableCertainPanel(SignUpPanel));
		gotoSignInButton.onClick.AddListener(() => EnableCertainPanel(SignInPanel));


		UserNameInput.text = "demo@lanius.com";
		PasswordInput.text = "123456789";

	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	#region UI

	void EnableCertainPanel(GameObject _panel)
	{
		DisableAllPanel();
		_panel.SetActive(true);
	}

	void DisableAllPanel()
	{
		GameObject[] Allpanel = GameObject.FindGameObjectsWithTag("Panel");

		foreach (GameObject panel in Allpanel)
		{
			panel.SetActive(false);
		}
	}

	#endregion

	// Use this for initialization1
	void InitializeFirebase() {
		DebugLog ("Setting up Firebase Auth");
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		auth.StateChanged += AuthStateChanged;
		//auth.IdTokenChanged += IdTokenChanged;
		AuthStateChanged(this, null);
	}

	void AuthStateChanged (object sender, System.EventArgs EventArgs)
	{
		if (auth.CurrentUser != user)
		{
			bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
			if (!signedIn && user != null)
			{
				DebugLog ("Signed out " + user.UserId);
			}
			user = auth.CurrentUser;
			if (signedIn)
			{
				DebugLog("Signed in "+user.UserId);
				displayName = user.DisplayName ?? "";
				email = user.Email ?? "";
				photoUrl = user.PhotoUrl;
				uid = user.UserId ?? "";
			}
		}
	}

	void OnDestroy()
	{
		auth.StateChanged -= AuthStateChanged;
    	auth = null;
		/*auth.IdTokenChanged -= IdTokenChanged;
    	
		if (otherAuth != null) {
		otherAuth.StateChanged -= AuthStateChanged;
		otherAuth.IdTokenChanged -= IdTokenChanged;
		otherAuth = null;
		}*/
	}

	// Output text to the debug log text field, as well as the console.
	public void DebugLog (string s)
	{
		Debug.Log (s);
		logText += s + "\n";
		while (logText.Length > kMaxLogSize)
		{
			int index = logText.IndexOf("\n");
			logText = logText.Substring(index + 1);
		}
		StatusText.text = logText;
	}

	public void SignOut()
	{
		auth.SignOut();
		DebugLog("Signed Out");
	}

	private void SigninWithEmailPassword (string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task=>
        {
            if (task.IsCanceled)
            {
                DebugLog("SignInWithEmailAndPasswordAsync canceled.");
                return;
            }
            
            if (task.IsFaulted)
            {
                DebugLog("SignInWithEmailAndPasswordAsync error :" + task.Exception);
                if (task.Exception.InnerExceptions.Count > 0)
                DebugLog(task.Exception.InnerExceptions[0].Message);
                return;
            }

            FirebaseUser user = task.Result;
            //Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);
			DebugLog(String.Format ("User signed in successfully: {0} ({1})", user.DisplayName, user.UserId));

            PlayerPrefs.SetString("LoginUser", user != null ? user.Email : "Unknow");
            var userRef = PlayerPrefs.GetString ("LoginUser", "Unknown");
            DebugLog ("Succesfully signed in as " + userRef);
            
            if (task.IsCompleted){
                SceneManager.LoadScene("NewsFeed");
            }            
        });
    }

	public void SignUpWithEmailPassword(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            //Error handling
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith (task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }

            if (task.IsFaulted) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            StatusText.text = String.Format("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
        
        });
    }

	private void SigninWithFacebook(string token)
	{
		//Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		
		Firebase.Auth.Credential credential  = Firebase.Auth.FacebookAuthProvider.GetCredential(token);
		auth.SignInWithCredentialAsync(credential).ContinueWith (task =>
		{
			if (task.IsCanceled){
				DebugLog ("SignInWithCredentialAsync was canceled");
				return;
			}
			 if (task.IsFaulted)
			{
				DebugLog("SignInWithCredentialAsync encountered an error: " + task.Exception);
				return;
			}

			Firebase.Auth.FirebaseUser newUser = task.Result;
			DebugLog("User signed in successfully: " +
                    newUser.DisplayName + " " + newUser.UserId);
		});
	}

	private void SigninWithGoogle(string googleIdToken, string googleAccessToken)
	{
		//Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		
		Firebase.Auth.Credential credential  = Firebase.Auth.GoogleAuthProvider.GetCredential(googleIdToken, googleAccessToken);
		auth.SignInWithCredentialAsync(credential).ContinueWith (task =>
		{
			if (task.IsCanceled){
				DebugLog ("SignInWithCredentialAsync was canceled");
				return;
			}
			 if (task.IsFaulted)
			{
				DebugLog("SignInWithCredentialAsync encountered an error: " + task.Exception);
				return;
			}

			Firebase.Auth.FirebaseUser newUser = task.Result;
			DebugLog("User signed in successfully: " +
                    newUser.DisplayName + " " + newUser.UserId);
		});
	}
}
