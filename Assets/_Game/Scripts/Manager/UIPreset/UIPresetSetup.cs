using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using System.Linq;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
public enum TypeUI
{
    Button,
    Text
}
#if UNITY_EDITOR
public class UIPresetSetup : MonoBehaviour
{
    [field: SerializeField, ReadOnly] public TypeUI PresetType { get; set; }
    [field: ShowIf("@(IsInPrefabStage() || IsNotPartPrefab())")]
    [field: SerializeField, ValueDropdown(nameof(ShowListPreset), DropdownTitle = "Select Type of Preset")] 
    public string Preset;

    private TextMeshProUGUI TextField { get; set; }
    [OnInspectorGUI]
    private void FindAndApplyPreset()
    {
        if(TryGetComponent<TextMeshProUGUI>(out TextMeshProUGUI text))
        {
            PresetType = TypeUI.Text;
            if (UIPresetGlobalConfig.Instance.GetPresetText(Preset,out var preset) )
            {
                text.font = preset.m_OverrideTextFontAsset;
                text.spriteAsset = preset.m_OverrideTextSpriteAsset;
                text.styleSheet = preset.m_OverrideTextStyleSheet;
                text.fontSize = preset.m_presetSize;
            }
        }
        else if (TryGetComponent<Button>(out Button button))
        {
            PresetType = TypeUI.Button;
            if (UIPresetGlobalConfig.Instance.GetPresetButton(Preset, out var preset))
            {
                RectTransform btnTranform = button.GetComponent<RectTransform>();
                btnTranform.sizeDelta = preset.m_PresetSize;
            }
        }
        EditorUtility.SetDirty(gameObject);
    }
    

    private IEnumerable ShowListPreset()
    {
        switch(PresetType)
        {
            case TypeUI.Button:
                return UIPresetGlobalConfig.Instance.ButtonPresets.Select(x => x.m_presetName);
            case TypeUI.Text:
                return UIPresetGlobalConfig.Instance.TextPresets.Select(x => x.m_presetName);
            default:
                throw new System.ArgumentOutOfRangeException();
        }
    }
    private bool IsInPrefabStage()
    {
        PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();
        return stage != null && stage.prefabContentsRoot.name == name && !EditorUtility.IsPersistent(gameObject);
    }
    private bool IsNotPartPrefab()
    {
        return !PrefabUtility.IsPartOfAnyPrefab(gameObject);
    }
}
#endif
