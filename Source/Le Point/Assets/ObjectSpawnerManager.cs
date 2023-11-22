using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnerManager : MonoBehaviour
{
    public GameObject Object; // The object prefab to spawn
    public int xPos;
    public int yPos;

    public int dotCount;
    public int maxObjects = 5; // Maximum number of spawned objects allowed

    private Transform objectsContainer; // Container to store spawned objects (set in the Unity Editor)


    // Start is called before the first frame update
    void Start()
    {
        objectsContainer = new GameObject("LeTargetContainer").transform; // Create a new GameObject as the container

        // Start spawning objects
        StartCoroutine(TargetDrop());
    }

    private void Update()
    {
        //
    }

    IEnumerator TargetDrop()
    {
        while (true) // Infinite loop to keep spawning objects
        {
            if (dotCount < maxObjects)
            {
                xPos = Random.Range(-3, 3);
                yPos = Random.Range(2, 6);
                Vector3 spawnPosition = new Vector3(xPos, yPos, 11);
                GameObject spawnedObject = Instantiate(Object, spawnPosition, Quaternion.identity);

                // Set the container as the parent of the spawned object
                spawnedObject.transform.parent = objectsContainer;

                dotCount++;
                Debug.Log("Spawned: Le Target" + Object + dotCount + " at position: " + spawnPosition);
            }
            yield return new WaitForSeconds(0.5f); // Delay between object spawns
        }
    }

    // Call this method when the player interacts with an object
    public void OnObjectHit()
    {
        if (dotCount > 0)
        {
            dotCount--;
        }
    }
}