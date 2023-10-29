using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple cube with a life span
/// </summary>
public class Cube : MonoBehaviour
{
    public long lifeTimeInSeconds = 5;
    private long _startTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        this._startTime = new System.DateTimeOffset(System.DateTime.Now).ToUnixTimeSeconds();

    }

    // Update is called once per frame
    void Update()
    {
        long cur = new System.DateTimeOffset(System.DateTime.Now).ToUnixTimeSeconds();
        if(cur - _startTime > lifeTimeInSeconds)
        {
            Destroy(this.gameObject);
        }
    }
}
