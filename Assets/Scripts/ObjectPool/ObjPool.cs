using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPool<T>
{
    public delegate T FactoryMethod();
    List<T> _currentStock;
    FactoryMethod _factoryMethod;
    Action<T> _turnOn;
    Action<T> _turnOff;
    bool _isDynamic;
    public ObjPool(FactoryMethod factory, Action<T> turnOn, Action<T> turnOff, int stock, bool dynamic)
    {
        _factoryMethod = factory;
        _turnOn = turnOn;
        _turnOff = turnOff;
        _currentStock = new List<T>();
        _isDynamic = dynamic;
        for (int i = 0; i < stock; i++)
        {
            var obj = _factoryMethod();
            _turnOff(obj);
            _currentStock.Add(obj);
        }
    }
    public T GetObj()
    {
        var returnedObj = default(T);
        if (_currentStock.Count > 0)
        {
            returnedObj = _currentStock[0];
            _currentStock.RemoveAt(0);
        }
        else if (_isDynamic)
            returnedObj = _factoryMethod();
        if(returnedObj != null)
            _turnOn(returnedObj);
        return returnedObj;
    }
    public void Recycle(T obj)
    {
        _turnOff(obj);
        _currentStock.Add(obj);
    }
}
