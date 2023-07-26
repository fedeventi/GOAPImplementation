using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RoulleteWheel<T> //IA2-P1
{
    public Dictionary<int, T> myDic = new Dictionary<int, T>();

    int ProbabilityToDropSomething=100;
   
    public T Drop()
    {
        int dropChance = Random.Range(0, 101);
        if (dropChance >ProbabilityToDropSomething)
        {
            return default(T);
        }
        else
        {
            int weight=0;
            foreach (var probability in myDic)
            {
                weight += probability.Key;
            }
            int randomValue = Random.Range(0, weight);
           
            foreach (var item in myDic)
            {
                if (randomValue < item.Key)
                {
                    return item.Value;
                }
                randomValue -= item.Key;
            }


        }
        return default(T);
    }
  
}
