using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    private PlayerModel _playerModel;   

    private void Awake()
    {
        _playerModel= GetComponent<PlayerModel>();
    }
    private void Update()
    {        

        Ray cameraRay = _playerModel.Camera.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if(ground.Raycast(cameraRay,out rayLength) && Time.timeScale > 0)
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);
            _playerModel.LookAt(new Vector3(pointToLook.x,transform.position.y,pointToLook.z));
            
        }

        if (Input.GetMouseButton(0))       
            _playerModel.IsShooting = true;        
        else
        {
            _playerModel.IsShooting = false;
        }
        if (Time.timeScale > 0)
        {
            _playerModel.Shoot();
            _playerModel.Deadth();
        }

    }


    private void FixedUpdate()
    {
        if(_playerModel.isDeadth == false && Time.timeScale > 0)
            _playerModel.Move(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        
    }

}
