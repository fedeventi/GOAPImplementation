using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyEnemy : Enemy,IPoolGenerator<Acid>
{
    // Start is called before the first frame update
    SeekState<string> seek;
    HeavyAttackState<string> throwAcid;
    AttackState<string> attack;
    HeavyPathFindingState<string> search;
    DeathState<string> death;
    Rigidbody _rb;
    ObjPool<Acid> pool;
    public Acid acid;
    private void Awake()
    {
        grid = FindObjectOfType<Grid>();

        _rb = GetComponent<Rigidbody>();
        pool = new ObjPool<Acid>(Factory, Acid.TurnOnStatic, Acid.TurnOffStatic, 3, true);
    }

    public override void Start()
    {
        base.Start();
        Transitions();
        _stateMachine = new MyStateMachine<string>(seek);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        _stateMachine.OnUpdate();
    }
    public void ThrowAcid()
    {

        pool.GetObj().SetOwner(this).SetDirection(player.position-transform.position);
                     
        
    }
    public void Recycle(Acid acid)
    {
        pool.Recycle(acid);
    }
    public Acid Factory()
    {
        return Instantiate(acid, transform.position , Quaternion.identity).SetOwner(this);
    }
    protected override void Transitions()
    {
        base.Transitions();
        seek = new SeekState<string>(player, speed, this, distanceToAttack);
        search = new HeavyPathFindingState<string>(this, player, distanceToAttack);
        throwAcid = new HeavyAttackState<string>(this);
        attack = new AttackState<string>(cadence, this);
        death = new DeathState<string>(this);
        seek.SetTransition("Search", search);
        seek.SetTransition("Throw", throwAcid);
        seek.SetTransition("Attack", attack);
        seek.SetTransition("Death", death);
        attack.SetTransition("Walk", seek);
        attack.SetTransition("Death", death);
        throwAcid.SetTransition("Walk", seek);
        throwAcid.SetTransition("Search", search);
        throwAcid.SetTransition("Death", death);
        search.SetTransition("Walk", seek);
        search.SetTransition("Attack", attack);
        search.SetTransition("Throw", throwAcid);
        search.SetTransition("Search", search);
        search.SetTransition("Death", death);
        death.SetTransition("Walk", seek);
    }
}
