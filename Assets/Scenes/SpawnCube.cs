using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Cube Spawn emitter
/// </summary>
public class SpawnCube : MonoBehaviour
{

    public GameObject objectToSpawn;
    private long _startTime;
    // Start is called before the first frame update
    void Start()
    {
        this._startTime = new System.DateTimeOffset(System.DateTime.Now).ToUnixTimeSeconds();
        EventManager.DropBox += Spawn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Spawn()
    {


        var obj = Instantiate(objectToSpawn,this.transform);
       
    }
    private void OnDisable()
    {
        EventManager.DropBox -= Spawn;
    }
}
