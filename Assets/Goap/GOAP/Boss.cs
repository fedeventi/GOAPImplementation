using System.Collections.Generic;
using System.Linq;
using FSM;
using UnityEngine;
using System;

public enum StatesGoap
{
    isPlayerInSight,
    isPlayerNear,
    isPlayerAlive,
    hasBullets,
    itemsAvaiable,
    isPlayerBehindWall,
    distance,
    highLife
}
public class Boss : Enemy
{

    public LookState lookState;
    public ChaseState chaseState;
    public MeleeAttackState meleeAttackState;
    public PickAmmo pickAmmo;
    public RangeAttackState rangeAttack;
    public LaunchGrenade throwGrenade;
    private FiniteStateMachine _fsm;

    private float _lastReplanTime;
    private float _replanRate = .5f;

    // public Brick item;
    float distNearItem(GameObject ammo)
    {
        if (ammo == null)
            return 25000;
        return Vector3.Distance(ammo.transform.position, transform.position);
    }
    public bool hasBullet = false;

    public GameObject itemContenedor;
    public GameObject item;


    public int playerLife = 100;
    Action<IEnumerable<GOAPAction>> goapToPlan;

    protected override void Awake()
    {
        goapToPlan += ConfigureFsm;
        base.Awake();
        itemContenedor = FindObjectOfType<AmmoSpawner>().gameObject;
    }



    public override void Start()
    {
        base.Start();
        lookState.OnNeedsReplan += OnReplan;
        chaseState.OnNeedsReplan += OnReplan;
        meleeAttackState.OnNeedsReplan += OnReplan;
        pickAmmo.OnNeedsReplan += OnReplan;
        rangeAttack.OnNeedsReplan += OnReplan;
        throwGrenade.OnNeedsReplan += OnReplan;
        var actions = new List<GOAPAction>();
        var from = new GOAPState();
        var to = new GOAPState();
        DoPlanning(out actions, out from, out to);
        var planner = new GoapPlanner();

        var txtActions = "1_ ";
        foreach (var item in actions)
        {
            txtActions += $"{item.name}|";
        }
        //Debug.Log(txtActions);

        planner.Run(from, to, actions, StartCoroutine, goapToPlan);
    }

    private void DoPlanning(out List<GOAPAction> actions, out GOAPState from, out GOAPState to)
    {
        ;

        var ff = new Func<PlanerValue, bool>(f => f.GetValue<float>() < distanceToAttack);  //f => f.GetValue<float>() < distanceToAttack;


        //Debug.Log("--------->" + ff);

        actions = new List<GOAPAction>
        {


                                              new GOAPAction("Patrol")
                                                 .Effect(StatesGoap.isPlayerInSight, f => f.SetValue(true))
                                                 .LinkedState(lookState)
                                                 .Cost(1),

                                              //new GOAPAction("Chase")
                                              //   .Pre(StatesGoap.isPlayerInSight, f => f.GetValue<bool>())
                                              //   .Effect(StatesGoap.isPlayerNear, f => f.SetValue(true))
                                              //   .LinkedState(chaseState)
                                              //   .Cost(5),

                                              new GOAPAction("Pick Ammo")
                                                  .Pre(StatesGoap.isPlayerInSight, f =>  f.GetValue<bool>())
                                                  .Pre(StatesGoap.itemsAvaiable, f => f.GetValue<bool>() )
                                                  .Pre(StatesGoap.hasBullets, f =>  f.GetValue<bool>() )
                                                  .Effect(StatesGoap.hasBullets, f => f.SetValue(true))
                                                  .LinkedState(pickAmmo)
                                                  .Cost(1+distNearItem(item)),

                                              new GOAPAction("Melee Attack")
                                                 .Pre(StatesGoap.isPlayerNear, f => f.GetValue<bool>())
                                                 .Effect(StatesGoap.isPlayerAlive, f => f.SetValue(false))
                                                 .LinkedState(meleeAttackState)
                                                 .Cost(1),

                                              new GOAPAction("Range Attack")
                                                  .Pre(StatesGoap.hasBullets, f =>  f.GetValue<bool>())
                                                  .Effect(StatesGoap.isPlayerAlive, f => f.SetValue(false))
                                                  //.Effect(StatesGoap.hasBullets, f => f.SetValue(false))
                                                  .LinkedState(rangeAttack),

                                              new GOAPAction("Launch Grenade")
                                                  .Pre(StatesGoap.isPlayerBehindWall,f => f.GetValue<bool>() )
                                                  .Pre(StatesGoap.isPlayerInSight, f =>  f.GetValue<bool>() )
                                                  .Effect(StatesGoap.isPlayerAlive, f => f.SetValue(false))
                                                  .LinkedState(throwGrenade)
        };



        from = new GOAPState();
        from.values[StatesGoap.isPlayerInSight] = new PlanerValue(InSight());
        from.values[StatesGoap.isPlayerNear] = new PlanerValue(IsNear());
        from.values[StatesGoap.isPlayerAlive] = new PlanerValue(PlayerAlive);
        from.values[StatesGoap.hasBullets] = new PlanerValue(hasBullet);
        from.values[StatesGoap.itemsAvaiable] = new PlanerValue(AmmoAvaiable);
        from.values[StatesGoap.isPlayerBehindWall] = new PlanerValue(BehindWall());
        Debug.Log($"In Sight :{ from.values[StatesGoap.isPlayerInSight].GetValue<bool>()}");
        Debug.Log($"Is Near :{ from.values[StatesGoap.isPlayerNear].GetValue<bool>()}");
        Debug.Log($"Is Alive :{ from.values[StatesGoap.isPlayerAlive].GetValue<bool>()}");
        Debug.Log($"has bullets :{ from.values[StatesGoap.hasBullets].GetValue<bool>()}");
        Debug.Log($"item avaiables :{ from.values[StatesGoap.itemsAvaiable].GetValue<bool>()}");
        Debug.Log($"is player behind wall :{ from.values[StatesGoap.isPlayerBehindWall].GetValue<bool>()}");
       
        

         to = new GOAPState();

        to.values[StatesGoap.isPlayerAlive] = new PlanerValue(false);
    }


