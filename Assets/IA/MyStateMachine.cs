using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyStateMachine<T> //IA2-P1
{

    State<T> _current;
    public State<T> current { get { return _current; } }
    public MyStateMachine(State<T> initialize)
    {
        _current = initialize;
        _current.OnEnter();
    }
    //Actualizamos el estado
    public void OnUpdate()
    {
        _current.OnUpdate();
    }
    public void OnFixedUpdate()
    {
        _current.OnFixedUpdate();
    }
    //Realizamos la transicion
    public void Transition(T input)
    {

        var newState = _current.GetState(input);
        if (newState == null) return;
        _current.OnSleep();
        _current = newState;
        _current.OnEnter();

    }
}
