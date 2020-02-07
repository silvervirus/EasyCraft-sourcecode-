// Decompiled with JetBrains decompiler
// Type: EasyCraft.ConstructorInput_Craft_Patch
// Assembly: EasyCraft, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3773AFFF-02BF-495C-B57C-5121AC0BFC1B
// Assembly location: C:\Users\pred1\Desktop\EasyCraft.dll

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
