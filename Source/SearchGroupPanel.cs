using System;
using System.Reflection;
using ColossalFramework.UI;
using UnityEngine;

public class SearchGroupPanel : GeneratedGroupPanel {

    public override ItemClass.Service service {
        get {
            return ItemClass.Service.None;
        }
    }

    public override string serviceName {
        get {
            return (string)"Search";
        }
    }

    protected override bool IsServiceValid(PrefabInfo info) {
        return true; 
    }

    static BindingFlags bfAll = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
    private int GetObjectIndex(GeneratedGroupPanel gp) {
        FieldInfo info = typeof(GeneratedGroupPanel).GetField("m_ObjectIndex", bfAll);
        return (int)info.GetValue(gp);
    }

    private void IncObjectIndex(GeneratedGroupPanel gp) {
        FieldInfo info = typeof(GeneratedGroupPanel).GetField("m_ObjectIndex", bfAll);
        info.SetValue(gp, (int)info.GetValue(gp) + 1);
    }

    private UIButton OwnSpawnButtonEntry(UITabstrip strip, string name, string category, bool isDefaultCategory, string localeID, string unlockText, string spriteBase, bool enabled, bool forceFillContainer) {
        Debug.Print("SpawnButtonEntry");
        System.Type type = System.Type.GetType(name + "Panel");
        type = typeof(SearchPanel);
        if (type != null && !type.IsSubclassOf(typeof(GeneratedScrollPanel)))
            type = (System.Type)null;
        UIButton uiButton;
        if (strip.childCount > GetObjectIndex(this)) {
            uiButton = strip.components[GetObjectIndex(this)] as UIButton;
        } else {
            GameObject asGameObject1 = UITemplateManager.GetAsGameObject("SubbarButtonTemplate");
            GameObject asGameObject2 = UITemplateManager.GetAsGameObject("SubbarPanelTemplate");
            uiButton = strip.AddTab(category, asGameObject1, asGameObject2, type) as UIButton;
        }
        uiButton.isEnabled = enabled;
        uiButton.gameObject.GetComponent<TutorialUITag>().tutorialTag = category;
        SearchPanel generatedScrollPanel = strip.GetComponentInContainer(uiButton, type) as SearchPanel;
        Debug.Print(generatedScrollPanel);
        
        if ((UnityEngine.Object)generatedScrollPanel != (UnityEngine.Object)null) {
            generatedScrollPanel.component.isInteractive = true;
            generatedScrollPanel.m_OptionsBar = this.m_OptionsBar;
            generatedScrollPanel.m_DefaultInfoTooltipAtlas = this.m_DefaultInfoTooltipAtlas;
            if (forceFillContainer || enabled) {
                generatedScrollPanel.category = !isDefaultCategory ? category : string.Empty;
                generatedScrollPanel.RefreshPanel();
            }
        }
        string str = spriteBase + category;
        uiButton.normalFgSprite = str;
        uiButton.focusedFgSprite = str + "Focused";
        uiButton.hoveredFgSprite = str + "Hovered";
        uiButton.pressedFgSprite = str + "Pressed";
        uiButton.disabledFgSprite = str + "Disabled";
        //if (!string.IsNullOrEmpty(localeID) && !string.IsNullOrEmpty(unlockText))
        //	uiButton.tooltip = Locale.Get(localeID, category) + " - " + unlockText;
        //else if (!string.IsNullOrEmpty(localeID))
        //	uiButton.tooltip = Locale.Get(localeID, category);
        IncObjectIndex(this); 
        return uiButton;
    }

    protected override bool CustomRefreshPanel() {
        Debug.Print("CustomRefreshPanel");
        //this.DefaultGroup(this.serviceName);
        this.OwnSpawnButtonEntry(this.m_Strip, this.serviceName, this.serviceName + "Default", true, (string)null, (string)null, "SubBar", true, false);

        return true;
    }
}