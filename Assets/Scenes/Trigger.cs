using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    bool bEnabled = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DelayedEnable()
    {
        yield return new WaitForSeconds(5f);// Wait for one second
        bEnabled = true;
       
    }

    void OnCollisionEnter(Collision collision)
    {
        var collider = collision.collider;
        collider.gameObject.GetComponent<ThrustFx>()?.ApplyImpluse();

        if (!bEnabled)
        {
            Debug.Log("OnCollisionEnter - trigger disabled");
            return;
        }
        Debug.Log("OnCollisionEnter - trigger enabled");
        bEnabled = false;
        StartCoroutine(DelayedEnable());
        EventManager.TriggerFx();
    }
}
