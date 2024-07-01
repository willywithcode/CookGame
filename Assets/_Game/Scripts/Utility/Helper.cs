using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

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

}
