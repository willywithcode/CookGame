using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(ButtonCustom), typeof(Button))]
public class Toggle : MonoBehaviour
{
    private bool isOn;
    [SerializeField] private Transform left;
    [SerializeField] private Transform right;
    [SerializeField] private Transform handle;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private Sprite spriteOn;
    [SerializeField] private Sprite spriteOff;
    [SerializeField] private Image bgImg;
    [SerializeField] private ButtonCustom button;
    private Tween toggleTween;
    public bool IsOn => isOn;
    public ButtonCustom Button => button;

    private void Start()
    {
        button.customButtonOnClick += () =>
        {
            DoToggle();
        };
    }

    public void Setup(bool state) 
    {
        isOn = state;
        if (isOn)
        {
            handle.localPosition = right.localPosition;
            bgImg.sprite = spriteOn;
        }
        else
        {
            handle.localPosition = left.localPosition;
            bgImg.sprite = spriteOff;
        }
    }
    public virtual bool DoToggle()
    {
        isOn = !isOn;
        toggleTween?.Kill();
        if (isOn)
        {
            toggleTween = handle.DOLocalMove(right.localPosition, duration).OnComplete(() =>
            {
                bgImg.sprite = spriteOn;
            });
        }
        else
        {
            toggleTween = handle.DOLocalMove(left.localPosition, duration).OnComplete(() =>
            {
                bgImg.sprite = spriteOff;
            });
        }
        return isOn;
    }

    
}
