// Decompiled with JetBrains decompiler
// Type: EasyCraft.QMod
// Assembly: EasyCraft, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3773AFFF-02BF-495C-B57C-5121AC0BFC1B
// Assembly location: C:\Users\pred1\Desktop\EasyCraft.dll

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
        HarmonyInstance.Create("subnautica.easycraft.mod").PatchAll(Assembly.GetExecutingAssembly());
        QMod.Patch();
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

    private static void Patch()
    {
      Console.WriteLine("[EasyCraft] Patched");
    }
  }
}
