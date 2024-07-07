using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FishingButton : ACacheMonoBehaviour
{
    [SerializeField] private TypeOfDirection typeOfDirection;
    public TypeOfDirection TypeOfDirection => typeOfDirection;
    public ButtonCustom buttonCustom;

    private void Start()
    {
        buttonCustom.customButtonDown += () =>
        {
            TF.DOScale(0.9f, 0.1f);
        };
        buttonCustom.customButtonUp += () =>
        {
            TF.DOScale(1f, 0.1f);
        };
    }
}
