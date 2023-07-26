using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using FSM;
using UnityEngine;

public class GoapPlanner {

    private const int _WATCHDOG_MAX = 200;

    private int _watchdog;

    public void Run(GOAPState from, GOAPState to, IEnumerable<GOAPAction> actions, Func<IEnumerator, Coroutine> _coroutine, Action<IEnumerable<GOAPAction>> action)
    {
        //Debug.Log("GoapPlanner :: Run");
        _watchdog = _WATCHDOG_MAX;

        var astar = new AStar<GOAPState>(_coroutine);
        
        astar.OnPathCompleted += GoapReturn;
        astar.StartRun(from,
                             state => Satisfies(state, to),
                             node => Explode(node, actions, ref _watchdog),
                             state => GetHeuristic(state, to), action);
    }
    public void GoapReturn(IEnumerable<GOAPState> goapstate, Action<IEnumerable<GOAPAction>> goapaction)
    {
        //Debug.Log("GoapPlanner :: GoapReturn");
        var calculation = CalculateGoap(goapstate);
        goapaction?.Invoke(calculation);
    }

    public static FiniteStateMachine ConfigureFSM(IEnumerable<GOAPAction> plan, Func<IEnumerator, Coroutine> startCoroutine)
    {
        var prevState = plan.First().linkedState;

        var fsm = new FiniteStateMachine(prevState, startCoroutine);
        foreach (var item in plan)
        {
            Debug.Log(item.linkedState.Name);
            item.linkedState.Transitions.Clear();
        }
        foreach (var action in plan.Skip(1))
        {
            if (prevState == action.linkedState) continue;
            fsm.AddTransition("On" + action.linkedState.Name, prevState, action.linkedState);

            prevState = action.linkedState;
        }


        return fsm;
    }

    private IEnumerable<GOAPAction> CalculateGoap(IEnumerable<GOAPState> sequence)
    {
        //Debug.Log("GoapPlanner :: CalculateGoap");

        //foreach (var act in sequence.Skip(1))
        //{
        //    Debug.Log(act);
        //}
        
        return sequence.Skip(1).Select(x => x.generatingAction);
    }

    private static float GetHeuristic(GOAPState from, GOAPState goal) => goal.values.Count(kv => !kv.In(from.values));
    private static bool Satisfies(GOAPState state, GOAPState to) => to.values.All(kv => kv.In(state.values));

    private static IEnumerable<WeightedNode<GOAPState>> Explode(GOAPState node, IEnumerable<GOAPAction> actions,
                                                                ref int watchdog)
    {
        if (watchdog == 0) return Enumerable.Empty<WeightedNode<GOAPState>>();
        watchdog--;

        return actions.Where(action => action.preconditions.All(kv => kv.In(node.values)))
                      .Aggregate(new List<WeightedNode<GOAPState>>(), (possibleList, action) => {
                          var newState = new GOAPState(node);
                          newState.values.UpdateWith(action.effects);
                          newState.generatingAction = action;
                          newState.step = node.step + 1;
                          possibleList.Add(new WeightedNode<GOAPState>(newState, action.cost));
                          
                          return possibleList;
                      });
    }
}
