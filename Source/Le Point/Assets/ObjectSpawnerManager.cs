using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnerManager : MonoBehaviour
{

    
    public GameObject TargetL; // The object prefab to spawn initially
    public GameObject TargetM; // The object prefab to spawn after reaching a score of 25
    public GameObject TargetS; // The object prefab to spawn after reaching a score of 50
    public int maxObjects = 5; // Default maximum number of spawned objects
    public int maxObjectsTargetM = 5; // Maximum number of spawned objects for TargetM
    public int maxObjectsTargetS = 2; // Maximum number of spawned objects for TargetS
    public float spawnDelay = 0.5f; // Initial default spawn delay
    private float decayTime = 3.01f; // Time before spawned objects are destroyed

    private Transform objectsContainer; // Container to store spawned objects (set in the Unity Editor)
    private int targetCount;
    public GameObject GameOverScreen;

    private float maxObjectsTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {

        // Create a new GameObject as the container
        objectsContainer = new GameObject("LeTargetContainer").transform;

        // Start spawning objects
        StartCoroutine(TargetDrop());
    }

    IEnumerator TargetDrop()
    {
        while (true)
        {
            if (GameStateManager.Instance.CurrentGameState == GameState.Gameplay)
            {
                if (targetCount < maxObjects)
                {
                    // Get a random position within the specified range
                    Vector3 spawnPosition = GetRandomSpawnPosition();

                    // Determine the object to spawn based on the player's score
                    GameObject spawnedObject = GetSpawnedObject();

                    // Instantiate and set the container as the parent
                    GameObject instantiatedObject = Instantiate(spawnedObject, spawnPosition, Quaternion.identity);
                    instantiatedObject.transform.parent = objectsContainer;

                    targetCount++;
                    Debug.Log($"Spawned: {spawnedObject.name} {""} {targetCount} at position: {spawnPosition}");


                    // Change spawn delay based on the spawned object
                    if (spawnedObject == TargetM)
                    {
                        spawnDelay = 2.5f;
                        StartCoroutine(DestroyAfterDelay(instantiatedObject, decayTime));

                    }
                    else if (spawnedObject == TargetS)
                    {
                        spawnDelay = 5f;
                        StartCoroutine(DestroyAfterDelay(instantiatedObject, decayTime));
                    }
                    else
                    {
                        spawnDelay = 1f; // Reset to default if spawning TargetL
                        StartCoroutine(DestroyAfterDelay(instantiatedObject, decayTime));
                    }
                }
            }
            yield return new WaitForSeconds(spawnDelay);
            // Update maxObjectsTimer if maxObjects is reached
            if (targetCount == maxObjects)
            {
                maxObjectsTimer += spawnDelay;
                if (maxObjectsTimer >= 5f)
                {
                    GameOverScreen.SetActive(true);
                    yield break; // exit the coroutine
                }
            }
            else
            {
                maxObjectsTimer = 0f; // Reset the timer if targetCount is less than maxObjects
            }
        }
    }

    IEnumerator DestroyAfterDelay(GameObject target, float delay)
    {

        Renderer targetRenderer = target.GetComponent<Renderer>();

        // Change to custom color (#FF5733) for the first second
        targetRenderer.material.color = ColorUtility.TryParseHtmlString("#FFFFFF", out Color targetColor1) ? targetColor1 : Color.white;
        yield return new WaitForSeconds(1f);

        // Change to another custom color (#33FF57) for the second second
        targetRenderer.material.color = ColorUtility.TryParseHtmlString("#737373", out Color targetColor2) ? targetColor2 : Color.white;
        yield return new WaitForSeconds(1f);

        // Change to another custom color (#5733FF) for the third second
        targetRenderer.material.color = ColorUtility.TryParseHtmlString("#393939", out Color targetColor3) ? targetColor3 : Color.white;
        yield return new WaitForSeconds(1f);

        // Check if the object is null before attempting to destroy it
        if (target != null)
        {
            Destroy(target);
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        float xPos = Random.Range(-3, 3);
        float yPos = Random.Range(4, 8);
        float zPos = Random.Range(8, 2);

        return new Vector3(xPos, yPos, zPos);
    }

    GameObject GetSpawnedObject()
    {

        if (ScoreManager.scoreCount >= 50 && TargetS != null)
        {
            Debug.Log("Selected TargetS");
            maxObjects = maxObjectsTargetS; // Change maxObjects for TargetS

            return TargetS;
        }
        else if (ScoreManager.scoreCount >= 30 && TargetM != null)
        {
            Debug.Log("Selected TargetM");
            maxObjects = maxObjectsTargetM; // Change maxObjects for TargetM
            return TargetM;
        }
        else
        {
            Debug.Log("Selected TargetL");
            maxObjects = 10; // Reset maxObjects to default for TargetL
            return TargetL;
        }
    }

    // Call this method when the player interacts with an object
    public void OnObjectHit()
    {
        if (targetCount > 0)
        {
            targetCount--;
        }
    }
}