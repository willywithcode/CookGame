using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonCustom : MonoBehaviour
{
    [SerializeField] public Button button;
    public Button Btn => button;
    public UnityAction customeButton;
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
            customeButton?.Invoke();
        });
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
