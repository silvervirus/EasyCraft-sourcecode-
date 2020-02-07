// Decompiled with JetBrains decompiler
// Type: EasyCraft.ClosestItemContainers
// Assembly: EasyCraft_BZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7E51DAF-36A4-4014-9330-4A2B18F15F4A
// Assembly location: C:\Users\pred1\Desktop\EasyCraft_BZ.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace EasyCraft
{
  public static class ClosestItemContainers
  {
    private static float cacheExpired = 0.0f;
    private static float cacheDuration = 0.5f;
    private static ItemsContainer[] cached = new ItemsContainer[0];
    private static FieldInfo labelField = typeof (ItemsContainer).GetField("_label", BindingFlags.Instance | BindingFlags.NonPublic);

    private static ItemsContainer[] containers
    {
      get
      {
        if ((double) ClosestItemContainers.cacheExpired < (double) Time.get_unscaledTime() || (double) ClosestItemContainers.cacheExpired > (double) Time.get_unscaledTime() + (double) ClosestItemContainers.cacheDuration)
        {
          ClosestItemContainers.cached = ClosestItemContainers.Find();
          ClosestItemContainers.cacheExpired = Time.get_unscaledTime() + ClosestItemContainers.cacheDuration;
        }
        return ClosestItemContainers.cached;
      }
    }

    private static ItemsContainer[] Find()
    {
      List<ItemsContainer> itemsContainerList = new List<ItemsContainer>();
      StorageContainer[] storageContainerArray = new StorageContainer[0];
      itemsContainerList.Add(((Inventory) Inventory.main).get_container());
      if (Main.Settings.useStorage == NeighboringStorage.Off)
        return itemsContainerList.ToArray();
      if (Main.Settings.useStorage == NeighboringStorage.Inside && ((Player) Player.main).IsInside())
      {
        if (((Player) Player.main).IsInSub())
          storageContainerArray = (StorageContainer[]) ((Component) ((Player) Player.main).get_currentSub()).GetComponentsInChildren<StorageContainer>();
        else if (((Player) Player.main).get_currentInterior() != null && ((Player) Player.main).get_currentInterior() is LifepodDrop currentInterior)
          storageContainerArray = (StorageContainer[]) ((Component) currentInterior).GetComponentsInChildren<StorageContainer>();
      }
      else if (Main.Settings.useStorage == NeighboringStorage.Range100)
      {
        foreach (LifepodDrop lifepodDrop in (LifepodDrop[]) Object.FindObjectsOfType<LifepodDrop>())
        {
          Vector3 vector3 = Vector3.op_Subtraction(((Component) Player.main).get_transform().get_position(), ((Component) lifepodDrop).get_transform().get_position());
          if ((double) ((Vector3) ref vector3).get_sqrMagnitude() < 10000.0)
          {
            foreach (StorageContainer componentsInChild in (StorageContainer[]) ((Component) lifepodDrop).GetComponentsInChildren<StorageContainer>())
            {
              if (componentsInChild.get_container().containerType == null && !((string) componentsInChild.storageLabel).StartsWith("Aquarium"))
              {
                Text componentInChildren = (Text) ((Component) componentsInChild).GetComponentInChildren<Text>();
                if (!Object.op_Implicit((Object) componentInChildren) || !componentInChildren.get_text().EndsWith("*"))
                {
                  Constructable component = (Constructable) ((Component) componentsInChild).GetComponent<Constructable>();
                  if (!Object.op_Implicit((Object) component) || component.get_constructed())
                    itemsContainerList.Add(componentsInChild.get_container());
                }
              }
            }
          }
        }
        foreach (SubRoot subRoot in (SubRoot[]) Object.FindObjectsOfType<SubRoot>())
        {
          Vector3 position = ((Component) Player.main).get_transform().get_position();
          if (subRoot.isCyclops != null)
          {
            Vector3 vector3 = Vector3.op_Subtraction(position, subRoot.GetWorldCenterOfMass());
            if ((double) ((Vector3) ref vector3).get_sqrMagnitude() < 10000.0)
              goto label_22;
          }
          if (subRoot.isBase == null || !(subRoot is BaseRoot baseRoot) || (double) baseRoot.GetDistanceToPlayer() >= 100.0)
            continue;
label_22:
          foreach (StorageContainer componentsInChild in (StorageContainer[]) ((Component) subRoot).GetComponentsInChildren<StorageContainer>())
          {
            if (componentsInChild.get_container().containerType == null && !((string) componentsInChild.storageLabel).StartsWith("Aquarium"))
            {
              Text componentInChildren = (Text) ((Component) componentsInChild).GetComponentInChildren<Text>();
              if (!Object.op_Implicit((Object) componentInChildren) || !componentInChildren.get_text().EndsWith("*"))
              {
                Constructable component = (Constructable) ((Component) componentsInChild).GetComponent<Constructable>();
                if (!Object.op_Implicit((Object) component) || component.get_constructed())
                  itemsContainerList.Add(componentsInChild.get_container());
              }
            }
          }
        }
        foreach (DeployableStorage deployableStorage in (DeployableStorage[]) Object.FindObjectsOfType<DeployableStorage>())
        {
          StorageContainer componentInChildren = (StorageContainer) ((Component) deployableStorage).GetComponentInChildren<StorageContainer>();
          if (Object.op_Implicit((Object) componentInChildren) && componentInChildren.get_container().containerType == null)
          {
            Vector3 vector3 = Vector3.op_Subtraction(((Component) Player.main).get_transform().get_position(), ((Component) deployableStorage).get_transform().get_position());
            if ((double) ((Vector3) ref vector3).get_sqrMagnitude() < 10000.0)
              itemsContainerList.Add(componentInChildren.get_container());
          }
        }
      }
      foreach (StorageContainer storageContainer in storageContainerArray)
      {
        if (storageContainer.get_container().containerType == null && !((string) storageContainer.storageLabel).StartsWith("Aquarium"))
        {
          Text componentInChildren = (Text) ((Component) storageContainer).GetComponentInChildren<Text>();
          if (!Object.op_Implicit((Object) componentInChildren) || !componentInChildren.get_text().EndsWith("*"))
          {
            Constructable component = (Constructable) ((Component) storageContainer).GetComponent<Constructable>();
            if (!Object.op_Implicit((Object) component) || component.get_constructed())
              itemsContainerList.Add(storageContainer.get_container());
          }
        }
      }
      return ((IEnumerable<ItemsContainer>) ((IEnumerable<ItemsContainer>) itemsContainerList).Distinct<ItemsContainer>().OrderBy<ItemsContainer, float>((Func<ItemsContainer, float>) (x =>
      {
        Vector3 vector3 = Vector3.op_Subtraction(((Component) Player.main).get_transform().get_position(), x.get_tr().get_position());
        return ((Vector3) ref vector3).get_sqrMagnitude();
      }))).ToArray<ItemsContainer>();
    }

    public static int GetPickupCount(TechType techType)
    {
      return ((IEnumerable<ItemsContainer>) ClosestItemContainers.containers).Sum<ItemsContainer>((Func<ItemsContainer, int>) (x => x.GetCount(techType)));
    }

    public static bool AddItem(TechType techType, int count = 1)
    {
      for (int index = 0; index < count; ++index)
      {
        GameObject gameObject = CraftData.InstantiateFromPrefab(techType, false);
        if (Object.op_Inequality((Object) gameObject, (Object) null))
        {
          Pickupable component = (Pickupable) gameObject.GetComponent<Pickupable>();
          if (Object.op_Inequality((Object) component, (Object) null))
          {
            bool flag = false;
            component.Pickup(false);
            if (Main.Settings.returnSurplus == ReturnSurplus.Lockers)
            {
              foreach (ItemsContainer container in ClosestItemContainers.containers)
              {
                if ((string) ClosestItemContainers.labelField.GetValue((object) container) == "Autosorter" && container.AddItem(component) != null)
                {
                  Console.WriteLine(string.Format("[EasyCraft] Add {0}", (object) techType));
                  flag = true;
                  break;
                }
              }
            }
            if (!flag)
            {
              foreach (ItemsContainer container in ClosestItemContainers.containers)
              {
                if ((Main.Settings.returnSurplus != ReturnSurplus.Lockers || container != ((Inventory) Inventory.main).get_container()) && container.AddItem(component) != null)
                {
                  Console.WriteLine(string.Format("[EasyCraft] Add {0}", (object) techType));
                  flag = true;
                  break;
                }
              }
              if (Main.Settings.returnSurplus == ReturnSurplus.Lockers && !flag && ((Inventory) Inventory.main).get_container().AddItem(component) != null)
              {
                Console.WriteLine(string.Format("[EasyCraft] Add {0}", (object) techType));
                flag = true;
              }
            }
            if (!flag)
            {
              component.Drop(Vector3.op_Addition(((Component) Player.main).get_transform().get_position(), Vector3.get_down()), Vector3.get_down());
              Console.WriteLine(string.Format("[EasyCraft] Drop {0}", (object) techType));
            }
          }
        }
      }
      return true;
    }

    public static bool DestroyItem(TechType techType, int count = 1)
    {
      int num = 0;
      foreach (ItemsContainer container in ClosestItemContainers.containers)
      {
        for (int index = 0; index < count; ++index)
        {
          if (container.DestroyItem(techType))
          {
            ++num;
            Console.WriteLine(string.Format("[EasyCraft] Remove {0}", (object) techType));
          }
          if (num == count)
            return true;
        }
      }
      Console.WriteLine(string.Format("[EasyCraft] Unable to remove {0}", (object) techType));
      return false;
    }
  }
}
