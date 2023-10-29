using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A timed trigger behavior that changes object color and calls the SwitchActivated event.
/// </summary>
public class Trigger : MonoBehaviour
{
    public float delayTime = 5f;


    // Start is called before the first frame update
    protected bool bEnabled = true;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// enable trigger after a timed delay
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayedEnable()
    {
        yield return new WaitForSeconds(delayTime);
        bEnabled = true;
        this.gameObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1);

       
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
        
        this.gameObject.GetComponent<Renderer>().material.color = new Color(1, 0, 0);

        StartCoroutine(DelayedEnable());
        EventManager.EmitSwitchActivated();
    }
}
