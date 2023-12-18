using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSpawner : MonoBehaviour
{

    public Vector3 center;
    public Vector3 size;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(2, 0, 0, 0.5f);
        Gizmos.DrawCube(center, size);
    }
}
