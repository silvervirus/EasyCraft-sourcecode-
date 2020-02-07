// Decompiled with JetBrains decompiler
// Type: EasyCraft.Constructable_Construct_Patch
// Assembly: EasyCraft, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3773AFFF-02BF-495C-B57C-5121AC0BFC1B
// Assembly location: C:\Users\pred1\Desktop\EasyCraft.dll

using Harmony;
using System;

namespace EasyCraft
{
  [HarmonyPatch(typeof (Constructable), "Construct", new Type[] {})]
  internal class Constructable_Construct_Patch
  {
    private static bool Prefix(Constructable __instance, ref bool __result)
    {
      __result = Main.Construct(__instance);
      return false;
    }
  }
}
