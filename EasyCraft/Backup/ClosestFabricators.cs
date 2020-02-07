// Decompiled with JetBrains decompiler
// Type: EasyCraft.ClosestFabricators
// Assembly: EasyCraft, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3773AFFF-02BF-495C-B57C-5121AC0BFC1B
// Assembly location: C:\Users\pred1\Desktop\EasyCraft.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasyCraft
{
  public static class ClosestFabricators
  {
    private static float cacheExpired = 0.0f;
    private static float cacheDuration = 0.5f;
    private static GhostCrafter[] cached = new GhostCrafter[0];
    private static Dictionary<CraftTree.Type, List<TechType>> craftable = new Dictionary<CraftTree.Type, List<TechType>>();

    private static GhostCrafter[] fabricators
    {
      get
      {
        if ((double) ClosestFabricators.cacheExpired < (double) Time.get_unscaledTime() || (double) ClosestFabricators.cacheExpired > (double) Time.get_unscaledTime() + (double) ClosestFabricators.cacheDuration)
        {
          ClosestFabricators.cached = ClosestFabricators.Find();
          ClosestFabricators.cacheExpired = Time.get_unscaledTime() + ClosestFabricators.cacheDuration;
        }
        return ClosestFabricators.cached;
      }
    }

    public static void Add(GhostCrafter crafter)
    {
      if (((IEnumerable<GhostCrafter>) ClosestFabricators.fabricators).Count<GhostCrafter>((Func<GhostCrafter, bool>) (x => Object.op_Equality((Object) x, (Object) crafter))) != 0)
        return;
      Array.Resize<GhostCrafter>(ref ClosestFabricators.cached, ClosestFabricators.cached.Length + 1);
      ClosestFabricators.cached[ClosestFabricators.cached.Length - 1] = crafter;
    }

    private static GhostCrafter[] Find()
    {
      List<GhostCrafter> ghostCrafterList = new List<GhostCrafter>();
      GhostCrafter[] ghostCrafterArray = new GhostCrafter[0];
      if (Main.Settings.useStorage == NeighboringStorage.Off)
        return ghostCrafterList.ToArray();
      if (Main.Settings.useStorage == NeighboringStorage.Inside && ((Player) Player.main).IsInside())
      {
        if (Object.op_Inequality((Object) ((Player) Player.main).get_currentEscapePod(), (Object) null))
          ghostCrafterArray = (GhostCrafter[]) ((Component) ((Player) Player.main).get_currentEscapePod()).GetComponentsInChildren<GhostCrafter>();
        else if (((Player) Player.main).IsInSub())
          ghostCrafterArray = (GhostCrafter[]) ((Component) ((Player) Player.main).get_currentSub()).GetComponentsInChildren<GhostCrafter>();
      }
      else if (Main.Settings.useStorage == NeighboringStorage.Range100)
      {
        Vector3 vector3_1 = Vector3.op_Subtraction(((Component) Player.main).get_transform().get_position(), ((Component) EscapePod.main).get_transform().get_position());
        if ((double) ((Vector3) ref vector3_1).get_sqrMagnitude() < 10000.0)
          ghostCrafterArray = (GhostCrafter[]) ((Component) EscapePod.main).GetComponentsInChildren<GhostCrafter>();
        foreach (SubRoot subRoot in (SubRoot[]) Object.FindObjectsOfType<SubRoot>())
        {
          Vector3 position = ((Component) Player.main).get_transform().get_position();
          if (subRoot.isCyclops != null)
          {
            Vector3 vector3_2 = Vector3.op_Subtraction(position, subRoot.GetWorldCenterOfMass());
            if ((double) ((Vector3) ref vector3_2).get_sqrMagnitude() < 10000.0)
              goto label_13;
          }
          if (subRoot.isBase == null || !(subRoot is BaseRoot baseRoot) || (double) baseRoot.GetDistanceToPlayer() >= 100.0)
            continue;
label_13:
          foreach (GhostCrafter componentsInChild in (GhostCrafter[]) ((Component) subRoot).GetComponentsInChildren<GhostCrafter>())
          {
            if (Main.IsGhostCrafterCraftTree((CraftTree.Type) componentsInChild.craftTree))
            {
              Constructable component = (Constructable) ((Component) componentsInChild).GetComponent<Constructable>();
              if (!Object.op_Implicit((Object) component) || component.get_constructed())
                ghostCrafterList.Add(componentsInChild);
            }
          }
        }
      }
      foreach (GhostCrafter ghostCrafter in ghostCrafterArray)
      {
        if (Main.IsGhostCrafterCraftTree((CraftTree.Type) ghostCrafter.craftTree))
        {
          Constructable component = (Constructable) ((Component) ghostCrafter).GetComponent<Constructable>();
          if (!Object.op_Implicit((Object) component) || component.get_constructed())
            ghostCrafterList.Add(ghostCrafter);
        }
      }
      return ((IEnumerable<GhostCrafter>) ((IEnumerable<GhostCrafter>) ghostCrafterList).OrderBy<GhostCrafter, float>((Func<GhostCrafter, float>) (x =>
      {
        Vector3 vector3 = Vector3.op_Subtraction(((Component) Player.main).get_transform().get_position(), ((Component) x).get_transform().get_position());
        return ((Vector3) ref vector3).get_sqrMagnitude();
      }))).ToArray<GhostCrafter>();
    }

    private static void GenerateTechTypeList()
    {
      ClosestFabricators.craftable.Clear();
      foreach (CraftTree.Type type in Enum.GetValues(typeof (CraftTree.Type)))
      {
        if (Main.IsGhostCrafterCraftTree(type))
        {
          List<TechType> techTypeList = new List<TechType>();
          using (IEnumerator<CraftNode> enumerator = CraftTree.GetTree(type).get_nodes().Traverse(true))
          {
            while (((IEnumerator) enumerator).MoveNext())
            {
              CraftNode current = enumerator.Current;
              if (current.get_action() == 2)
                techTypeList.Add((TechType) current.techType0);
            }
          }
          ClosestFabricators.craftable.Add(type, techTypeList);
        }
      }
    }

    public static bool CanCraft(TechType techType)
    {
      if (ClosestFabricators.craftable.Count == 0)
        ClosestFabricators.GenerateTechTypeList();
      foreach (GhostCrafter fabricator in ClosestFabricators.fabricators)
      {
        List<TechType> techTypeList;
        if (ClosestFabricators.craftable.TryGetValue((CraftTree.Type) fabricator.craftTree, out techTypeList) && techTypeList.Contains(techType))
          return true;
      }
      return false;
    }

    public static bool HasEnergy(Dictionary<TechType, int> crafted, out float needEnergy)
    {
      if (ClosestFabricators.craftable.Count == 0)
        ClosestFabricators.GenerateTechTypeList();
      needEnergy = 0.0f;
      Dictionary<PowerRelay, float> dictionary1 = new Dictionary<PowerRelay, float>();
      Dictionary<TechType, float> dictionary2 = new Dictionary<TechType, float>();
      using (Dictionary<TechType, int>.Enumerator enumerator = crafted.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<TechType, int> current = enumerator.Current;
          float num1 = (float) current.Value * 5f;
          foreach (GhostCrafter fabricator in ClosestFabricators.fabricators)
          {
            List<TechType> techTypeList;
            if (ClosestFabricators.craftable.TryGetValue((CraftTree.Type) fabricator.craftTree, out techTypeList) && techTypeList.Contains(current.Key))
            {
              PowerRelay powerRelay = fabricator.GetPowerRelay();
              if (Object.op_Inequality((Object) powerRelay, (Object) null))
              {
                float num2;
                dictionary1.TryGetValue(powerRelay, out num2);
                float num3 = powerRelay.GetPower() - num2;
                if ((double) num3 >= (double) num1)
                {
                  dictionary1.Inc<PowerRelay>(powerRelay, num1);
                  num1 = 0.0f;
                }
                else if ((double) num3 > 0.0)
                {
                  dictionary1.Inc<PowerRelay>(powerRelay, num3);
                  num1 -= num3;
                }
              }
            }
            if ((double) num1 <= 0.0)
              break;
          }
          if ((double) num1 > 0.0)
            dictionary2.Inc<TechType>(current.Key, num1);
        }
      }
      needEnergy = ((IEnumerable<KeyValuePair<TechType, float>>) dictionary2).Sum<KeyValuePair<TechType, float>>((Func<KeyValuePair<TechType, float>, float>) (x => x.Value));
      return (double) needEnergy <= 0.0;
    }

    public static bool ConsumeEnergy(Dictionary<TechType, int> crafted, out float consumeEnergy)
    {
      consumeEnergy = 0.0f;
      using (Dictionary<TechType, int>.Enumerator enumerator = crafted.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<TechType, int> current = enumerator.Current;
          float num1 = (float) current.Value * 5f;
          foreach (GhostCrafter fabricator in ClosestFabricators.fabricators)
          {
            List<TechType> techTypeList;
            if (ClosestFabricators.craftable.TryGetValue((CraftTree.Type) fabricator.craftTree, out techTypeList) && techTypeList.Contains(current.Key))
            {
              PowerRelay powerRelay = fabricator.GetPowerRelay();
              if (Object.op_Inequality((Object) powerRelay, (Object) null))
              {
                float num2 = Mathf.Min(powerRelay.GetPower(), num1);
                float num3;
                PowerSystem.ConsumeEnergy((IPowerInterface) powerRelay, num2, ref num3);
                num1 -= num3;
                consumeEnergy += num3;
              }
            }
            if ((double) num1 <= 0.0)
              break;
          }
          if ((double) num1 > 0.0)
            return false;
        }
      }
      return true;
    }
  }
}
