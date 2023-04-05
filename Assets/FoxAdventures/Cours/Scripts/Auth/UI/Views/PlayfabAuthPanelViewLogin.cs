using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;


public class PlayfabAuthPanelViewLogin : PlayfabAuthPanelView
{
    [Header("Login View")]
    [SerializeField] protected InputField inputFieldEmail = null;
    [SerializeField] protected InputField inputFieldPassword = null;
    [SerializeField] protected Toggle toggleRemember = null;

    // Editor only
#if UNITY_EDITOR
    [Header("Editor only")]
    public bool automaticLogin = false;
#endif


    void OnEnable()
    {
        // Load previously saved data
        {
            if (PlayerPrefs.HasKey(PlayfabAuth.PlayfabAuthPlayerPrefsKeyEmail) == true)
            {
                if (this.inputFieldEmail != null)
                    this.inputFieldEmail.text = PlayerPrefs.GetString(PlayfabAuth.PlayfabAuthPlayerPrefsKeyEmail);
            }

            if (PlayerPrefs.HasKey(PlayfabAuth.PlayfabAuthPlayerPrefsKeyPassword) == true)
            {
                if (this.inputFieldPassword != null)
                    this.inputFieldPassword.text = PlayerPrefs.GetString(PlayfabAuth.PlayfabAuthPlayerPrefsKeyPassword);
            }
        }

#if UNITY_EDITOR
        if (this.automaticLogin == true)
            this.TryLogin();
#endif
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) == true)
            this.TryLogin();
    }

    public void OnLoginButtonClicked()
    {
        this.TryLogin();
    }

    private void TryLogin()
    {
        // Check setup
        if (this.inputFieldEmail == null || this.inputFieldPassword == null)
            return;

        // Remember ?
        bool remember = (this.toggleRemember != null ? this.toggleRemember.isOn : false);

        // Get input
        string email = this.inputFieldEmail.text;
        string password = this.inputFieldPassword.text;

        // Check input
        if (string.IsNullOrWhiteSpace(email) == false && string.IsNullOrWhiteSpace(password) == false)
        {
            // Save Data
            if (remember == true)
            {
                PlayerPrefs.SetString(PlayfabAuth.PlayfabAuthPlayerPrefsKeyEmail, email);
                PlayerPrefs.SetString(PlayfabAuth.PlayfabAuthPlayerPrefsKeyPassword, password);
                PlayerPrefs.Save();
            }

            // Call API
            PlayfabAuth.TryLoginWithEmail(email, password, this.OnLoginSuccess, this.OnLoginError);
        }
    }

    private void OnLoginSuccess(LoginResult result)
    {
        // Log
        Debug.Log("Successffuly authenticate!");

        // Hide auth panel
        if (this.PlayfabAuthPanel != null)
            this.PlayfabAuthPanel.HideAll();
    }

    private void OnLoginError(PlayFabError error)
    {
        // Log
        Debug.LogWarning("Authentication failed.");
        //// TODO: We could adapt here this code to determine wether the user is trying to connect with a non existing account
  
    }
}
