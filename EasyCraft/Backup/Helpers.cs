// Decompiled with JetBrains decompiler
// Type: EasyCraft.Helpers
// Assembly: EasyCraft, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3773AFFF-02BF-495C-B57C-5121AC0BFC1B
// Assembly location: C:\Users\pred1\Desktop\EasyCraft.dll

using Harmony;
using System.Collections.Generic;
using UnityEngine;
using UWE;

namespace EasyCraft
{
  public static class Helpers
  {
    public static void Inc<T>(this Dictionary<T, int> dictionary, T key, int value = 1)
    {
      int num;
      dictionary.TryGetValue(key, out num);
      dictionary[key] = num + value;
    }

    public static void Inc<T>(this Dictionary<T, float> dictionary, T key, float value = 1f)
    {
      float num;
      dictionary.TryGetValue(key, out num);
      dictionary[key] = num + value;
    }

    public static PowerRelay GetPowerRelay(this GhostCrafter crafter)
    {
      return (PowerRelay) Traverse.Create((object) crafter).Field("powerRelay").GetValue<PowerRelay>();
    }

    public static float GetDistanceToPlayer(this BaseRoot baseRoot)
    {
      Int3 grid = ((Base) baseRoot.baseComp).WorldToGrid(((Component) Player.main).get_transform().get_position());
      int num1 = int.MaxValue;
      Int3 int3_1;
      ((Int3) ref int3_1).\u002Ector(int.MaxValue);
      Int3.RangeEnumerator allCells = ((Base) baseRoot.baseComp).get_AllCells();
      using (Int3.RangeEnumerator enumerator = ((Int3.RangeEnumerator) ref allCells).GetEnumerator())
      {
        while (((Int3.RangeEnumerator) ref enumerator).MoveNext())
        {
          Int3 current = ((Int3.RangeEnumerator) ref enumerator).get_Current();
          Int3 int3_2 = Int3.op_Subtraction(grid, current);
          int num2 = ((Int3) ref int3_2).SquareMagnitude();
          if (num2 < num1)
          {
            int3_1 = current;
            num1 = num2;
          }
        }
      }
      Vector3 vector3_1;
      ((Base) baseRoot.baseComp).GridToWorld(int3_1, Utils.get_half3(), ref vector3_1);
      Vector3 vector3_2 = Vector3.op_Subtraction(((Component) Player.main).get_transform().get_position(), vector3_1);
      return ((Vector3) ref vector3_2).get_magnitude();
    }
  }
}
