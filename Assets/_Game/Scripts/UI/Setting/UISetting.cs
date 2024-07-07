using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : UICanvas
{
    [SerializeField] private Toggle toggleMusic;
    [SerializeField] private Toggle toggleSound;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Transform settingPanel;
    private bool isFirst = false;
    public override void Setup()
    {
        base.Setup();
        toggleMusic.Setup(SaveGameManager.Instance.IsSound);
        toggleSound.Setup(SaveGameManager.Instance.IsFXSound);
        canvasGroup.interactable = false;
        settingPanel.DOScale(1, 0.5f).From(0).SetEase(Ease.InOutBounce).onComplete += () =>
        {
            canvasGroup.interactable = true;
        };
        if (!isFirst)
        {
            isFirst = true;
            toggleMusic.Button.customButtonOnClick += () =>
            {
                SoundManager.Instance.MuteMusic(!toggleMusic.IsOn);
                SaveGameManager.Instance.IsSound = !toggleMusic.IsOn;
            };
            toggleSound.Button.customButtonOnClick += () =>
            {
                SoundManager.Instance.Mute(!toggleSound.IsOn);
                SaveGameManager.Instance.IsFXSound = !toggleSound.IsOn;
            };
        }
    }

    public void OnSliderMusicChange(Slider slider)
    {
        SoundManager.Instance.SetVolumMusic(slider.value);
    }
    public void OnSliderSoundChange(Slider slider)
    {
        SoundManager.Instance.SetVolumFX(slider.value);
    }
    public void TurnOffSetting()
    {
        canvasGroup.interactable = false;
        settingPanel.DOScale(0, 0.5f).SetEase(Ease.InOutBack).onComplete += () =>
        {
            this.CloseDirectly();
        };
    }
}
