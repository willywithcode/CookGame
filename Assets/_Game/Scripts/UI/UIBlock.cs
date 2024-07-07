using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIBlock : UICanvas
{
    [SerializeField] private Image block;
    public override void Setup()
    {
        base.Setup();
        block.DOFade(1, 0.3f).From(0).onComplete += () =>
        {
            block.DOFade(0, 1).SetDelay(0.5f).onComplete += () =>
            {
                this.CloseDirectly();
            };
        };
    }
}
