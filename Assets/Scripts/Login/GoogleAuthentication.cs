using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Google;
using TMPro;

public class GoogleAuthentication : MonoBehaviour
{
    private string imageURL;

    [SerializeField]
    private TMP_Text userNameTxt;

    [SerializeField]
    private GameObject loginPanel, menuPanel;

    [SerializeField]
    private Image profilePic;

    private string webClientId = "1062223557824-evipikt86dg8njemhd354r61pthettk3.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;

    private const string LoginKey = "IsLogin";
    private const string ReplayKey = "IsReplay";
    private const string UserName = "UserName";
    private const string UserPic = "UserPic";

    void Awake()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestIdToken = true
        };
    }

    private void Start()
    {
        if(PlayerPrefs.GetInt(ReplayKey) == 1 && PlayerPrefs.GetInt(LoginKey) == 1)
        {
            LoadSignIn();
        }
        else if(PlayerPrefs.GetInt(ReplayKey) == 1 && PlayerPrefs.GetInt(LoginKey) == 2)
        {
            OnSignInGuest();
        }
    }

    public void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
        OnAuthenticationFinished,TaskScheduler.Default);

        PlayerPrefs.SetInt(LoginKey, 1);
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<System.Exception> enumerator =
                    task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error =
                            (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.LogError("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    Debug.LogError("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.LogError("Canceled");
        }
        else
        {
            Debug.LogError("Welcome: " + task.Result.DisplayName + "!");

            userNameTxt.text = "" + task.Result.DisplayName;
            PlayerPrefs.SetString(UserName, "" + task.Result.DisplayName);

            imageURL = task.Result.ImageUrl.ToString();
            PlayerPrefs.SetString(UserPic, task.Result.ImageUrl.ToString());

            StartCoroutine(LoadProfilePic());

            StartCoroutine(OpenMenu());

        }
    }

    IEnumerator LoadProfilePic()
    {
        WWW www = new WWW(imageURL);
        yield return www;

        profilePic.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));


    }

    public void OnSignOut()
    {
        userNameTxt.text = "";

        imageURL = "";

        Debug.LogError("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public void LoadSignIn()
    {
        userNameTxt.text = PlayerPrefs.GetString(UserName);

        imageURL = PlayerPrefs.GetString(UserPic);

        StartCoroutine(LoadProfilePic());

    }

    public void OnSignInGuest()
    {
        userNameTxt.text = "Guest";
        PlayerPrefs.SetInt(LoginKey, 2);

        StartCoroutine(OpenMenu());
    }

    IEnumerator OpenMenu()
    {
        yield return new WaitForSeconds(0.1f);
        loginPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

}
