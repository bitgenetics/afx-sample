using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustFx : MonoBehaviour
{

    Rigidbody m_Rigidbody;


    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyImpluse()
    {
        m_Rigidbody.AddForce(new Vector3(0, 5, 5), ForceMode.Impulse);
    }
}
