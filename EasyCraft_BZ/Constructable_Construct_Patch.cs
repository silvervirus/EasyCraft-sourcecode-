// Decompiled with JetBrains decompiler
// Type: EasyCraft.Constructable_Construct_Patch
// Assembly: EasyCraft_BZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7E51DAF-36A4-4014-9330-4A2B18F15F4A
// Assembly location: C:\Users\pred1\Desktop\EasyCraft_BZ.dll

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
