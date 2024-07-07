using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UITutorial : UICanvas
{
    [SerializeField] private ButtonCustom buttonCustom;
    public override void Setup()
    {
        base.Setup();
        buttonCustom.gameObject.SetActive(false);
        DOVirtual.DelayedCall(15, () => buttonCustom.gameObject.SetActive(true));
    }
    public void TurnOffTutorial()
    {
        this.CloseDirectly();
        UIManager.Instance.OpenUI<UIGamePlay>().PopUpText("Well done! You have completed the tutorial!");
    }
}
