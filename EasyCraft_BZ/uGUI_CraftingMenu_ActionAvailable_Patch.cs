// Decompiled with JetBrains decompiler
// Type: EasyCraft.uGUI_CraftingMenu_ActionAvailable_Patch
// Assembly: EasyCraft_BZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7E51DAF-36A4-4014-9330-4A2B18F15F4A
// Assembly location: C:\Users\pred1\Desktop\EasyCraft_BZ.dll

using Harmony;
using System;
using System.Reflection;

namespace EasyCraft
{
  [HarmonyPatch]
  internal class uGUI_CraftingMenu_ActionAvailable_Patch
  {
    private static Type nodeType = typeof (uGUI_CraftingMenu).GetNestedType("Node", BindingFlags.NonPublic);
    private static FieldInfo idField = typeof (uGUI_CraftingMenu).GetField("id", BindingFlags.Instance | BindingFlags.NonPublic);
    private static FieldInfo actionField = uGUI_CraftingMenu_ActionAvailable_Patch.nodeType.GetField("action");
    private static FieldInfo techTypeField = uGUI_CraftingMenu_ActionAvailable_Patch.nodeType.GetField("techType");

    private static MethodInfo TargetMethod()
    {
      return typeof (uGUI_CraftingMenu).GetMethod("ActionAvailable", BindingFlags.Instance | BindingFlags.NonPublic);
    }

    private static bool Prefix(uGUI_CraftingMenu __instance, ref bool __result, object sender)
    {
      string str = uGUI_CraftingMenu_ActionAvailable_Patch.idField.GetValue((object) __instance) as string;
      if (str == "Centrifuge" || str == "Rocket")
        return true;
      TreeAction treeAction = (TreeAction) uGUI_CraftingMenu_ActionAvailable_Patch.actionField.GetValue(sender);
      TechType techType = (TechType) uGUI_CraftingMenu_ActionAvailable_Patch.techTypeField.GetValue(sender);
      __result = treeAction == 1 || treeAction == 2 && CrafterLogic.IsCraftRecipeUnlocked(techType) && Main.IsCraftRecipeFulfilledAdvanced(techType);
      return false;
    }
  }
}