    //private void PlanAndExecute() {


    //    item = NearestAmmo(transform);


    //    // bool -> HASBULLA, BehindWall, InSight | float -> isPlayerNear | string -> itemsAvaiable | int -> itemsAvaiable
    //    var actions = new List<GOAPAction>{
    //                                         new GOAPAction("Patrol")
    //                                             .Effect(StatesGoap.isPlayerInSight, f => f.SetValue(true))
    //                                             .Effect(StatesGoap.isPlayerAlive, f => f.SetValue(true))
    //                                             .LinkedState(lookState)
    //                                             .Cost(1),

    //                                          new GOAPAction("Chase")
    //                                             //.Pre(StatesGoap.isPlayerInSight, f => f.GetValue<bool>() == true)
    //                                             //.Effect(StatesGoap.isPlayerNear, f => f.SetValue(0f))
    //                                             .Effect(StatesGoap.isPlayerAlive, f => f.SetValue(true))
    //                                             .LinkedState(chaseState)
    //                                             .Cost(5),

    //                                          //new GOAPAction("Pick Ammo")
    //                                          //    .Pre(StatesGoap.isPlayerInSight, f => f.GetValue<bool>() == true)
    //                                          //    .Pre(StatesGoap.itemsAvaiable, f =>f.GetValue<string>() == "SI")
    //                                          //    .Pre(StatesGoap.hasBullets, f => f.GetValue<bool>() == false)
    //                                          //    .Effect(StatesGoap.hasBullets, f => f.SetValue(true))
    //                                          //    .LinkedState(pickAmmo)
    //                                          //    .Cost(1+distNearItem(item)),

    //                                          //new GOAPAction("Melee Attack")
    //                                          //   .Pre(StatesGoap.isPlayerNear, f => f.GetValue<float>() < distanceToAttack)
    //                                          //   .Effect(StatesGoap.isPlayerAlive, f => f.SetValue(0))
    //                                          //   .LinkedState(meleeAttackState),

    //                                          //new GOAPAction("Range Attack")
    //                                          //    .Pre(StatesGoap.hasBullets, f => f.GetValue<bool>() == true)
    //                                          //    .Effect(StatesGoap.isPlayerAlive, f => f.SetValue(false))
    //                                          //    .Effect(StatesGoap.hasBullets, f => f.SetValue(0))
    //                                          //    .LinkedState(rangeAttack),

    //                                          //new GOAPAction("Launch Grenade")
    //                                          //    .Pre(StatesGoap.isPlayerBehindWall,f => f.GetValue<bool>() == true)
    //                                          //    .Pre(StatesGoap.isPlayerInSight, f => f.GetValue<bool>() == false)
    //                                          //    .Effect(StatesGoap.isPlayerAlive, f => f.SetValue(0))
    //                                          //    .LinkedState(throwGrenade)
    //                                      };

