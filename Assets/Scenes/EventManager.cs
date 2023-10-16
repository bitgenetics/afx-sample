using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action ExampleEvent;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("button down");
            //if(ExampleEvent != null)
            //{
            //    ExampleEvent();
            //}
            ExampleEvent?.Invoke();
        }
    }

    public static void TriggerFx()
    {

    }

}
