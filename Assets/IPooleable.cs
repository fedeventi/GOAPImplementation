using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooleable<T>
{
    void TurnOn(T obj);
    void TurnOff(T obj);
}
 