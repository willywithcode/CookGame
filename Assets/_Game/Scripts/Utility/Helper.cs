using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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

}
