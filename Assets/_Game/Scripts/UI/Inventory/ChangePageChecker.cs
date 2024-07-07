using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum TypeChecker
{
    BackPageChecker,
    NextPageChecker
}
public class ChangePageChecker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideIf("@(typeChecker == TypeChecker.NextPageChecker)")]
    public UnityEvent onBackPage;
    [HideIf("@(typeChecker == TypeChecker.BackPageChecker)")]
    public UnityEvent onNextPage;
    [SerializeField] private TypeChecker typeChecker;
    [SerializeField] private float timeToChangePage = 1;
    private bool isStay = false;
    private CancellationTokenSource checkCancellationTokenSource = new CancellationTokenSource();
    public void OnPointerEnter(PointerEventData eventData)
    {
        this.isStay = true;
        if(!UIManager.Instance.GetUI<UIInventory>().IsDraging()) return;
        _ = CheckChangePage();
    }

    private void OnDisable()
    {
        this.CancelChangePage();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(typeChecker == TypeChecker.BackPageChecker)
        {
            if (eventData.position.x < 
                RectTransformUtility.WorldToScreenPoint(Camera.main, this.GetComponent<RectTransform>().position).x)
            {
                this.CancelChangePage();
            }
        }
        else
        {
            if (eventData.position.x >
                RectTransformUtility.WorldToScreenPoint(Camera.main, this.GetComponent<RectTransform>().position).x)
            {
                this.CancelChangePage();
            }
        }
        
    }
    private async UniTask CheckChangePage()
    {
        await UniTask.WaitForSeconds(timeToChangePage, 
            cancellationToken: checkCancellationTokenSource.Token);
        if(!isStay) return;
        if(typeChecker == TypeChecker.BackPageChecker)
        {
            onBackPage?.Invoke();
            await CheckChangePage();
        }
        else
        {
            onNextPage?.Invoke();
            await CheckChangePage();
        }
    }

    private void CancelChangePage()
    {
        if (isStay)
        {
            checkCancellationTokenSource?.Cancel();
            checkCancellationTokenSource = new CancellationTokenSource();
        }
        this.isStay = false;
    }
}