    //    var from = new GOAPState();
    //    from.values[StatesGoap.isPlayerInSight] = new PlanerValue(InSight());
    //    from.values[StatesGoap.isPlayerNear] = new PlanerValue(IsNear());
    //    from.values[StatesGoap.isPlayerAlive] = new PlanerValue(PlayerAlive>0);
    //    from.values[StatesGoap.hasBullets] = new PlanerValue(hasBullet);
    //    from.values[StatesGoap.itemsAvaiable] = new PlanerValue(AmmoAvaiable);
    //    from.values[StatesGoap.isPlayerBehindWall] = new PlanerValue(BehindWall());


    //    var to = new GOAPState();

    //    to.values[StatesGoap.isPlayerAlive] = new PlanerValue(false);



    //    var planner = new GoapPlanner(StartCoroutine);
    //    planner.Run(from, to, actions);
    //    planner.OnPathCreated += ConfigureFsm;


    //}
    private void Update()
    {

        // Debug.Log($"InSight: {InSight()} |IsNear: {IsNear()} | PlayerAlive: {PlayerAlive} |hasBullet:  {hasBullet} | AmmoAvaiable: {AmmoAvaiable} | BehindWall: {BehindWall()}");


        if (GetLife <= 0)
        {
            _fsm.Active = false;
            _fsm.Clear();
            Notify("Death");
        }

        NearestAmmo(transform);
    }
    //private void OnReplan() {
    //    if (Time.time >= _lastReplanTime + _replanRate) {
    //        _lastReplanTime = Time.time;
    //    }
    //    else {
    //        return;
    //    }

    //    item = NearestAmmo(transform);



    //    var actions = new List<GOAPAction> {
    //                                          new GOAPAction("Patrol")
    //                                             .Effect(StatesGoap.isPlayerInSight, f => f.SetValue(true))
    //                                             .Effect(StatesGoap.isPlayerAlive, f => f.SetValue(true))
    //                                             .LinkedState(lookState)
    //                                             .Cost(1),

    //                                          new GOAPAction("Chase")
    //                                             //.Pre(StatesGoap.isPlayerInSight, f => f.GetValue<bool>() == true)
    //                                             //.Effect(StatesGoap.isPlayerNear, f => f.SetValue(0f))
    //                                             .Effect(StatesGoap.isPlayerAlive, f => f.SetValue(true))
    //                                             .LinkedState(chaseState)
    //                                             .Cost(5),

    //                                          //new GOAPAction("Pick Ammo")
    //                                          //    .Pre(StatesGoap.isPlayerInSight, f => f.GetValue<bool>() == true)
    //                                          //    .Pre(StatesGoap.itemsAvaiable, f =>f.GetValue<string>() == "SI")
    //                                          //    .Pre(StatesGoap.hasBullets, f => f.GetValue<bool>() == false)
    //                                          //    .Effect(StatesGoap.hasBullets, f => f.SetValue(true))
    //                                          //    .Effect(StatesGoap.itemsAvaiable, f => f.SetValue(false))
    //                                          //    .LinkedState(pickAmmo)
    //                                          //    .Cost(1+distNearItem(item)),

    //                                          //new GOAPAction("Melee Attack")
    //                                          //   .Pre(StatesGoap.isPlayerNear, f => f.GetValue<float>() < distanceToAttack)
    //                                          //   .Effect(StatesGoap.isPlayerAlive, f => f.SetValue(0))
    //                                          //   .LinkedState(meleeAttackState),

    //                                          //new GOAPAction("Range Attack")
    //                                          //    .Pre(StatesGoap.hasBullets, f => f.GetValue<bool>() == true)
    //                                          //    .Effect(StatesGoap.isPlayerAlive, f => f.SetValue(0))
    //                                          //    .Effect(StatesGoap.hasBullets, f => f.SetValue(false))
    //                                          //    .LinkedState(rangeAttack),

    //                                          //new GOAPAction("Launch Grenade")
    //                                          //    .Pre(StatesGoap.isPlayerBehindWall,f => f.GetValue<bool>() == true)
    //                                          //    .Pre(StatesGoap.isPlayerInSight, f => f.GetValue<bool>() == false)
    //                                          //    .Effect(StatesGoap.isPlayerAlive, f => f.SetValue(0))
    //                                          //    .LinkedState(throwGrenade)


    //                                          /// ////////////////////
    //                                          /// ////////////////////
    //                                          /// ////////////////////


    //                                          //new GOAPAction("Pick Ammo")
    //                                          //    .Pre(StatesGoap.isPlayerInSight,true)
    //                                          //    .Pre(StatesGoap.itemsAvaiable,true)
    //                                          //    .Effect(StatesGoap.hasBullets, true)
    //                                          //    .Effect(StatesGoap.itemsAvaiable,false)
    //                                          //    .LinkedState(pickAmmo)
    //                                          //    .Cost(1+distNearItem(item)),

