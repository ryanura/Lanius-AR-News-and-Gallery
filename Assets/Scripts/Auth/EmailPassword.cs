using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Auth;


public class EmailPassword : MonoBehaviour {

    private FirebaseAuth auth;
    public InputField UserNameInput, PasswordInput;
    public Button SignupButton, LoginButton;
    public Text ErrorText;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

       
        // UserNameInput.text = "demofirebase@gmail.com";
        // PasswordInput.text = "abcdefgh";

        SignupButton.onClick.AddListener(() => SignUp(UserNameInput.text, PasswordInput.text));
        LoginButton.onClick.AddListener(() => Login(UserNameInput.text, PasswordInput.text));
        
    }

    public void SignUp(string email, string password)
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
            ErrorText.text = String.Format("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
        
        });
    }

    private void UpdateErrorMessage(string message)
    {
        ErrorText.text = message;
        Invoke("ClearErrorMessage", 3);
    }

    void ClearErrorMessage()
    {
        ErrorText.text = "";
    }
    

    public void Login (string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task=>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync canceled.");
                return;
            }
            
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync error :" + task.Exception);
                if (task.Exception.InnerExceptions.Count > 0)
                UpdateErrorMessage(task.Exception.InnerExceptions[0].Message);
                return;
            }

            FirebaseUser user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);

            PlayerPrefs.SetString("LoginUser", user != null ? user.Email : "Unknow");
            var userRef = PlayerPrefs.GetString ("LoginUser", "Unknown");
            ErrorText.text = "Succesfully signed in as " + userRef;
            
            if (task.IsCompleted){
                //SceneManager.LoadScene("NewsFeed");
            }            
        });
    }
}
