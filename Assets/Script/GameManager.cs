using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Net;
using LitJson;
using System.Text;

public class ScoreData
{
    public int id;
    public string name;
    public int score;
}

public class GameManager : MonoBehaviour
{
    private const string REQUEST_URL_BASE = "https://web-network.sakura.ne.jp/games2020/4193321/unity/ranking-game/api";
    

    public string REQUEST_URL_ADD_DATA => $"{REQUEST_URL_BASE}/addscore.php";
    private string REQUEST_URL_GET_TOP_SCORE => $"{REQUEST_URL_BASE}/get_top_score.php";
    private string REQUEST_URL_GET_MOCHI_SCORE => $"{REQUEST_URL_BASE}/get_mochi_score.php";

    private string BASIC_USER_NAME = "student";

    private string BASIC_PASSWORD = "jyobi0200021";

    public InputField inputName;//名前
    public WebGLNativeInputField webGLNativeInput;
    public InputField inputScore;//スコア
    public Text MainScoreText;
    public Text textRanking;
    public MochiGameManager mochiGameManager;

    /// <summary>
    /// 送信ボタン処理
    /// </summary>
    public void OnSendBtn()
    {
        //送信データ
        WWWForm fromData = new WWWForm();
        fromData.AddField("name", inputName.text);
        fromData.AddField("score", int.Parse(inputScore.text));

        //実行するWebRequest
        UnityWebRequest request = UnityWebRequest.Post(REQUEST_URL_ADD_DATA, fromData);

        //Basic認証用のAUTHORIZATIONヘッダー付加
        string authorization = GetBasicAuth("student", "jyobi0200021");
        request.SetRequestHeader("AUTHORIZATION", authorization);

        //通信開始
        StartWebRequest(request, () =>
        {
            Debug.Log(request.downloadHandler.text);
        }, () =>
         {
             Debug.LogError(request.error);
             Debug.LogError(request.downloadHandler.text);
         });

        //入力フォームリセット
        inputName.name = String.Empty;
        inputScore.text = String.Empty;
    }

    public void OnBtnResult()
    {
        //送信データ
        WWWForm fromData = new WWWForm();
        fromData.AddField("name", webGLNativeInput.text);
        fromData.AddField("score", MainScoreText.text);
        fromData.AddField("mode", GameConfig.DifficultyLebel.gameModeName);

        //実行するWebRequest
        UnityWebRequest request = UnityWebRequest.Post(REQUEST_URL_ADD_DATA, fromData);

        //Basic認証用のAUTHORIZATIONヘッダー付加
        string authorization = GetBasicAuth("student", "jyobi0200021");
        request.SetRequestHeader("AUTHORIZATION", authorization);

        //通信開始
        StartWebRequest(request, () =>
        {
            Debug.Log(request.downloadHandler.text);
        }, () =>
        {
            Debug.LogError(request.error);
            Debug.LogError(request.downloadHandler.text);
        });

        //入力フォームリセット
        webGLNativeInput.text = String.Empty;
        mochiGameManager.gameMode = MochiGameManager.GameMode.Ranking;
    }
   
    /// <summary>
    /// 通信処理の開始
    /// </summary>
    /// <param name="request"></param>
    /// <param name="successCallback"></param>
    /// <param name="errorCallback"></param>
    public void StartWebRequest(UnityWebRequest request, Action successCallback, Action errorCallback)
    {
        StartCoroutine(WebRequest(request, successCallback, errorCallback));
    }

    public IEnumerator WebRequest(UnityWebRequest request, Action successCallback, Action errorCallback)
    {
        //通信が終了するまで待機
        yield return request.SendWebRequest();

        //コールバックの実行
        if(request.result == UnityWebRequest.Result.ProtocolError ||
            request.result == UnityWebRequest.Result.ConnectionError)
        {
            errorCallback?.Invoke();
        }
        else
        {
            successCallback?.Invoke();
        }
    }