    //                                          //new GOAPAction("Melee Attack")
    //                                          //   .Pre(StatesGoap.isPlayerNear,   true)
    //                                          //   .Effect(StatesGoap.isPlayerAlive, false)
    //                                          //   .LinkedState(meleeAttackState),

    //                                          //new GOAPAction("Range Attack")
    //                                          //.Pre(StatesGoap.hasBullets, true)
    //                                          //.Effect(StatesGoap.isPlayerAlive, false)
    //                                          //.LinkedState(rangeAttack),

    //                                          //new GOAPAction("Launch Grenade")
    //                                          //.Pre(StatesGoap.isPlayerBehindWall,true)
    //                                          //.Pre(StatesGoap.isPlayerInSight,false)
    //                                          //.Effect(StatesGoap.isPlayerAlive,false)
    //                                          //.LinkedState(throwGrenade)
    //                                      };

    //    var from = new GOAPState();
    //    from.values[StatesGoap.isPlayerInSight] = new PlanerValue(InSight());
    //    from.values[StatesGoap.isPlayerNear] = new PlanerValue(IsNear());
    //    from.values[StatesGoap.isPlayerAlive] = new PlanerValue(PlayerAlive>0);
    //    from.values[StatesGoap.hasBullets] = new PlanerValue(hasBullet);
    //    from.values[StatesGoap.itemsAvaiable] = new PlanerValue(AmmoAvaiable);
    //    from.values[StatesGoap.isPlayerBehindWall] = new PlanerValue(BehindWall());

    //    var to = new GOAPState();

    //    to.values[StatesGoap.isPlayerAlive] = new PlanerValue(true);


    //    var planner = new GoapPlanner(StartCoroutine);
    //    planner.Run(from, to, actions);
    //    planner.OnPathCreated += ConfigureFsm;
    //}
    private void OnReplan()
    {
        if (Time.time >= _lastReplanTime + _replanRate)
        {
            _lastReplanTime = Time.time;
        }
        else
        {
            return;
        }

        
        Debug.Log("Replan");
        
        var actions = new List<GOAPAction>();
        var from = new GOAPState();
        var to = new GOAPState();

        DoPlanning(out actions, out from, out to);

        var planner = new GoapPlanner();

        var txtActions = "Replan:  ";
        foreach (var item in actions)
        {
            txtActions += $"{item.name}|";
        }
        //Debug.Log(txtActions);

        planner.Run(from, to, actions, StartCoroutine, goapToPlan);

    }
    //private void ConfigureFsm(IEnumerable<GOAPAction> plan)
    //{
    //    Debug.Log("Completed Plan");

    //    _fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine, _fsm);
    //    _fsm.Active = true;
    //}
    private void ConfigureFsm(IEnumerable<GOAPAction> plan)
    {
        //Debug.Log("Completed Plan");
        if (_fsm != null)
        {
            _fsm.Active = false;
        }
        _fsm = GoapPlanner.ConfigureFSM(plan, StartCoroutine);
        _fsm.Active = true;
    }
    public GameObject NearestAmmo(Transform myPosition)
    {
        List<GameObject> childs = new List<GameObject>();
        for (int i = 0; i < itemContenedor.transform.childCount; i++)
        {
            childs.Add(itemContenedor.transform.GetChild(i).gameObject);
        }
        if (childs.Count() <= 0)
            return default(GameObject);

        var item = childs.Where(x => x.activeSelf).OrderBy(x => Vector3.Distance(x.transform.position, myPosition.position))
                    .ToArray()
                    .FirstOrDefault();

        return item;
    }
    //public string AmmoAvaiable => itemContenedor.transform.childCount > 0 ? "SI" : "NO";
    public bool AmmoAvaiable => itemContenedor.transform.childCount > 0;

    
    bool PlayerAlive => player.GetComponent<PlayerModel>().currentLife > 0;
    public bool IsNear()
    {
        if (player == null) return false;
        return (transform.position - player.position).magnitude < 3;
    }


    public override void Damaged(float damage)
    {
        currentlife -= damage;
        if (currentlife <= 0)
        {
            Debug.Log("mori");
        }
        else
        {
            Notify("Damaged");
        }
    }
}




public class PlanerValue
{
    object value;

    public PlanerValue(object value)
    {
        this.value = value;
    }

    public T GetValue<T>()
    {
        return (T)value;
    }

    public PlanerValue Clone()
    {
        return new PlanerValue(value);

    }
    public void SetValue<T>(T newValue)
    {
        // Debug.Log($"GetValue : {newValue} {newValue.GetType()}");
        value = newValue;
    }
}