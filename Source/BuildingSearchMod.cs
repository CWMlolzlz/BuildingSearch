using System;
using System.Reflection;
using ICities;
using UnityEngine;
using ColossalFramework.UI;

namespace BuildingSearch {
    public class BuildingSearchMod : ILoadingExtension {
        static bool loaded = false;
        UITextureAtlas m_buttonAtlas;
        private static readonly string[] spriteNames = new string[]{
            "ToolbarIconSearch",
            "ToolbarIconSearchDisabled",
            "ToolbarIconSearchFocused",
            "ToolbarIconSearchPressed",
            "ToolbarIconSearchHovered"
        };

        public void OnCreated(ILoading loading) {
            Debug.Print("BuildingSearch Created 212124 23423 423");
            Init();
        }
        public void OnReleased() {
            loaded = false;
            Debug.Print("BuildingSearch Released");
        }

        public void OnLevelLoaded(LoadMode mode) {
            Debug.Print("BuildingSearch LevelLoaded");
            Init();
        }

        private MethodInfo GetSpawnEntryMethod(Type t) {
            MethodInfo[] methods = t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            foreach (MethodInfo mi in methods) {
                if (mi.Name.Equals("SpawnSubEntry") && mi.GetParameters().Length == 6) {
                    return mi;
                }
            }
            return null;
        }

        private void Init() {
            if (loaded) {
                Debug.Print("Already Loaded");
                return;
            }
            loaded = false;

            this.m_buttonAtlas = TextureLoader.CreateTextureAtlas("ToolbarIconSearchAtlas", spriteNames, "BuildingSearch.Source.Images.");

            //Debug.Print (typeof(SearchGroupPanel).Name);
            //Debug.Print (System.Type.GetType (typeof(ZoningPanel).Name)); 

            //DestroyOld(UISearchBox.NAME);
            //DestroyOld("UISearchButton");
            DestroyOld("SearchDefaultPanel"); 
            DestroyOld("SearchPanel");
            DestroyOld("Search"); 
            //DestroyOld("FindContainer");

            MainToolbar toolbar = ToolsModifierControl.mainToolbar;
            UITabstrip tabstrip = (UITabstrip)toolbar.component;
            UITabContainer tabcontainer = tabstrip.tabContainer;
            //return;
            UIButton button = this.SpawnSubEntry(toolbar, tabstrip, "Search", "MAIN_TOOL", "Unlock", "ToolbarIcon", true);
            button.atlas = m_buttonAtlas;
            button.normalFgSprite = "ToolbarIconSearch"; 
            button.focusedFgSprite = button.normalFgSprite + "Focused";
            button.hoveredFgSprite = button.normalFgSprite + "Hovered";
            button.pressedFgSprite = button.normalFgSprite + "Pressed";
            button.disabledFgSprite = button.normalFgSprite + "Disabled";
            Debug.Print("Successfully added button");
            var searchPanel = tabcontainer.Find<UIPanel>("SearchPanel").Find<UITabContainer>("GTSContainer").Find<UIPanel>("SearchDefaultPanel").GetComponent<SearchPanel>();
            Debug.Print(searchPanel, typeof(SearchPanel), searchPanel.Equals(typeof(SearchPanel)), searchPanel.Equals(typeof(GeneratedScrollPanel)));
            Debug.Print(typeof(SearchPanel) == Type.GetType("SearchPanel"));
            UISearchBox.CreateUISearch(searchPanel);

            //UISearchBox.CreateUISearch(searchPanel);
        }

        static BindingFlags bfAll = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        private int GetObjectIndex(MainToolbar tb) {
            FieldInfo info = typeof(MainToolbar).GetField("m_ObjectIndex", bfAll);
            return (int)info.GetValue(tb);
        }

        private void IncObjectIndex(MainToolbar tb) {
            FieldInfo info = typeof(MainToolbar).GetField("m_ObjectIndex", bfAll);
            info.SetValue(tb, (int)info.GetValue(tb) + 1);
        }

        private UIComponent GetOptionsBar(MainToolbar tb) {
            FieldInfo info = typeof(MainToolbar).GetField("m_OptionsBar", bfAll);
            return (UIComponent)info.GetValue(tb);
        }

        private UITextureAtlas GetDefaultInfoTooltipAtlas(MainToolbar tb) {
            FieldInfo info = typeof(MainToolbar).GetField("m_DefaultInfoTooltipAtlas", bfAll);
            return (UITextureAtlas)info.GetValue(tb);
        }

        protected UIButton SpawnSubEntry(MainToolbar tb, UITabstrip strip, string name, string localeID, string unlockText, string spriteBase, bool enabled){
            System.Type type = System.Type.GetType(name + "Group" + "Panel");
            type = typeof(SearchGroupPanel);
            if (type != null && !type.IsSubclassOf(typeof(GeneratedGroupPanel)))
                type = (System.Type)null;
            if (type == null)
                return (UIButton)null;
            UIButton button;
            GameObject asGameObject1 = UITemplateManager.GetAsGameObject("MainToolbarButtonTemplate");
            GameObject asGameObject2 = UITemplateManager.GetAsGameObject("ScrollableSubPanelTemplate");
            button = strip.AddTab(name, asGameObject1, asGameObject2, type) as UIButton;
                    
            button.isEnabled = enabled;
            //button.gameObject.GetComponent<TutorialUITag>().tutorialTag = name;
            //GeneratedGroupPanel generatedGroupPanel = strip.tabContainer.Find<UIPanel>("SearchPanel").GetComponent<GeneratedGroupPanel>() as GeneratedGroupPanel;
            SearchGroupPanel generatedGroupPanel = strip.GetComponentInContainer(button, type) as SearchGroupPanel;  
           
            generatedGroupPanel.enabled = true;           
            if ((UnityEngine.Object)generatedGroupPanel != (UnityEngine.Object)null) {
                generatedGroupPanel.component.isInteractive = true;
                generatedGroupPanel.m_OptionsBar = this.GetOptionsBar(tb);
                generatedGroupPanel.m_DefaultInfoTooltipAtlas = this.GetDefaultInfoTooltipAtlas(tb);
                if (enabled) {
                    generatedGroupPanel.RefreshPanel();
                }
            }
            /*
            button.normalBgSprite = this.GetBackgroundSprite(button, spriteBase, name, "Normal");
            button.focusedBgSprite = this.GetBackgroundSprite(button, spriteBase, name, "Focused");
            button.hoveredBgSprite = this.GetBackgroundSprite(button, spriteBase, name, "Hovered");
            button.pressedBgSprite = this.GetBackgroundSprite(button, spriteBase, name, "Pressed");
            button.disabledBgSprite = this.GetBackgroundSprite(button, spriteBase, name, "Disabled");*/

            this.IncObjectIndex(tb); 

            return button;
        }

        public void OnLevelUnloading() {
            loaded = false;
            Debug.Print("BuildingSearch LevelUnloaded");
            //GameObject.Destroy(m_search.gameObject);
        }

        private void DestroyOld(string name) {
            while (true) {
                try {
                    GameObject.DestroyImmediate(GameObject.Find(name).gameObject);
                    Debug.Print("Removed " + name);
                } catch {
                    break; 
                }
            }
        }
    }
}