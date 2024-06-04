using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "UIPresetGlobalConfig", menuName = "GlobalConfigs/UIPresetGlobalConfig")]
[GlobalConfig("Assets/Resources/GlobalConfig/")]
public class UIPresetGlobalConfig : GlobalConfig<UIPresetGlobalConfig>
{
    [System.Serializable]
    public struct ButtonPreset
    {
        public string m_presetName;
        public Vector2 m_PresetSize;
    }
    [System.Serializable]
    public struct TextPreset
    {
        public string m_presetName;
        public float m_presetSize;
        [FoldoutGroup("Override")] public TMP_FontAsset m_OverrideTextFontAsset;
        [FoldoutGroup("Override")] public TMP_SpriteAsset m_OverrideTextSpriteAsset;
        [FoldoutGroup("Override")] public TMP_StyleSheet m_OverrideTextStyleSheet;
    }

    [field: Title("Text Preset")]
    [field: SerializeField] public TMP_FontAsset m_MainTextFontAsset;
    [field: SerializeField] public TMP_SpriteAsset m_MainSpriteAsset;
    [field: SerializeField] public TMP_StyleSheet m_MainTextStyleSheet;
    [field: ListDrawerSettings(CustomAddFunction = nameof(OnAddTextPreset)), SerializeField]
    public TextPreset[] TextPresets;

    [field: Title("Button Preset")]
    [field: ListDrawerSettings(CustomAddFunction = nameof(OnAddButtonPreset)), SerializeField] 
    public ButtonPreset[] ButtonPresets;

    public TextPreset OnAddTextPreset()
    {
        return new TextPreset()
        {
            m_presetName = "text preset name",
            m_presetSize = 20,
            m_OverrideTextFontAsset = m_MainTextFontAsset,
            m_OverrideTextSpriteAsset = m_MainSpriteAsset,
            m_OverrideTextStyleSheet = m_MainTextStyleSheet
        };
    }

    public ButtonPreset OnAddButtonPreset()
    {
        return new ButtonPreset()
        {
            m_presetName = "button preset name",
            m_PresetSize = new Vector2(20, 20)
        };
    }
    public bool GetPresetButton(string name, out ButtonPreset button)
    {
        button = ButtonPresets.FirstOrDefault(x => x.m_presetName == name);
        return button.m_presetName != null;
    }
    public bool GetPresetText (string name, out TextPreset text)
    {
        text = TextPresets.FirstOrDefault(x => x.m_presetName == name);
        return text.m_presetName != null;
    }
}
