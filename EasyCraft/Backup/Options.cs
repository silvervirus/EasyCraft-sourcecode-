// Decompiled with JetBrains decompiler
// Type: EasyCraft.Options
// Assembly: EasyCraft, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3773AFFF-02BF-495C-B57C-5121AC0BFC1B
// Assembly location: C:\Users\pred1\Desktop\EasyCraft.dll

using Harmony;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EasyCraft
{
  public class Options : MonoBehaviour
  {
    private uGUI_OptionsPanel optionsPanel;

    private void Awake()
    {
      this.optionsPanel = (uGUI_OptionsPanel) ((Component) this).GetComponent<uGUI_OptionsPanel>();
    }

    private void OnEnable()
    {
      this.AddTab();
    }

    private void OnDisable()
    {
      Main.Settings.Save();
    }

    private void AddTab()
    {
      int num1 = 0;
      IEnumerable enumerable = (IEnumerable) Traverse.Create((object) this.optionsPanel).Field("tabs").GetValue<IEnumerable>();
      if (enumerable != null)
      {
        int num2 = 0;
        foreach (object obj in enumerable)
        {
          if (((Text) ((GameObject) obj.GetType().GetField("tab").GetValue(obj))?.GetComponentInChildren<Text>()).get_text() == "Mods")
          {
            num1 = num2;
            break;
          }
          ++num2;
        }
      }
      if (num1 == 0)
        num1 = ((uGUI_TabbedControlsPanel) this.optionsPanel).AddTab("Mods");
      ((uGUI_TabbedControlsPanel) this.optionsPanel).AddHeading(num1, "EasyCraft");
      // ISSUE: method pointer
      ((uGUI_TabbedControlsPanel) this.optionsPanel).AddToggleOption(num1, "Auto Craft", Main.Settings.autoCraft, new UnityAction<bool>((object) this, __methodptr(OnAutoCraftChanged)));
      // ISSUE: method pointer
      ((uGUI_TabbedControlsPanel) this.optionsPanel).AddChoiceOption(num1, "Use Lockers At Distance", Enum.GetNames(typeof (NeighboringStorage)), (int) Main.Settings.useStorage, new UnityAction<int>((object) this, __methodptr(OnNeighboringStorageChanged)));
      // ISSUE: method pointer
      ((uGUI_TabbedControlsPanel) this.optionsPanel).AddChoiceOption(num1, "Return Surplus To", Enum.GetNames(typeof (ReturnSurplus)), (int) Main.Settings.returnSurplus, new UnityAction<int>((object) this, __methodptr(OnReturnSurplusChanged)));
    }

    private void OnAutoCraftChanged(bool value)
    {
      Main.Settings.autoCraft = value;
    }

    private void OnNeighboringStorageChanged(int index)
    {
      Main.Settings.useStorage = (NeighboringStorage) index;
    }

    private void OnReturnSurplusChanged(int index)
    {
      Main.Settings.returnSurplus = (ReturnSurplus) index;
    }

    public Options()
    {
      base.\u002Ector();
    }
  }
}
