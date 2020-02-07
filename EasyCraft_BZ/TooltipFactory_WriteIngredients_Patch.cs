// Decompiled with JetBrains decompiler
// Type: EasyCraft.TooltipFactory_WriteIngredients_Patch
// Assembly: EasyCraft_BZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7E51DAF-36A4-4014-9330-4A2B18F15F4A
// Assembly location: C:\Users\pred1\Desktop\EasyCraft_BZ.dll

using Harmony;
using System;
using System.Collections.Generic;

namespace EasyCraft
{
  [HarmonyPatch(typeof (TooltipFactory), "WriteIngredients", new Type[] {typeof (IList<Ingredient>), typeof (List<TooltipIcon>)})]
  internal class TooltipFactory_WriteIngredients_Patch
  {
    private static bool Prefix(IList<Ingredient> ingredients, List<TooltipIcon> icons)
    {
      Main.WriteIngredients(ingredients, icons);
      return false;
    }
  }
}
