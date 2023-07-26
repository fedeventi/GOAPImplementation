using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;

public abstract class Enemy : MonoBehaviour , IObservable, IGridEntity,IPooleable<Enemy>
{
    public bool canAttack;
    public float damageMelee;
    public float speed;
    public float lifeMax;
    protected float currentlife; 
    public float speedRotation;
    public float cadence;
    public float distanceView;
    public float angleView;
    public LayerMask obstacles;
    public Transform player;
    public Grid grid;
    public List<Vector3> waypoints = new List<Vector3>();
    public int currentNode;
    public MyStateMachine<string> _stateMachine;
    List<IObserver> _Observers = new List<IObserver>();
    public float distanceToAttack;
    public BoxQuery boxQuery;
    EnemyView view;


    public event Action<IGridEntity> OnMove;
    public Vector3 Position { get => transform.position; set => transform.position = value; }

    int waveID;
    
    public Enemy SetPosition(Vector3 pos)
    {
        transform.position = pos;
        return this;
    }

    public Enemy SetPlayer(Transform p)
    {
        player = p;
        return this;
    }

    public Enemy SetWaveID(int id)
    {
        waveID = id;
        return this;
    }


    public Enemy SetLife()
    {
        currentlife = lifeMax;
        if(_stateMachine!=null)
            TransitionSt("Walk");
        return this;
    }



    public int GetWaveID
    {
        get
        {
            return waveID;
        }
    }


    public int GetLife
    {
        get
        {
            return (int)currentlife;
        }
    }


    public void TransitionSt(string state)
    {
        _stateMachine.Transition(state);
        
    }


    public virtual void Update()
    {

    }

    protected virtual void Awake()
    {
        player = FindObjectOfType<PlayerController>().transform;
        currentlife = lifeMax;
        view = GetComponent<EnemyView>();
    }

    public virtual void Start()
    {
        grid = FindObjectOfType<Grid>();
    }


    public void Notify(string eventName)
    {
        foreach (var item in _Observers)
        {
            item.OnNotify(eventName);
        }
    }
    public float DistanceFromPlayer
    {
        get
        {
            return Vector3.Distance(transform.position, player.position);
        }
    }

    

    public bool InSight()
    {
        if (Vector3.Distance(transform.position, player.position) < distanceView)
        {
            if (Vector3.Angle(transform.forward, player.position - transform.position) < angleView)
            {
                if (BehindWall())
                    return false;
                else
                    return true;

            }
            else return false;
        }
        return false;
    }
    
    public bool BehindWall()
    {
        RaycastHit hit;
        Vector3 dir = player.position - transform.position;
        if (Physics.Raycast(transform.position + new Vector3(0, 2, 0), dir.normalized, out hit, dir.magnitude, obstacles))
        {
            if (hit.transform.gameObject.GetComponent<PlayerModel>())
            {
                return false;
            }

            else
            {
                return true;
            }
        }return true;
    }
    public virtual void Damaged(float damage)
    {
        currentlife -= damage;
        if (currentlife <= 0)
        {
            TransitionSt("Death");
        }
        else
        {
            Notify("Damaged");
        }
    }
    

    public void CanAttackOn()
    {
        canAttack = true;
    }
    public void CanAttackOff()
    {
        canAttack = false;
    }

    public void SubEvent(IObserver obs)
    {
        _Observers.Add(obs);
    }

    public void UnSubEvent(IObserver obs)
    {
        _Observers.Remove(obs);
    }



    public void OnDrawGizmos()
    {
       
    }


    protected virtual void Transitions()
    {

    }


    public static void TurnOnStatic(Enemy obj)
    {
        obj.TurnOn(obj);
    }

    public static void TurnOffStatic(Enemy obj)
    {
        obj.TurnOff(obj);
    }

    public void TurnOn(Enemy obj)
    {
        obj.gameObject.SetActive(true);
    }

    public void TurnOff(Enemy obj)
    {
        obj.gameObject.SetActive(false);
    }


}
