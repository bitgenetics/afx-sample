using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCube : MonoBehaviour
{

    public GameObject objectToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.ExampleEvent += Spawn;
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
        EventManager.ExampleEvent -= Spawn;
    }
}
