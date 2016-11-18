using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;

public class FBscript : MonoBehaviour
{

    public GameObject DialogLoggedIn;
    public GameObject DialogLoggedOut;
    public GameObject DialogUsername;
    public GameObject DialogProfilePic;

    public Text ScoresDebug;
    private List<object> scoresList = null;
    public GameObject ScoreEntryPanel;
    public GameObject ScoreScrollList;

    void Awake()
    {
        FB.Init(SetInit, OnHideUnity);
    }

    void SetInit()
    {

        if (FB.IsLoggedIn)
        {
            Debug.Log("FB is logged in");
        }
        else
        {
            Debug.Log("FB is not logged in");
        }

        DealWithFBMenus(FB.IsLoggedIn);

    }

    void OnHideUnity(bool isGameShown)
    {

        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

    }

    public void FBlogin()
    {

        List<string> permissions = new List<string>();
        permissions.Add("public_profile");

        FB.LogInWithReadPermissions(permissions, AuthCallBack);
    }

    void AuthCallBack(IResult result)
    {

        if (result.Error != null)
        {
            Debug.Log(result.Error);
        }
        else
        {
            if (FB.IsLoggedIn)
            {
                Debug.Log("FB is logged in");
            }
            else
            {
                Debug.Log("FB is not logged in");
            }

            DealWithFBMenus(FB.IsLoggedIn);
        }

    }

    void DealWithFBMenus(bool isLoggedIn)
    {

        if (isLoggedIn)
        {
            DialogLoggedIn.SetActive(true);
            DialogLoggedOut.SetActive(false);

            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);

        }
        else
        {
            DialogLoggedIn.SetActive(false);
            DialogLoggedOut.SetActive(true);
        }

    }

    void DisplayUsername(IResult result)
    {

        Text UserName = DialogUsername.GetComponent<Text>();

        if (result.Error == null)
        {

            UserName.text = "Hola, "+result.ResultDictionary["first_name"];

        }
        else
        {
            Debug.Log(result.Error);
        }

    }

    void DisplayProfilePic(IGraphResult result)
    {

        if (result.Texture != null)
        {

            Image ProfilePic = DialogProfilePic.GetComponent<Image>();

            ProfilePic.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());

        }

    }

    //get scores
    public void QueryScores()
    {
        FB.API("/app/scores?fields=score,user.limit(30)", HttpMethod.GET, ScoresCallback);
    }

    private void ScoresCallback(IResult result)
    {
        //ScoresDebug.text = ""+ result.RawResult;
        Debug.Log("Scores callback: " + result.RawResult);


        scoresList = Util.DeserializeScores(result.RawResult);

        foreach (object score in scoresList)
        {

            var entry = (Dictionary<string, object>)score;
            var user = (Dictionary<string, object>)entry["user"];

            
            ScoresDebug.text = ScoresDebug.text + "UN: " + user["name"]+" - "+entry["score"]+",";
            Debug.Log(ScoresDebug.text);
            GameObject ScorePanel;
            ScorePanel = Instantiate(ScoreEntryPanel) as GameObject;
            ScorePanel.transform.parent = ScoreScrollList.transform;

            Transform ThisScoreName = ScorePanel.transform.Find("FriendName");
            Transform ThisScoreScore = ScorePanel.transform.Find("FriendScore");
            Text ScoreName = ThisScoreName.GetComponent<Text>();
            Text ScoreScore = ThisScoreScore.GetComponent<Text>();

            ScoreName.text = user["name"].ToString();
            ScoreScore.text = entry["score"].ToString();

            Transform TheUserAvatar = ScorePanel.transform.Find("FriendAvatar");
            Image UserAvatar = TheUserAvatar.GetComponent<Image>();


            /* FB.API(Util.GetPictureURL(user["id"].ToString(), 128, 128), HttpMethod.GET, delegate (IResult pictureResult) {

                 if (pictureResult.Error != null) // if there was an error
                 {
                     Debug.Log(pictureResult.Error);
                 }
                 else // if everything was fine
                 {
                     UserAvatar.sprite = Sprite.Create(pictureResult.Texture, new Rect(0, 0, 128, 128), new Vector2(0, 0));
                 }

             });*/
        }



    }

    public void setScore()
    {
        var scoreData = new Dictionary<string, string>();
        scoreData["score"] = Random.Range(10, 200).ToString();
       /* FB.API("/me/scores", HttpMethod.POST, delegate (IResult result) {
            Debug.Log("Score submit result: " + result.RawResult);
        }, scoreData);*/
    }
    }