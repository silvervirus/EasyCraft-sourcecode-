// Decompiled with JetBrains decompiler
// Type: EasyCraft.GhostCrafter_Craft_Patch
// Assembly: EasyCraft, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3773AFFF-02BF-495C-B57C-5121AC0BFC1B
// Assembly location: C:\Users\pred1\Desktop\EasyCraft.dll

using Harmony;
using System;

namespace EasyCraft
{
  [HarmonyPatch(typeof (GhostCrafter), "Craft", new Type[] {typeof (TechType), typeof (float)})]
  internal class GhostCrafter_Craft_Patch
  {
    private static bool Prefix(GhostCrafter __instance, TechType techType, float duration)
    {
      if (!Main.IsGhostCrafterCraftTree((CraftTree.Type) __instance.craftTree))
        return true;
      Main.GhostCraft(__instance, techType, duration);
      return false;
    }
  }
}
