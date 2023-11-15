using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    private ObjectController spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("TheDot").GetComponent<ObjectController>();
    }

    void OnDestroy()
    {
        spawnManager.ObjectDestroyed(gameObject);
    }

    private void ObjectDestroyed(GameObject gameObject)
    {
        throw new NotImplementedException();
    }
}
