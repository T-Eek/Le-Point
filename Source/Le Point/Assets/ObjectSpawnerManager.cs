using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnerManager : MonoBehaviour
{
    public GameObject TheDot; // The object prefab to spawn
    public int xPos;
    public int yPos;

    public int dotCount;
    public int maxObjects = 5; // Maximum number of spawned objects allowed

    // Start is called before the first frame update
    void Start()
    {
        // Start spawning objects
        StartCoroutine(DotDrop());
    }

    private void Update()
    {
        //
    }

    IEnumerator DotDrop()
    {
        while (true) // Infinite loop to keep spawning objects
        {
            if (dotCount < maxObjects)
            {
                xPos = Random.Range(-3, 3);
                yPos = Random.Range(2, 6);
                Vector3 spawnPosition = new Vector3(xPos, yPos, 11);
                Instantiate(TheDot, spawnPosition, Quaternion.identity);
                dotCount++;
                Debug.Log("Spawned " + TheDot + dotCount + " at position: " + spawnPosition);
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