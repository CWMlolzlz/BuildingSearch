using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using ColossalFramework;
using ColossalFramework.UI;
using ColossalFramework.DataBinding;
using System.Reflection;


public class SearchPanel : GeneratedScrollPanel {

    static readonly BindingFlags bf = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
    private static MethodInfo m_createAssetItem = typeof(GeneratedScrollPanel).GetMethod("CreateAssetItem", bf);
    private static FieldInfo m_ObjectIndexInfo = typeof(GeneratedScrollPanel).GetField("m_ObjectIndex", bf);

    private static readonly PrefabInfo[] m_allPrefabs;
    static SearchPanel() {
        m_allPrefabs = Resources.FindObjectsOfTypeAll<PrefabInfo>().Where(prefabInfo =>
            !prefabInfo.GetType().Equals(typeof(NetInfo)) &&
            !prefabInfo.GetType().Equals(typeof(VehicleInfo)) &&
            !prefabInfo.GetType().Equals(typeof(CitizenInfo)) &&
            !prefabInfo.GetType().Equals(typeof(BuildingInfoSub)) &&
            !prefabInfo.GetType().Equals(typeof(TransportInfo))
        ).ToArray();
        Array.Sort(m_allPrefabs, new Comparison<PrefabInfo>((x, y) => { return x.name.CompareTo(y.name); }));
    }
    private string searchText = "";

    public void CreateAssetItem(PrefabInfo info) {
        m_createAssetItem.Invoke(this, new object[] { info });
    }

    public override ItemClass.Service service {
        get { return ItemClass.Service.None; }
    }

    protected override void OnButtonClicked(UIComponent comp) {
        Debug.Print("Button Clicked");
        PrefabInfo prefabInfo = comp.objectUserData as PrefabInfo;
        if (PropTool(prefabInfo))
            return;
        NetTool(prefabInfo);
        BuildingTool(prefabInfo);
    }

    private bool NetTool(PrefabInfo info) {
        NetInfo netInfo = info as NetInfo;
        if (netInfo == null) {
            NetTool netTool = ToolsModifierControl.SetTool<NetTool>();
            if (netTool == null) {
                netTool.m_prefab = netInfo;
                return true;
            }
        }
        return false;
    }

    private bool PropTool(PrefabInfo info) {
        PropInfo propInfo = info as PropInfo;
        if (propInfo != null) {
            PropTool propTool = ToolsModifierControl.SetTool<PropTool>();
            if (propTool != null) {
                propTool.m_prefab = propInfo;
                return true;
            }
        }
        return false;
    }

    private bool BuildingTool(PrefabInfo info) {
        BuildingInfo buildingInfo = info as BuildingInfo;
        if (buildingInfo == null)
            return false;
        BuildingTool buildingTool = ToolsModifierControl.SetTool<BuildingTool>();
        if (buildingTool == null)
            return false;
        buildingTool.m_prefab = buildingInfo;
        buildingTool.m_relocate = 0;
        return true;
    }

    public int Search(string text) {
        this.searchText = text;
        this.RefreshPanel(); 
        return this.childComponents.Count;
    } 


    static FieldInfo m_ScrollablePanelInfo = typeof(GeneratedScrollPanel).GetField("m_ScrollablePanel", bf);
    public override void CleanPanel() {
        Debug.Print("Cleaning Panel");
        UIScrollablePanel scrollPanel = (UIScrollablePanel)m_ScrollablePanelInfo.GetValue(this);
        UIButton[] buttons = scrollPanel.GetComponentsInChildren<UIButton>();
        Debug.Print("Buttons.Length = " + buttons.Length);
        foreach (UIButton button in buttons)
            GameObject.DestroyImmediate(button.gameObject);
        Debug.Print("Done Cleaning");
    }

    public override void RefreshPanel() {
        Debug.Print("Refreshing Panel"); 
        this.CleanPanel();
        m_ObjectIndexInfo.SetValue(this, 0);
        if (string.IsNullOrEmpty(searchText))
            return;
        var prefabs = m_allPrefabs.Where(info => info.name.ToLower().Contains(searchText)).ToList();

        foreach (PrefabInfo info in prefabs)
            this.CreateAssetItem(info);

        Debug.Print(prefabs.Count);
    }

}