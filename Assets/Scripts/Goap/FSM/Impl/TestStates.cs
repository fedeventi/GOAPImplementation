using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStates : MonoBehaviour
{
    // Start is called before the first frame update
    public List<MonoBaseState> estados = new List<MonoBaseState>();
    int currentState=1;
    
    
    void Start()
    {
       
       
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (currentState < estados.Count) currentState++;
            else currentState = 0;
            Debug.Log(estados[currentState]);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (currentState > 0) currentState--;
            else currentState = estados.Count-1;
            Debug.Log(estados[currentState]);
        }
        estados[currentState].UpdateLoop();
        Debug.Log(estados[currentState].name);
    }
   
}
