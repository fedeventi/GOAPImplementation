using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CameraMove : MonoBehaviour
{
    PlayerController _tgt;
    public Vector3 offset;

    private void Awake()
    {
        _tgt = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        //transform.LookAt(_tgt.transform);
        transform.position = new Vector3(_tgt.transform.position.x + offset.x, offset.y, _tgt.transform.position.z + offset.z);
        //transform.rotation =  new Quaternion(transform.rotation.x,_tgt.transform.rotation.y,transform.rotation.z,transform.rotation.w);
    }
}
