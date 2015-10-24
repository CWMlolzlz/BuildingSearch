using System;
using UnityEngine;
using ColossalFramework.UI;


public class UISearchBox : UIPanel {
    public static readonly string NAME = "UISearchBox";
    public static readonly float WIDTH = 320f;
    public static readonly float HEIGHT = 35f;

    private UITextField m_textField;
    private UILabel m_searchLabel;
    private UILabel m_countLabel;
    SearchPanel m_targetPanel;

    public static UISearchBox CreateUISearch(SearchPanel panel) {
        UIComponent parent = panel.component;

        UISearchBox searchBox = parent.AddUIComponent<UISearchBox>();
        searchBox.m_targetPanel = panel;

        return searchBox;
    }

    public override void Start() {
        base.Start();
        //Debug.Print ("Starting UISearch");
        this.name = NAME;

        this.size = new Vector2(WIDTH, HEIGHT);
        this.backgroundSprite = "GenericTabDisabled";
        this.relativePosition = new Vector2(parent.width - width, -height);
        //this.BringToFront();
        //this.Show ();

        m_countLabel = this.AddUIComponent<UILabel>();
        m_countLabel.autoSize = false;
        m_countLabel.backgroundSprite = "GenericTabDisabled";
        m_countLabel.text = "Items";
        m_countLabel.textScale = 0.75f;
        m_countLabel.padding = new RectOffset(5, 5, 5, 0);
        m_countLabel.size = new Vector2(65, 18);
        m_countLabel.relativePosition = new Vector2(this.width - m_countLabel.width, -m_countLabel.height);
        m_countLabel.textAlignment = UIHorizontalAlignment.Right;
        m_countLabel.verticalAlignment = UIVerticalAlignment.Middle;
        m_countLabel.Hide();

        m_searchLabel = this.AddUIComponent<UILabel>();
        m_searchLabel.text = "Search:";
        m_searchLabel.autoSize = false;
        m_searchLabel.size = new Vector2(60, height);
        m_searchLabel.padding = new RectOffset(5, 0, 4, 0);
        m_searchLabel.relativePosition = new Vector2(0, this.height / 2 - m_searchLabel.height / 2);
        m_searchLabel.textAlignment = UIHorizontalAlignment.Center;
        m_searchLabel.verticalAlignment = UIVerticalAlignment.Middle;

        m_textField = this.AddUIComponent<UITextField>();
        m_textField.eventEnterFocus += (component, eventParam) => { Debug.Print("Hello"); };
        m_textField.builtinKeyNavigation = true;
        m_textField.isInteractive = true;
        m_textField.canFocus = true;
        m_textField.bottomColor = Color.red;

        m_textField.normalBgSprite = "TextFieldPanel";
        m_textField.disabledBgSprite = "TextFieldPanelDisabled";
        m_textField.focusedBgSprite = m_textField.normalBgSprite + "Focused";
        m_textField.hoveredBgSprite = m_textField.normalBgSprite + "Hovered";

        m_textField.size = new Vector2(this.width - m_searchLabel.width - 12, this.height - 10);
        m_textField.relativePosition = new Vector2(8 + m_searchLabel.width, this.height / 2 - m_textField.height / 2);
        m_textField.horizontalAlignment = UIHorizontalAlignment.Left;
        m_textField.verticalAlignment = UIVerticalAlignment.Middle;
        m_textField.padding = new RectOffset(5, 0, 4, 0);
        //m_textField.submitOnFocusLost = false;
        m_textField.selectOnFocus = true;
        //Debug.Print ("Finsihed UISearch");

        this.eventKeyPress += this.SearchConfirm;
        this.m_textField.eventTextSubmitted += this.SearchConfirm;
    }

    public void SetTargetPanel(SearchPanel target) {
        m_targetPanel = target;
        target.component.AttachUIComponent(this.gameObject);
    }

    private void SearchConfirm(UIComponent component, UIKeyEventParameter eventParam) {
        if (eventParam.keycode == KeyCode.Return) {
            Debug.Print("Search Confirmed");
            SearchConfirm(null, this.m_textField.text);
            eventParam.Use();
        }
    }

    private void SearchConfirm(UIComponent component, string text) {
        int itemsFound = m_targetPanel.Search(text);
        if (itemsFound == 0) {
            m_countLabel.Show();
            m_countLabel.text = "No Items";
        } else {
            m_countLabel.Show();
            m_countLabel.text = itemsFound + " Items";
        }
    }


}


