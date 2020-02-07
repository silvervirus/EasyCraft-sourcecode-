// Decompiled with JetBrains decompiler
// Type: EasyCraft.TooltipFactory_WriteIngredients_Patch
// Assembly: EasyCraft, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3773AFFF-02BF-495C-B57C-5121AC0BFC1B
// Assembly location: C:\Users\pred1\Desktop\EasyCraft.dll

using Harmony;
using System;
using System.Collections.Generic;

namespace EasyCraft
{
  [HarmonyPatch(typeof (TooltipFactory), "WriteIngredients", new Type[] {typeof (ITechData), typeof (List<TooltipIcon>)})]
  internal class TooltipFactory_WriteIngredients_Patch
  {
    private static bool Prefix(ITechData data, List<TooltipIcon> icons)
    {
      Main.WriteIngredients(data, icons);
      return false;
    }
  }
}
