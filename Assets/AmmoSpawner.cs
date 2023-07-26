using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public Ammo ammo;
    ObjPool<Ammo> pool;
    public float timeToRespawn;
    float _timeMax;
    int position;
    Vector3[] positions = new Vector3[4] { new Vector3(-15.75f,0, 26.85f),
                                           new Vector3(-13.1f,0, -12.3f),
                                           new Vector3(15.2f,0, -12.5f),
                                           new Vector3(12.57f,0,21.72f) };
    public void Recycle(Ammo ammo)
    {
        pool.Recycle(ammo);
    }
    public Ammo Factory()
    {
        return Instantiate(ammo, transform.position, Quaternion.identity,transform);
    }
    private void Start()
    {
        pool = new ObjPool<Ammo>(Factory, Ammo.TurnOnStatic, Ammo.TurnOffStatic, 0, true);
        _timeMax = timeToRespawn;
        position=Random.Range(0,4);
    }
    private void Update()
    {
            timeToRespawn -= Time.deltaTime;
        if (transform.childCount <= positions.Length-1)
        {
            if (timeToRespawn <= 0)
            {
                pool.GetObj().SetPosition(positions[position]);
                position = position++ >= positions.Length - 1 ? 0 : position++;
                timeToRespawn = _timeMax;
            }
        }
    }
}
