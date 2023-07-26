using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  interface IPoolGenerator<T>
{
    void Recycle(T obj);
    T Factory();
}
