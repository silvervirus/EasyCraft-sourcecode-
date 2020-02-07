// Decompiled with JetBrains decompiler
// Type: EasyCraft.GhostCrafter_Craft_Patch
// Assembly: EasyCraft_BZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7E51DAF-36A4-4014-9330-4A2B18F15F4A
// Assembly location: C:\Users\pred1\Desktop\EasyCraft_BZ.dll

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
