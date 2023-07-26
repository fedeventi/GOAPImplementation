using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    // Start is called before the first frame update
    public float speed;
    void Start()
    {
        _damage = 40;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 || other.gameObject.layer == 9 || other.gameObject.layer == 10)
        {
            Destroy(gameObject);
        }
    }
}
