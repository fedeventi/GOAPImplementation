using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;


public class SpawnEnemies : MonoBehaviour, IPoolGenerator<Enemy>
{
    ObjPool<Enemy> poolPrisioner;
    ObjPool<Enemy> poolProsti;
    ObjPool<Enemy> poolClown;
    public Enemy prisioner, prosti, clown;
    Transform player;
    public Transform[] pos;
    float currentTime; 
    public TMP_Text info,debug;
    public float Cooldown;
    public int typesEnemis;
    public GameObject boss;
    ObjPool<Enemy>[] pools ;
    int[] numbers = new int[3] { 1, 2, 3 };
    int[] cantEnemies = new int[3] {4,8, 16};
    int currentWave;
    bool bossArrived;

    [Header("cantindad de enemigos inicial")]
    public int initialEnemies;


    public List<Enemy> lst = new List<Enemy>();
    public List<Enemy> lst2 = new List<Enemy>();


    private void Start()
    {
        currentWave = 1;
    }

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>().transform;
        poolPrisioner = new ObjPool<Enemy>(FactoryPrisioner, Enemy.TurnOnStatic, Enemy.TurnOffStatic, 3, true);
        poolClown = new ObjPool<Enemy>(FactoryClown, Enemy.TurnOnStatic, Enemy.TurnOffStatic, 3, true);
        poolProsti = new ObjPool<Enemy>(FactoryProsti, Enemy.TurnOnStatic, Enemy.TurnOffStatic, 3, true);
        pools = new ObjPool<Enemy>[3] { poolPrisioner, poolClown, poolProsti };
        
    }

    private void Update()
    {
        info.text = $"Wave : {currentWave} \n Enemigos vivos : {EnemiesAmount} \n Enemigos muertos : {EnemiesDieAmount} \n Muertos oleada anterior : { lst2.Count }";
        currentTime += Time.deltaTime;
        Waves();
    }


    void Waves()
    {

        if (currentWave > EnemiesForWave().Count)
        {   if(!bossArrived)
                Instantiate(boss, transform.position, Quaternion.identity, transform);
            bossArrived = true;
            return;
        }

        if (EnemiesAmount + EnemiesDieAmount < EnemiesForWave()[currentWave] && currentWave <= EnemiesForWave().Count) 
        {
            if (Cooldown - currentTime < 0)
            {
                for (int i = 0; i < EnemiesForWave()[currentWave] / 2; i++)
                {
                    EnemiesDictionary()[Random.Range(1, typesEnemis)].GetObj()
                                      .SetPosition(pos[Random.Range(0, 5)].position)//IA2-P3
                                      .SetPlayer(player)
                                      .SetWaveID(currentWave)
                                      .SetLife();
                }
                currentTime = 0;
            }
          
        }
        else
        {
            if (EnemiesDieAmount >= EnemiesForWave()[currentWave])
            {
                lst2.AddRange(TotalEnemidKill());
                ClearEnemies();
                currentWave += 1;
            }

        }
    }
    Dictionary<int,ObjPool<Enemy>> EnemiesDictionary()
    {
        var poolDic = numbers.Zip(pools, (key, value) => new { key, value }).ToDictionary(x => x.key, x => x.value);
        return poolDic;
    }
    Dictionary<int, int> EnemiesForWave()
    {
        var wavesEnemies = numbers.Zip(cantEnemies, (key, value) => new { key, value }).ToDictionary(x => x.key, x => x.value);
        return wavesEnemies;
    }
    private List<Enemy> TotalEnemidKill()
    {
        var x = FindObjectsOfType<Enemy>().Where(enemy => enemy.GetLife <= 0 && enemy.GetWaveID == currentWave);//IA2-P3
        return lst.Concat(x).ToList();

    }


    public void ClearEnemies()
    {
        foreach (var enemy in FindObjectsOfType<Enemy>())
        {
            
            Recycle(enemy);
        }
    }

    public int EnemiesAmount
    {
        get
        {
            return FindObjectsOfType<Enemy>().Where(enemy => enemy.GetLife > 0 && enemy.GetWaveID == currentWave).ToList().Count;//IA2-P3

        }
    }

    public int EnemiesDieAmount
    {
        get
        {                     
           return FindObjectsOfType<Enemy>().Where(enemy => enemy.GetLife <= 0 && enemy.GetWaveID == currentWave).ToList().Count;//IA2-P3
        }
    }



    public void Recycle(Enemy obj)
    {
        poolPrisioner.Recycle(obj);
    }


    public Enemy FactoryPrisioner()
    {
        return Instantiate(prisioner, transform.position, Quaternion.identity,transform).SetPlayer(player);
    }

    public Enemy FactoryProsti()
    {
        return Instantiate(prosti, transform.position, Quaternion.identity, transform).SetPlayer(player);
    }

    public Enemy FactoryClown()
    {
        return Instantiate(clown, transform.position, Quaternion.identity, transform).SetPlayer(player);
    }

    public Enemy Factory()
    {
        throw new System.NotImplementedException();
    }
}
