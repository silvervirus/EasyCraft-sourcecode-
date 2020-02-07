// Decompiled with JetBrains decompiler
// Type: EasyCraft.uGUI_CraftingMenu_ActionAvailable_Patch
// Assembly: EasyCraft, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3773AFFF-02BF-495C-B57C-5121AC0BFC1B
// Assembly location: C:\Users\pred1\Desktop\EasyCraft.dll

using Harmony;
using System;

namespace EasyCraft
{
  [HarmonyPatch(typeof (uGUI_CraftingMenu), "ActionAvailable", new Type[] {typeof (uGUI_CraftNode)})]
  internal class uGUI_CraftingMenu_ActionAvailable_Patch
  {
    private static bool Prefix(
      uGUI_CraftingMenu __instance,
      ref bool __result,
      uGUI_CraftNode sender)
    {
      if (__instance.get_id() == "Centrifuge" || __instance.get_id() == "Rocket")
        return true;
      TreeAction action = sender.get_action();
      TechType techType0 = (TechType) sender.techType0;
      __result = action == 1 || action == 2 && CrafterLogic.IsCraftRecipeUnlocked(techType0) && Main.IsCraftRecipeFulfilledAdvanced(techType0);
      return false;
    }
  }
}
