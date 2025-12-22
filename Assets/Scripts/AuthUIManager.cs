using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

public class AuthUIManager : MonoBehaviour
{
    // ================= UI =================
    public GameObject landingPanel, loginPanel, registerPanel, notificationPanel;

    public InputField loginEmail, loginPassword;
    public InputField registerEmail, registerUsername, registerPassword, registerConfirmPassword;

    public Text notif_Title, notif_Message;

    // ================= FIREBASE =================
    private FirebaseAuth auth;
    private FirebaseUser user;

    // ================= UNITY =================
    void Start()
    {
        InitializeFirebase();
        ShowLanding();
    }

    // ================= FIREBASE INIT =================
    void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                auth.StateChanged += AuthStateChanged;
                user = auth.CurrentUser;

                Debug.Log("Firebase initialized");

                if (user != null)
                {
                    LoadGameScene();
                }
            }
            else
            {
                Debug.LogError("Firebase init failed: " + task.Result);
            }
        });
    }

    void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            user = auth.CurrentUser;
            if (user != null)
            {
                Debug.Log("Signed in: " + user.UserId);
            }
            else
            {
                Debug.Log("Signed out");
            }
        }
    }

    void OnDestroy()
    {
        if (auth != null)
        {
            auth.StateChanged -= AuthStateChanged;
        }
    }

    // ================= UI NAVIGATION =================
    public void ShowLanding()
    {
        landingPanel.SetActive(true);
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        notificationPanel.SetActive(false);
    }

    public void ShowLogin()
    {
        landingPanel.SetActive(false);
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        notificationPanel.SetActive(false);
    }

    public void ShowRegister()
    {
        landingPanel.SetActive(false);
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        notificationPanel.SetActive(false);
    }

    // ================= REGISTER =================
    public void RegisterUser()
    {
        if (string.IsNullOrEmpty(registerUsername.text) ||
            string.IsNullOrEmpty(registerEmail.text) ||
            string.IsNullOrEmpty(registerPassword.text) ||
            string.IsNullOrEmpty(registerConfirmPassword.text))
        {
            ShowNotification("Error", "All fields are required");
            return;
        }

        if (registerPassword.text != registerConfirmPassword.text)
        {
            ShowNotification("Error", "Passwords do not match");
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(
            registerEmail.text,
            registerPassword.text
        ).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                ShowNotification("Error", task.Exception.Message);
                return;
            }

            user = task.Result.User;
            UpdateUserProfile(registerUsername.text);
        });
    }

    void UpdateUserProfile(string username)
    {
        UserProfile profile = new UserProfile
        {
            DisplayName = username
        };

        user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                ShowNotification("Success", "Account created");
                LoadGameScene();
            }
        });
    }

    // ================= LOGIN =================
    public void LoginUser()
    {
        if (string.IsNullOrEmpty(loginEmail.text) ||
            string.IsNullOrEmpty(loginPassword.text))
        {
            ShowNotification("Error", "Email & Password required");
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(
            loginEmail.text,
            loginPassword.text
        ).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                ShowNotification("Error", task.Exception.Message);
                return;
            }

            Debug.Log("Login successful");
            LoadGameScene();
        });
    }

    // ================= SCENE =================
    void LoadGameScene()
    {
        SceneManager.LoadScene("NewCharselection"); // CHANGE if needed
    }

    // ================= NOTIFICATION =================
    void ShowNotification(string title, string message)
    {
        notif_Title.text = title;
        notif_Message.text = message;
        notificationPanel.SetActive(true);
    }

    public void CloseNotification()
    {
        notificationPanel.SetActive(false);
    }
}
