// Decompiled with JetBrains decompiler
// Type: EasyCraft.QMod
// Assembly: EasyCraft_BZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7E51DAF-36A4-4014-9330-4A2B18F15F4A
// Assembly location: C:\Users\pred1\Desktop\EasyCraft_BZ.dll

using Harmony;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace EasyCraft
{
  public class QMod
  {
    public static void Load()
    {
      try
      {
        HarmonyInstance harmony = HarmonyInstance.Create("subnautica.easycraft.mod");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
        QMod.Patch(harmony);
        // ISSUE: method pointer
        SceneManager.add_sceneLoaded(new UnityAction<Scene, LoadSceneMode>((object) null, __methodptr(OnSceneLoaded)));
      }
      catch (Exception ex)
      {
        Debug.LogException(ex);
      }
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
      if (!(((Scene) ref scene).get_name() == "Main"))
        return;
      Main.Load();
    }

    private static void Patch(HarmonyInstance harmony)
    {
      Console.WriteLine("[EasyCraft] Patched");
    }
  }
}
