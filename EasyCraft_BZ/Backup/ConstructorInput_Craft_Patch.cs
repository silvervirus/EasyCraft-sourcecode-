// Decompiled with JetBrains decompiler
// Type: EasyCraft.ConstructorInput_Craft_Patch
// Assembly: EasyCraft_BZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7E51DAF-36A4-4014-9330-4A2B18F15F4A
// Assembly location: C:\Users\pred1\Desktop\EasyCraft_BZ.dll

using Harmony;
using System;

namespace EasyCraft
{
  [HarmonyPatch(typeof (ConstructorInput), "Craft", new Type[] {typeof (TechType), typeof (float)})]
  internal class ConstructorInput_Craft_Patch
  {
    private static bool Prefix(ConstructorInput __instance, TechType techType, float duration)
    {
      Main.ConstructorCraft(__instance, techType, duration);
      return false;
    }
  }
}
