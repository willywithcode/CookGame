using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCustom : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] public Button button;
    public Button Btn => button;
    public UnityAction customButtonOnClick;
    public UnityAction customButtonUp;
    public UnityAction customButtonDown;
    private RectTransform tf;
    public RectTransform TF
    {
        get
        {
            tf ??= this.GetComponent<RectTransform>();
            return tf;
        }
    }
    private void Start()
    {
        button?.onClick.AddListener(() =>
        {
            customButtonOnClick?.Invoke();
        });
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        customButtonDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        customButtonUp?.Invoke();
    }
#if UNITY_EDITOR

    [OnInspectorGUI]
    public void AssignButotn()
    {
        Button btn;
        if (TryGetComponent<Button>(out btn))
        {
            button = btn;
        }
    }
#endif
    
}
