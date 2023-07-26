using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObservable 
{
    void Notify(string eventName);
    void SubEvent(IObserver obs);
    void UnSubEvent(IObserver obs);
}
