using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public static class Helper
{
    public static async UniTask Typing(this TextMeshProUGUI container, string text, float speed, CancellationToken cancellationToken)
    {
        container.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            container.text += text[i];
            await UniTask.Delay((int)(speed * 1000), cancellationToken: cancellationToken);
        }
    }
    public static uint ComputeDJB2Hash(string input)
    {
        uint hash = 5381;
        foreach (char c in input)
        {
            hash = ((hash << 5) + hash) + c; 
        }
        return hash;
    }
    public static async Task<string> GetJson(string spreadsheetId, string tabName)
    {
        return await SendRequest(spreadsheetId, tabName);
    }

    private static async Task<string> SendRequest(string spreadsheetId, string tabName)
    {
        string url = $"https://opensheet.elk.sh/{spreadsheetId}/{tabName}";
        UnityWebRequest www = UnityWebRequest.Get(url);

        await www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"Update Success {spreadsheetId}/{tabName}");
            return www.downloadHandler.text;
        }
        else
        {
            Debug.LogError("Error: " + www.error);
            return "";
        }
    }
    public static string NormalizeString(this string input)
    {
        string firstChar = input.Substring(0, 1).ToUpper();
        string restChars = input.Substring(1).ToLower();
        return firstChar + restChars;
    }
    private static Tween tweenIncressCoin;
    public static void TweenIncressCoin(this TextMeshProUGUI currentCoinTxt,int start, int end, float duration)
    {
        tweenIncressCoin?.Kill();
        tweenIncressCoin = DOTween.To(
            () => start, 
            x => currentCoinTxt.text = x.ToString(), end, duration)
            .OnKill(() => currentCoinTxt.text = end.ToString());
    }

}
