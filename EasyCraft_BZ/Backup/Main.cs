// Decompiled with JetBrains decompiler
// Type: EasyCraft.Main
// Assembly: EasyCraft_BZ, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E7E51DAF-36A4-4014-9330-4A2B18F15F4A
// Assembly location: C:\Users\pred1\Desktop\EasyCraft_BZ.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace EasyCraft
{
  public class Main : MonoBehaviour
  {
    private static Settings settings = new Settings();
    private static float waitingForMessage = 1f;
    private static float lastTimeShowMessage = 0.0f;

    public static Settings Settings
    {
      get
      {
        return Main.settings;
      }
    }

    public static string Dir
    {
      get
      {
        return Environment.CurrentDirectory + "\\QMods\\EasyCraft_BZ\\";
      }
    }

    public static void Load()
    {
      Main.settings.Load();
      new GameObject("EasyCraft_BZ.Controller").AddComponent<Main>();
    }

    private IEnumerator Start()
    {
      yield return (object) new WaitForSeconds(1f);
      while (!uGUI_SceneLoading.get_IsLoadingScreenFinished() || !Object.op_Implicit((Object) uGUI.get_main()) || ((uGUI_SceneLoading) uGUI.get_main().loading).get_IsLoading())
        yield return (object) null;
      this.Run();
    }

    private void Run()
    {
      uGUI_OptionsPanel uGuiOptionsPanel = ((IEnumerable<uGUI_OptionsPanel>) ((Component) IngameMenu.main).GetComponentsInChildren<uGUI_OptionsPanel>(true)).Where<uGUI_OptionsPanel>((Func<uGUI_OptionsPanel, bool>) (x => ((Object) x).get_name() == "Options")).FirstOrDefault<uGUI_OptionsPanel>();
      if (Object.op_Implicit((Object) uGuiOptionsPanel))
        ((Component) uGuiOptionsPanel).get_gameObject().AddComponent<Options>();
      Console.WriteLine("[EasyCraft] Run");
    }

    private static bool ShowMessage(string str)
    {
      if ((double) Main.lastTimeShowMessage + (double) Main.waitingForMessage >= (double) Time.get_unscaledTime() && (double) Main.lastTimeShowMessage <= (double) Time.get_unscaledTime())
        return false;
      ErrorMessage.AddWarning(str);
      Main.lastTimeShowMessage = Time.get_unscaledTime();
      return true;
    }

    public static bool IsGhostCrafterCraftTree(CraftTree.Type type)
    {
      return type != null && type != 8 && (type != 2 && type != 10) && (type != 4 && type != 5);
    }

    public static bool IsCraftRecipeFulfilledAdvanced(TechType techType)
    {
      if (Object.op_Equality((Object) Inventory.main, (Object) null))
        return false;
      if (!GameModeUtils.RequiresIngredients())
        return true;
      Dictionary<TechType, int> consumable = new Dictionary<TechType, int>();
      Dictionary<TechType, int> crafted = new Dictionary<TechType, int>();
      return Main._IsCraftRecipeFulfilledAdvanced(techType, techType, consumable, crafted, 0);
    }

    private static bool _IsCraftRecipeFulfilledAdvanced(
      TechType parent,
      TechType techType,
      Dictionary<TechType, int> consumable,
      Dictionary<TechType, int> crafted,
      int depth = 0)
    {
      if (depth >= 5)
        return false;
      ReadOnlyCollection<Ingredient> ingredients = TechData.GetIngredients(techType);
      if (ingredients == null || ingredients.Count <= 0)
        return false;
      crafted.Inc<TechType>(techType, 1);
      using (IEnumerator<Ingredient> enumerator1 = ingredients.GetEnumerator())
      {
        while (((IEnumerator) enumerator1).MoveNext())
        {
          Ingredient current1 = enumerator1.Current;
          TechType techType1 = current1.get_techType();
          if (parent == techType1)
            return false;
          int pickupCount = ClosestItemContainers.GetPickupCount(techType1);
          int num1 = consumable.ContainsKey(techType1) ? consumable[techType1] : 0;
          int num2 = Mathf.Max(0, pickupCount - num1);
          int amount = current1.get_amount();
          if (num2 < amount)
          {
            EquipmentType equipmentType = TechData.GetEquipmentType(techType1);
            if (!Main.Settings.autoCraft || equipmentType == 3 || (equipmentType == 7 || equipmentType == 8) || (equipmentType == 14 || equipmentType == 5 || (equipmentType == 4 || equipmentType == 2)) || (equipmentType == 13 || equipmentType == 6 || (equipmentType == 9 || equipmentType == 19) || equipmentType == 18))
              return false;
            consumable.Inc<TechType>(techType1, num2);
            for (int index = 0; index < amount - num2; ++index)
            {
              if (!Main._IsCraftRecipeFulfilledAdvanced(parent, techType1, consumable, crafted, depth + 1) || !ClosestFabricators.CanCraft(techType1))
                return false;
              int craftAmount = TechData.GetCraftAmount(techType1);
              if (craftAmount > 0)
              {
                index += craftAmount - 1;
                consumable.Inc<TechType>(techType1, -craftAmount);
                ReadOnlyCollection<TechType> linkedItems = TechData.GetLinkedItems(techType1);
                if (linkedItems != null)
                {
                  using (IEnumerator<TechType> enumerator2 = linkedItems.GetEnumerator())
                  {
                    while (((IEnumerator) enumerator2).MoveNext())
                    {
                      TechType current2 = enumerator2.Current;
                      consumable.Inc<TechType>(current2, -1);
                    }
                  }
                }
              }
            }
            consumable.Inc<TechType>(techType1, amount - num2);
            int num3 = consumable.ContainsKey(techType1) ? consumable[techType1] : 0;
            if (pickupCount < num3)
              return false;
          }
          else
            consumable.Inc<TechType>(techType1, current1.get_amount());
        }
      }
      return true;
    }

    public static void GhostCraft(GhostCrafter crafter, TechType techType, float duration)
    {
      Console.WriteLine(string.Format("[EasyCraft] Craft {0}", (object) techType));
      Base @base = (Base) typeof (GhostCrafter).GetField("baseComp", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue((object) crafter);
      PowerRelay powerRelay = (PowerRelay) typeof (GhostCrafter).GetField("powerRelay", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue((object) crafter);
      float num1 = (float) typeof (GhostCrafter).GetField("spawnAnimationDelay", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue((object) crafter);
      float num2 = (float) typeof (GhostCrafter).GetField("spawnAnimationDuration", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue((object) crafter);
      PropertyInfo property = typeof (GhostCrafter).GetProperty("state", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      CrafterLogic crafterLogic = (CrafterLogic) typeof (GhostCrafter).GetProperty("logic", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue((object) crafter, (object[]) null);
      MethodInfo method = typeof (GhostCrafter).GetMethod("OnCraftingBegin", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      if (Object.op_Implicit((Object) @base) && !@base.IsPowered(((Component) crafter).get_transform().get_position()))
        return;
      Dictionary<TechType, int> consumable = new Dictionary<TechType, int>();
      Dictionary<TechType, int> crafted = new Dictionary<TechType, int>();
      if (GameModeUtils.RequiresIngredients())
      {
        if (!Main._IsCraftRecipeFulfilledAdvanced(techType, techType, consumable, crafted, 0))
        {
          Main.ShowMessage(Language.get_main().Get("DontHaveNeededIngredients"));
          return;
        }
        using (Dictionary<TechType, int>.Enumerator enumerator = crafted.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<TechType, int> current = enumerator.Current;
            float num3 = 0.0f;
            if (TechData.GetCraftTime(current.Key, ref num3))
              duration += num3 * (float) current.Value;
          }
        }
      }
      else
        crafted.Add(techType, 1);
      duration = Mathf.Clamp(duration, num1 + num2, 20f);
      if (Main.Settings.autoCraft && crafted.Count > 1)
      {
        if (Main.Settings.useStorage == NeighboringStorage.Off)
          ClosestFabricators.Add(crafter);
        if (crafter.needsPower != null && !Main.ConsumeEnergy(crafted))
          return;
      }
      else if (crafter.needsPower != null)
      {
        if (!CrafterLogic.ConsumeEnergy(powerRelay, 5f))
        {
          if (!Main.ShowMessage(Language.get_main().Get("NoPower")))
            return;
          Console.WriteLine(string.Format("[EasyCraft] Not enough energy {0}", (object) 5f));
          return;
        }
        Console.WriteLine(string.Format("[EasyCraft] Consume energy {0}", (object) 5f));
      }
      Main.ConsumeIngredients(consumable);
      if (!Object.op_Inequality((Object) crafterLogic, (Object) null) || !crafterLogic.Craft(techType, duration))
        return;
      Console.WriteLine("[EasyCraft] Craft permitted");
      property.SetValue((object) crafter, (object) true, (object[]) null);
      method.Invoke((object) crafter, new object[2]
      {
        (object) techType,
        (object) duration
      });
    }

    public static void ConstructorCraft(
      ConstructorInput crafter,
      TechType techType,
      float duration)
    {
      Console.WriteLine(string.Format("[EasyCraft] Craft {0}", (object) techType));
      PropertyInfo property = typeof (ConstructorInput).GetProperty("state", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      CrafterLogic crafterLogic = (CrafterLogic) typeof (ConstructorInput).GetProperty("logic", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue((object) crafter, (object[]) null);
      MethodInfo method1 = typeof (ConstructorInput).GetMethod("OnCraftingBegin", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      MethodInfo method2 = typeof (ConstructorInput).GetMethod("ReturnValidCraftingPosition", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
      Transform itemSpawnPoint = ((Constructor) crafter.constructor).GetItemSpawnPoint(techType);
      if (techType == 2003)
      {
        if (!(bool) method2.Invoke((object) crafter, new object[1]
        {
          (object) itemSpawnPoint.get_position()
        }))
        {
          ((PDANotification) crafter.invalidNotification).Play();
          return;
        }
      }
      duration = 10f;
      switch (techType - 2000)
      {
        case 0:
        case 1:
          duration = 10f;
          break;
        case 3:
          duration = 20f;
          break;
        case 7:
        case 8:
        case 9:
        case 10:
        case 11:
        case 12:
        case 13:
        case 14:
          duration = 15f;
          break;
        default:
          if (techType == 5900)
          {
            duration = 25f;
            break;
          }
          break;
      }
      Dictionary<TechType, int> consumable = new Dictionary<TechType, int>();
      Dictionary<TechType, int> crafted = new Dictionary<TechType, int>();
      if (GameModeUtils.RequiresIngredients())
      {
        if (!Main._IsCraftRecipeFulfilledAdvanced(techType, techType, consumable, crafted, 0))
        {
          Main.ShowMessage(Language.get_main().Get("DontHaveNeededIngredients"));
          return;
        }
        crafted.Remove(techType);
        using (Dictionary<TechType, int>.Enumerator enumerator = crafted.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            KeyValuePair<TechType, int> current = enumerator.Current;
            float num = 0.0f;
            if (TechData.GetCraftTime(current.Key, ref num))
              duration += num * (float) current.Value;
          }
        }
      }
      duration = Mathf.Clamp(duration, 3f, 50f);
      if (Main.Settings.autoCraft && ((IEnumerable<KeyValuePair<TechType, int>>) crafted).Sum<KeyValuePair<TechType, int>>((Func<KeyValuePair<TechType, int>, int>) (x => x.Value)) > 0)
      {
        if (Main.Settings.useStorage != NeighboringStorage.Range100 && GameModeUtils.RequiresPower())
        {
          if (!Main.ShowMessage(Language.get_main().Get("NoPower")))
            return;
          Console.WriteLine("[EasyCraft] Not enough energy");
          return;
        }
        if (!Main.ConsumeEnergy(crafted))
          return;
      }
      Main.ConsumeIngredients(consumable);
      if (!Object.op_Inequality((Object) crafterLogic, (Object) null) || !crafterLogic.Craft(techType, duration))
        return;
      Console.WriteLine("[EasyCraft] Craft permitted");
      property.SetValue((object) crafter, (object) true, (object[]) null);
      method1.Invoke((object) crafter, new object[2]
      {
        (object) techType,
        (object) duration
      });
    }

    public static bool Construct(Constructable construct)
    {
      List<TechType> techTypeList = (List<TechType>) typeof (Constructable).GetField("resourceMap", BindingFlags.Instance | BindingFlags.NonPublic).GetValue((object) construct);
      MethodInfo method1 = typeof (Constructable).GetMethod("GetResourceID", BindingFlags.Instance | BindingFlags.NonPublic);
      MethodInfo method2 = typeof (Constructable).GetMethod("UpdateMaterial", BindingFlags.Instance | BindingFlags.NonPublic);
      float num1 = (float) typeof (Constructable).GetMethod("GetConstructInterval", BindingFlags.Static | BindingFlags.NonPublic).Invoke((object) construct, (object[]) null);
      if (construct._constructed != null)
        return false;
      int count = techTypeList.Count;
      int num2 = (int) method1.Invoke((object) construct, (object[]) null);
      Constructable constructable = construct;
      constructable.constructedAmount = (__Null) (constructable.constructedAmount + (double) Time.get_deltaTime() / ((double) count * (double) num1));
      construct.constructedAmount = (__Null) (double) Mathf.Clamp01((float) construct.constructedAmount);
      int num3 = (int) method1.Invoke((object) construct, (object[]) null);
      if (num3 != num2)
      {
        TechType techType = techTypeList[num3 - 1];
        if (GameModeUtils.RequiresIngredients())
        {
          if (ClosestItemContainers.GetPickupCount(techType) > 0)
          {
            if (!ClosestItemContainers.DestroyItem(techType, 1))
            {
              construct.constructedAmount = (__Null) ((double) num2 / (double) count);
              return false;
            }
            uGUI_IconNotifier.get_main().Play(techType, (uGUI_IconNotifier.AnimationType) 1, (uGUI_IconNotifier.AnimationDone) null);
          }
          else if (Main.Settings.autoCraft && Main.Settings.useStorage != NeighboringStorage.Off)
          {
            Dictionary<TechType, int> dictionary = new Dictionary<TechType, int>();
            Dictionary<TechType, int> crafted = new Dictionary<TechType, int>();
            if (!Main._IsCraftRecipeFulfilledAdvanced(techType, techType, dictionary, crafted, 0) || !ClosestFabricators.CanCraft(techType))
            {
              construct.constructedAmount = (__Null) ((double) num2 / (double) count);
              return false;
            }
            if (!Main.ConsumeEnergy(crafted))
            {
              construct.constructedAmount = (__Null) ((double) num2 / (double) count);
              return false;
            }
            int craftAmount = TechData.GetCraftAmount(techType);
            if (craftAmount > 0)
            {
              dictionary.Inc<TechType>(techType, -(craftAmount - 1));
              ReadOnlyCollection<TechType> linkedItems = TechData.GetLinkedItems(techType);
              if (linkedItems != null)
              {
                using (IEnumerator<TechType> enumerator = linkedItems.GetEnumerator())
                {
                  while (((IEnumerator) enumerator).MoveNext())
                  {
                    TechType current = enumerator.Current;
                    dictionary.Inc<TechType>(current, -1);
                  }
                }
              }
            }
            Main.ConsumeIngredients(dictionary);
          }
          else
          {
            construct.constructedAmount = (__Null) ((double) num2 / (double) count);
            return false;
          }
        }
      }
      method2.Invoke((object) construct, (object[]) null);
      if (construct.constructedAmount >= 1.0)
        construct.SetState(true, true);
      return true;
    }

    private static void ConsumeIngredients(Dictionary<TechType, int> consumable)
    {
      if (!GameModeUtils.RequiresIngredients())
        return;
      using (Dictionary<TechType, int>.Enumerator enumerator = consumable.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<TechType, int> current = enumerator.Current;
          if (current.Value > 0)
          {
            ClosestItemContainers.DestroyItem(current.Key, current.Value);
            uGUI_IconNotifier.get_main().Play(current.Key, (uGUI_IconNotifier.AnimationType) 1, (uGUI_IconNotifier.AnimationDone) null);
          }
        }
      }
      using (Dictionary<TechType, int>.Enumerator enumerator = consumable.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          KeyValuePair<TechType, int> current = enumerator.Current;
          if (current.Value < 0)
          {
            ClosestItemContainers.AddItem(current.Key, Mathf.Abs(current.Value));
            uGUI_IconNotifier.get_main().Play(current.Key, (uGUI_IconNotifier.AnimationType) 0, (uGUI_IconNotifier.AnimationDone) null);
          }
        }
      }
    }

    private static bool ConsumeEnergy(Dictionary<TechType, int> crafted)
    {
      if (GameModeUtils.RequiresPower() && crafted.Count > 0)
      {
        float needEnergy;
        if (!ClosestFabricators.HasEnergy(crafted, out needEnergy))
        {
          if (Main.ShowMessage(Language.get_main().Get("NoPower")))
            Console.WriteLine(string.Format("[EasyCraft] Not enough energy {0}", (object) needEnergy));
          return false;
        }
        float consumeEnergy;
        if (!ClosestFabricators.ConsumeEnergy(crafted, out consumeEnergy))
        {
          if (Main.ShowMessage(Language.get_main().Get("NoPower")))
            Console.WriteLine("[EasyCraft] Not enough energy");
          return false;
        }
        Console.WriteLine(string.Format("[EasyCraft] Consume energy {0}", (object) consumeEnergy));
      }
      return true;
    }

    public static void WriteIngredients(IList<Ingredient> ingredients, List<TooltipIcon> icons)
    {
      if (ingredients == null)
        return;
      StringBuilder stringBuilder = new StringBuilder();
      int index = 0;
      for (int count = ((ICollection<Ingredient>) ingredients).Count; index < count; ++index)
      {
        stringBuilder.Length = 0;
        Ingredient ingredient = ingredients[index];
        TechType techType = ingredient.get_techType();
        int pickupCount = ClosestItemContainers.GetPickupCount(techType);
        int amount = ingredient.get_amount();
        int num = pickupCount >= amount ? 1 : (!GameModeUtils.RequiresIngredients() ? 1 : 0);
        Atlas.Sprite sprite = SpriteManager.Get(techType);
        if (num != 0)
          stringBuilder.Append("<color=#94DE00FF>");
        else
          stringBuilder.Append("<color=#DF4026FF>");
        string orFallback = TechTypeExtensions.GetOrFallback(Language.get_main(), ((CachedEnumString<TechType>) TooltipFactory.techTypeIngredientStrings).Get(techType), techType);
        stringBuilder.Append(orFallback);
        if (amount > 1)
        {
          stringBuilder.Append(" x");
          stringBuilder.Append(amount);
        }
        if (pickupCount > 0 && pickupCount < amount)
        {
          stringBuilder.Append(" (");
          stringBuilder.Append(pickupCount);
          stringBuilder.Append(")");
        }
        stringBuilder.Append("</color>");
        icons.Add(new TooltipIcon(sprite, stringBuilder.ToString()));
      }
    }

    public Main()
    {
      base.\u002Ector();
    }
  }
}
