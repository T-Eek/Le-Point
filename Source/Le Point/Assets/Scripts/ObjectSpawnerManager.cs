using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnerManager : MonoBehaviour
{

    
    public GameObject TargetL; // The object prefab to spawn initially untile the next target spawns
    public GameObject TargetM; // The object prefab to spawn after reaching a score of 25
    public GameObject TargetS; // The object prefab to spawn after reaching a score of 50
    private int targetCount; // each target GameObject
    public int maxObjects = 1; // Default maximum number of spawned objects
    public static int maxObjectsTagetL = 10; // Maximum number of spawned objects for TargetL
    public static int maxObjectsTargetM = 10; // Maximum number of spawned objects for TargetM
    public static int maxObjectsTargetS = 5; // Maximum number of spawned objects for TargetS
    private float spawnDelay = 0.5f; // Initial default spawn delay between the spawned target GameObjects
    private float decayTime = 3.5f; // The set time when all target GameObjects are destoryed to show the Game Over Screen
    public static float decayTargetTime = 1f; // The time before spawned objects are being destroyed

    private Transform objectsContainer; // Container to store spawned objects (set in the Unity Editor)
    public GameObject GameOverScreen;

    public GameObject spawnArea;
    public GameObject spawnArea1;
    public GameObject teleporter;

    private float maxObjectsTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {

        // Create a new GameObject as the container
        objectsContainer = new GameObject("LeTargetContainer").transform;

        // Start spawning objects
        StartCoroutine(TargetDrop());

        // Changes the GameState
        GameStateManager.Instance.OnGameStateChanged += onGameStateChanged;
    }

        // Destroys the previous GameState
    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= onGameStateChanged;
    }

    private void Update()
    {
        //
    }

    IEnumerator TargetDrop()
    {
        while (true)
        {
            if (GameStateManager.Instance.CurrentGameState == GameState.Gameplay)
            {
                if (targetCount < maxObjects)
                {
                    // Determine the object to spawn based on the player's score
                    GameObject spawnedObject = GetSpawnedObject();

                    // If GetSpawnedObject returns null, stop spawning targets
                    if (spawnedObject == null)
                    {
                        yield break;
                    }

                    // Get a random position within the specified range using TargetArea from AreaSpawner
                    Vector3 spawnPosition = AreaSpawner.Instance.TargetArea();

                    // Instantiate and set the container as the parent
                    GameObject instantiatedObject = Instantiate(spawnedObject, spawnPosition, Quaternion.identity);
                    instantiatedObject.transform.parent = objectsContainer;

                    targetCount++;
                    //Debug.Log($"Spawned: {spawnedObject.name} {""} {targetCount} at position: {spawnPosition}");


                    // Change spawn delay based on the spawned object
                    if (spawnedObject == TargetM)
                    {
                        spawnDelay = 0.8f;
                        StartCoroutine(DestroyAfterDelay(instantiatedObject, decayTime));

                    }
                    else if (spawnedObject == TargetS)
                    {
                        spawnDelay = 0.4f;
                        StartCoroutine(DestroyAfterDelay(instantiatedObject, decayTime));
                    }
                    else
                    {
                        spawnDelay = 1.5f; // Reset to default if spawning TargetL
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
                GameOverScreen.SetActive(false);
            }
        }
    }

    IEnumerator DestroyAfterDelay(GameObject target, float delay)
    {

        Renderer targetRenderer = target.GetComponent<Renderer>();

        // Change to custom color (#FF5733) for the first second
        targetRenderer.material.color = ColorUtility.TryParseHtmlString("#FFFFFF", out Color targetColor1) ? targetColor1 : Color.white;
        yield return new WaitForSeconds(spawnDelay);

        // Change to another custom color (#33FF57) for the second second
        targetRenderer.material.color = ColorUtility.TryParseHtmlString("#737373", out Color targetColor2) ? targetColor2 : Color.white;
        yield return new WaitForSeconds(spawnDelay);

        // Change to another custom color (#5733FF) for the third second
        targetRenderer.material.color = ColorUtility.TryParseHtmlString("#393939", out Color targetColor3) ? targetColor3 : Color.white;
        yield return new WaitForSeconds(spawnDelay);

        // Check if the object is null before attempting to deactivate and destroy it
        if (target != null)
        {
            target.SetActive(false);

            // Start a coroutine to destroy the object after a delay of 1 second
            StartCoroutine(DestroyObjectWithDelay(1f));
        }

        // Coroutine to destroy the object after a delay
        IEnumerator DestroyObjectWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Destroy(target);
        }
    }

    GameObject GetSpawnedObject()
    {
        if (ScoreManager.scoreCount >= 25)
        {
            Debug.Log("Score is 25 or higher, no targets will spawn.");

            Debug.Log("Teleporter is active");
            teleporter.SetActive(true); // Activate the teleporter

            return null;
        }

        // Your existing logic for selecting targets based on score
        if (ScoreManager.scoreCount >= 20 && TargetS != null)
        {
            Debug.Log("Selected TargetS");
            maxObjects = maxObjectsTargetS; // Change maxObjects for TargetS

            return TargetS;
        }
        else if (ScoreManager.scoreCount >= 10 && TargetM != null)
        {
            Debug.Log("Selected TargetM");
            maxObjects = maxObjectsTargetM; // Change maxObjects for TargetM

            return TargetM;
        }
        else
        {
            Debug.Log("Selected TargetL");
            maxObjects = maxObjectsTagetL; // Reset maxObjects of maxObjectsTargetL

            Debug.Log("Teleporter is not active");
            teleporter.SetActive(false); // Deactivate the teleporter

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

    private void onGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}