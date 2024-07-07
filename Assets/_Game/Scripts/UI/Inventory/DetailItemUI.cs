using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DetailItemUI : MonoBehaviour
{
    [SerializeField] private Image contentItemImg;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private ButtonCustom throwBtn;
    [SerializeField] private ButtonCustom throwAllBtn;
    [SerializeField] private ButtonCustom splitBtn;
    [SerializeField] private ButtonCustom holdBtn;
    [SerializeField] private TextMeshProUGUI titleFuctionButton;
    private bool isButtonVisible = true;
    private Tween tween_ThrowBtn;
    private Tween tween_ThrowAllBtn;
    private Tween tween_SplitBtn;
    private Tween tween_HoldBtn;
    private List<Tuple<Vector3, Vector3>> listPos = new List<Tuple<Vector3, Vector3>>();
    private CancellationTokenSource cancellationTypingToken;
    public bool IsButtonVisible => isButtonVisible;
    public ButtonCustom SplitBtn => splitBtn;
    public void SetDetail(DataItem dataItem)
    {
        contentItemImg.gameObject.SetActive(true);
        contentItemImg.sprite = dataItem.icon;
        cancellationTypingToken?.Cancel();
        cancellationTypingToken = new CancellationTokenSource();
        _ = itemNameText.Typing(dataItem.title, 0.1f, cancellationTypingToken.Token);
        _ = descriptionText.Typing(dataItem.description, 0.03f, cancellationTypingToken.Token);
    }

    public void ResetDetail()
    {
        cancellationTypingToken?.Cancel();
        contentItemImg.gameObject.SetActive(false);
        contentItemImg.sprite = null;
        itemNameText.text = "";
        descriptionText.text = "";
    }

    public void AddEventOnclickThrowBtn(UnityAction action)
    {
        this.throwBtn.customButtonOnClick += action;
    }

    public void AddEventOnclickThrowAllBtn(UnityAction action)
    {
        this.throwAllBtn.customButtonOnClick += action;
    }

    public void AddEventOnclickSplitBtn(UnityAction action)
    {
        this.splitBtn.customButtonOnClick += action;
    }
    public void AddEventOnclickHoldBtn(UnityAction action)
    {
        this.holdBtn.customButtonOnClick += action;
    }

    

    public void SetInvisibleButton()
    {
        this.isButtonVisible = false;
        this.Setup(throwBtn);
        this.Setup(throwAllBtn);
        this.Setup(splitBtn);
        this.Setup(holdBtn);
    }
    public void DoMoveButtonsUp()
    {
        if(isButtonVisible) return;
        this.isButtonVisible = true;
        this.DoMoveUp(this.throwBtn, tween_ThrowBtn, 0);
        this.DoMoveUp(this.throwAllBtn, tween_ThrowAllBtn, 1);
        this.DoMoveUp(this.splitBtn, tween_SplitBtn, 2);
        this.DoMoveUp(this.holdBtn, tween_HoldBtn, 3);
    }

    public void SetTextTitleFunctionButton(ItemType itemType)
    {
        if(itemType == ItemType.Food)
            this.SetTextTitleFunctionButton(Constant.USE_STRING);
        else
            this.SetTextTitleFunctionButton(Constant.HOLD_STRING);
    }
    public void SetTextTitleFunctionButton(string title)
    {
        this.titleFuctionButton.text = title;
    }
    public void DoMoveUp(ButtonCustom button, Tween tween, int index)
    {
        tween.Kill();
        tween = button.TF.DOMoveY(listPos[index].Item1.y, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            button.button.enabled = true;
        });
    }
    public void Setup(ButtonCustom button)
    {
        if(listPos.Count >=4 ) return;
        listPos.Add(new Tuple<Vector3, Vector3>(button.TF.position, new Vector3(button.TF.position.x, button.TF.position.y - 600, button.TF.position.z)));
        button.TF.position = new Vector3(button.TF.position.x, button.TF.position.y - 600, button.TF.position.z);
        button.button.enabled = false;
    }
    public void DoMoveDown(ButtonCustom button, Tween tween, int index)
    {
        tween.Kill();
        tween = button.TF.DOMoveY(listPos[index].Item2.y, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            button.button.enabled = false;
        });
    }
    public void DoMoveButtonsDown()
    {
        if(!isButtonVisible) return;
        this.isButtonVisible = false;
        this.DoMoveDown(this.throwBtn, tween_ThrowBtn, 0);
        this.DoMoveDown(this.throwAllBtn, tween_ThrowAllBtn, 1);
        this.DoMoveDown(this.splitBtn, tween_SplitBtn, 2);
        this.DoMoveDown(this.holdBtn, tween_HoldBtn, 3);
    }
}
