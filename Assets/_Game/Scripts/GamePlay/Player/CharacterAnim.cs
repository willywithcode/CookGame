using System.Collections;
using System.Collections.Generic;
using Animancer;
using Animancer.Units;
using UnityEngine;

public class CharacterAnim : MonoBehaviour
{
    [SerializeField] private AnimancerComponent anim;
    [SerializeField] private AvatarMask actionMask;
    [SerializeField, Seconds] private float actionFadeDuration = AnimancerPlayable.DefaultFadeDuration;
    private AnimancerLayer baseLayer;
    private AnimancerLayer actionLayer;

    private bool canPlayActionFullBody;

    private void Awake()
    {
        baseLayer = anim.Layers[0];
        actionLayer = anim.Layers[1];
        actionLayer.SetMask(actionMask);
        actionLayer.SetDebugName("Action Layer");
    }

    public AnimancerState PlayBase(ITransition transition, bool canPlayActionFullBody = false)
    {
        this.canPlayActionFullBody = canPlayActionFullBody;
        if (canPlayActionFullBody && actionLayer.TargetWeight > 0)
        {
            return PlayActionFullBody(actionFadeDuration);
        }
        else
        {
             return baseLayer.Play(transition);
        }
    }
    public AnimancerState PlayAction(ITransition transition)
    {
        var state = actionLayer.Play(transition);
        if (canPlayActionFullBody)
        {
             return this.PlayActionFullBody(transition.FadeDuration);
        }
        return state;
    }
    public AnimancerState PlayActionFullBody(float fadeDuration)
    {
        AnimancerState upperBodyState = actionLayer.CurrentState;
        AnimancerState fullBodyClone = baseLayer.GetOrCreateState(upperBodyState, upperBodyState.Clip);
        var state = baseLayer.Play(fullBodyClone, fadeDuration);
        fullBodyClone.NormalizedTime = upperBodyState.NormalizedTime;
        return state;
    }
    public void FadeOutUpperBody()
    {
        actionLayer.StartFade(0, actionFadeDuration);
    }
}
