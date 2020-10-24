using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Target : MonoBehaviour
{
    public Transform Agent;
    

    public void Collect()
    {
        Respawn();
    }

    private void Respawn()
    {
        int size = ArenaScaler.instance.Size;
        Vector3 newPos = new Vector3(Random.Range(-size, size), 0, Random.Range(-size, size));
        
        if(newPos == Agent.position) Respawn();
        else transform.position = newPos;
    }
}
