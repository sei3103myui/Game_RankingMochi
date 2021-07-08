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

    public InputField inputName;//���O
    public WebGLNativeInputField webGLNativeInput;
    public InputField inputScore;//�X�R�A
    public Text MainScoreText;
    public Text textRanking;
    public MochiGameManager mochiGameManager;

    /// <summary>
    /// ���M�{�^������
    /// </summary>
    public void OnSendBtn()
    {
        //���M�f�[�^
        WWWForm fromData = new WWWForm();
        fromData.AddField("name", inputName.text);
        fromData.AddField("score", int.Parse(inputScore.text));

        //���s����WebRequest
        UnityWebRequest request = UnityWebRequest.Post(REQUEST_URL_ADD_DATA, fromData);

        //Basic�F�ؗp��AUTHORIZATION�w�b�_�[�t��
        string authorization = GetBasicAuth("student", "jyobi0200021");
        request.SetRequestHeader("AUTHORIZATION", authorization);

        //�ʐM�J�n
        StartWebRequest(request, () =>
        {
            Debug.Log(request.downloadHandler.text);
        }, () =>
         {
             Debug.LogError(request.error);
             Debug.LogError(request.downloadHandler.text);
         });

        //���̓t�H�[�����Z�b�g
        inputName.name = String.Empty;
        inputScore.text = String.Empty;
    }

    public void OnBtnResult()
    {
        //���M�f�[�^
        WWWForm fromData = new WWWForm();
        fromData.AddField("name", webGLNativeInput.text);
        fromData.AddField("score", MainScoreText.text);
        fromData.AddField("mode", GameConfig.DifficultyLebel.gameModeName);

        //���s����WebRequest
        UnityWebRequest request = UnityWebRequest.Post(REQUEST_URL_ADD_DATA, fromData);

        //Basic�F�ؗp��AUTHORIZATION�w�b�_�[�t��
        string authorization = GetBasicAuth("student", "jyobi0200021");
        request.SetRequestHeader("AUTHORIZATION", authorization);

        //�ʐM�J�n
        StartWebRequest(request, () =>
        {
            Debug.Log(request.downloadHandler.text);
        }, () =>
        {
            Debug.LogError(request.error);
            Debug.LogError(request.downloadHandler.text);
        });

        //���̓t�H�[�����Z�b�g
        webGLNativeInput.text = String.Empty;
        mochiGameManager.gameMode = MochiGameManager.GameMode.Ranking;
    }
   
    /// <summary>
    /// �ʐM�����̊J�n
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
        //�ʐM���I������܂őҋ@
        yield return request.SendWebRequest();

        //�R�[���o�b�N�̎��s
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
    /// �x�[�V�b�N�F�ؗp�̃p�����[�^��Ԃ�
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    string GetBasicAuth(string username, string password)
    {
        //���[�U�[ID�ƃp�X���[�h���o�C�g�z��ɕϊ�
        string auth = $"{username}:{password}";
        var b = Encoding.GetEncoding("ISO-8859-1").GetBytes(auth);

        //�o�C�g�z���Web�Ŏg�p�ł���Base64�ɕϊ�
        auth = Convert.ToBase64String(b);
        auth = $"Basic {auth}";
        return auth;
    }

    /// <summary>
    /// �����L���O�̎擾
    /// </summary>
    public void OnGetRankingBtn()
    {
        //���s����WebRequest
        UnityWebRequest request = UnityWebRequest.Get(REQUEST_URL_GET_TOP_SCORE);

        //Basic�F�ؗp��AUTHORIZATION�w�b�_�[�t��
        string authorization = GetBasicAuth(BASIC_USER_NAME, BASIC_PASSWORD);
        request.SetRequestHeader("AUTHORIZATION", authorization);

        //�ʐM�J�n
        StartWebRequest(request, () =>
        {
            Debug.Log(request.downloadHandler.text);

            //Json�̃p�X
            JsonData jsonDatas = JsonMapper.ToObject(request.downloadHandler.text);

            JsonReader jsonData;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < jsonDatas.Count; i++)
            {
                try
                {
                    jsonData = new JsonReader(jsonDatas[i].ToJson());//
                    ScoreData data = JsonMapper.ToObject<ScoreData>(jsonData);//


                    //StringBuilder�ɂ�镶����̍쐬
                    builder.Append("ID ");
                    builder.Append(data.id);
                    builder.Append(",");
                    builder.Append("�j�b�N�l�[��");
                    builder.Append(data.name);
                    builder.Append(",");
                    builder.Append("�X�R�A");
                    builder.Append(data.score);
                    builder.Append("\n");
                }
                catch(Exception e)
                {
                    //�p�[�X���s
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
        //���M�f�[�^
        WWWForm fromData = new WWWForm();
        fromData.AddField("mode",GameConfig.DifficultyLebel.gameModeName);
        //���s����WebRequest
        UnityWebRequest request = UnityWebRequest.Post(REQUEST_URL_GET_MOCHI_SCORE,fromData);

        //Basic�F�ؗp��AUTHORIZATION�w�b�_�[�t��
        string authorization = GetBasicAuth(BASIC_USER_NAME, BASIC_PASSWORD);
        request.SetRequestHeader("AUTHORIZATION", authorization);

        //�ʐM�J�n
        StartWebRequest(request, () =>
        {
            Debug.Log(request.downloadHandler.text);

            //Json�̃p�X
            JsonData jsonDatas = JsonMapper.ToObject(request.downloadHandler.text);

            JsonReader jsonData;
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < jsonDatas.Count; i++)
            {
                try
                {
                    jsonData = new JsonReader(jsonDatas[i].ToJson());//
                    ScoreData data = JsonMapper.ToObject<ScoreData>(jsonData);//


                    //StringBuilder�ɂ�镶����̍쐬
                    builder.Append("ID ");
                    builder.Append(data.id);
                    builder.Append("\n");
                    builder.Append("�j�b�N�l�[�� ");
                    builder.Append(data.name);
                    builder.Append("\n");
                    builder.Append("�X�R�A ");
                    builder.Append(data.score);
                    builder.Append("\n\n");
                }
                catch (Exception e)
                {
                    //�p�[�X���s
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