    /// <summary>
    /// ベーシック認証用のパラメータを返す
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    string GetBasicAuth(string username, string password)
    {
        //ユーザーIDとパスワードをバイト配列に変換
        string auth = $"{username}:{password}";
        var b = Encoding.GetEncoding("ISO-8859-1").GetBytes(auth);

        //バイト配列をWebで使用できるBase64に変換
        auth = Convert.ToBase64String(b);
        auth = $"Basic {auth}";
        return auth;
    }

    /// <summary>
    /// ランキングの取得
    /// </summary>
    public void OnGetRankingBtn()
    {
        //実行するWebRequest
        UnityWebRequest request = UnityWebRequest.Get(REQUEST_URL_GET_TOP_SCORE);

        //Basic認証用のAUTHORIZATIONヘッダー付加
        string authorization = GetBasicAuth(BASIC_USER_NAME, BASIC_PASSWORD);
        request.SetRequestHeader("AUTHORIZATION", authorization);

        //通信開始
        StartWebRequest(request, () =>
        {
            Debug.Log(request.downloadHandler.text);

            //Jsonのパス
            JsonData jsonDatas = JsonMapper.ToObject(request.downloadHandler.text);

            JsonReader jsonData;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < jsonDatas.Count; i++)
            {
                try
                {
                    jsonData = new JsonReader(jsonDatas[i].ToJson());//
                    ScoreData data = JsonMapper.ToObject<ScoreData>(jsonData);//


                    //StringBuilderによる文字列の作成
                    builder.Append("ID ");
                    builder.Append(data.id);
                    builder.Append(",");
                    builder.Append("ニックネーム");
                    builder.Append(data.name);
                    builder.Append(",");
                    builder.Append("スコア");
                    builder.Append(data.score);
                    builder.Append("\n");
                }
                catch(Exception e)
                {
                    //パース失敗
                    builder.Append($"Error Data");
                    builder.Append("\n");
                }
            }
            textRanking.text = builder.ToString();
        }, () =>
         {
             Debug.LogError(request.error);
             Debug.LogError(request.downloadHandler.text);
         });

    }

    public void OnGetRankingMode()
    {
        //送信データ
        WWWForm fromData = new WWWForm();
        fromData.AddField("mode",GameConfig.DifficultyLebel.gameModeName);
        //実行するWebRequest
        UnityWebRequest request = UnityWebRequest.Post(REQUEST_URL_GET_MOCHI_SCORE,fromData);

        //Basic認証用のAUTHORIZATIONヘッダー付加
        string authorization = GetBasicAuth(BASIC_USER_NAME, BASIC_PASSWORD);
        request.SetRequestHeader("AUTHORIZATION", authorization);

        //通信開始
        StartWebRequest(request, () =>
        {
            Debug.Log(request.downloadHandler.text);

            //Jsonのパス
            JsonData jsonDatas = JsonMapper.ToObject(request.downloadHandler.text);

            JsonReader jsonData;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < jsonDatas.Count; i++)
            {
                try
                {
                    jsonData = new JsonReader(jsonDatas[i].ToJson());//
                    ScoreData data = JsonMapper.ToObject<ScoreData>(jsonData);//


                    //StringBuilderによる文字列の作成
                    builder.Append("ID ");
                    builder.Append(data.id);
                    builder.Append("\n");
                    builder.Append("ニックネーム ");
                    builder.Append(data.name);
                    builder.Append("\n");
                    builder.Append("スコア ");
                    builder.Append(data.score);
                    builder.Append("\n\n");
                }
                catch (Exception e)
                {
                    //パース失敗
                    builder.Append($"Error Data");
                    builder.Append("\n");
                }
            }
            textRanking.text = builder.ToString();
        }, () =>
        {
            Debug.LogError(request.error);
            Debug.LogError(request.downloadHandler.text);
        });
    }
}
