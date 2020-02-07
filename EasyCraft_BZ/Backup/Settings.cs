// Decompiled with JetBrains decompiler
// Type: EasyCraft.Settings
// Assembly: EasyCraft_BZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7E51DAF-36A4-4014-9330-4A2B18F15F4A
// Assembly location: C:\Users\pred1\Desktop\EasyCraft_BZ.dll

using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace EasyCraft
{
  public class Settings
  {
    public bool autoCraft = true;
    public NeighboringStorage useStorage = NeighboringStorage.Range100;
    public ReturnSurplus returnSurplus;

    public void Save()
    {
      File.WriteAllText(Path.Combine(Main.Dir, "settings.json"), JsonUtility.ToJson((object) this, true));
    }

    public void Load()
    {
      Settings settings = new Settings();
      string path = Path.Combine(Main.Dir, "settings.json");
      if (File.Exists(path))
      {
        try
        {
          settings = (Settings) JsonUtility.FromJson<Settings>(File.ReadAllText(path));
        }
        catch (Exception ex)
        {
          Debug.LogException(ex);
          settings.Save();
        }
      }
      foreach (FieldInfo field in typeof (Settings).GetFields())
      {
        if (field.IsPublic)
          field.SetValue((object) this, field.GetValue((object) settings));
      }
    }
  }
}
