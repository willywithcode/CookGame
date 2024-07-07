using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : UICanvas
{
    [SerializeField] private Slider sliderLoading;
    [SerializeField] private TextMeshProUGUI txtLoading;
    private int portionRnd;
    private int count;
    private int currentValue;
    private string loadingText = "Loading...";

    public override void Setup()
    {
        base.Setup();
        sliderLoading.value = 0;
        txtLoading.text = loadingText + "0%";
        currentValue = 0;
        portionRnd = Random.Range(3, 5);
        Loading();
    }

    public void Loading()
    {
        int end = 0;
        if (count == portionRnd)
        {
            CloseDirectly();
            GameManager.ChangeState(GameState.Gameplay);
            UIManager.Instance.OpenUI<UITutorial>();
        }
        else if(count == portionRnd -1)
        {
            end = 100;   
        }
        else
        {
            end = currentValue + Random.Range(0, 30);
        }
        int start = currentValue;
        DOTween.To(() => start, value =>
        {
            sliderLoading.value = (float)value / (float)100;
            txtLoading.text = loadingText + ((int)(sliderLoading.value * 100)).ToString() + "%";
        }, end, 1.5f).OnComplete(() =>
        {
            count++;
            currentValue = end;
            Loading();
        });
    }
}
