using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class ObjectSpawnerManager : MonoBehaviour
{
    [SerializeField] GameObject[] Targets;
    [SerializeField] GameObject[] spawnAreas;

    private int targetCount; // each target GameObject
    public int maxObjects = 0; // Default maximum number of spawned objects
    public static int maxObjectsTargetL = 25; // Maximum number of spawned objects for TargetL
    public static int maxObjectsTargetM = 15; // Maximum number of spawned objects for TargetM
    public static int maxObjectsTargetS = 10; // Maximum number of spawned objects for TargetS
    private float spawnDelay = 0.5f; // Initial default spawn delay between the spawned target GameObjects
    private float decayTime = 3.5f; // The set time when all target GameObjects are destoryed to show the Game Over Screen
    private float maxObjectsTimer = 0f;
    public static float decayTargetTime = 1f; // The time before spawned objects are being destroyed

    private Transform objectsContainer; // Container to store spawned objects (set in the Unity Editor)
    [SerializeField] GameObject GameOverScreen;
    [SerializeField] GameObject teleporter;

    //private Movement playerMovement;

    // Start is called before the first frame update
    void Start()
    {

        // Create a new GameObject as the container
        objectsContainer = new GameObject("LeTargetContainer").transform;

        // Start spawning objects
        StartCoroutine(targetSpawner());

        // Changes the GameState
        GameStateManager.Instance.OnGameStateChanged += onGameStateChanged;

        // Assuming the Movement script is on the player object
        /*playerMovement = FindObjectOfType<Movement>();
        if (playerMovement == null)
        {
            Debug.LogError("Movement script not found on player object.");
        }*/
    }

    // Destroys the previous GameState
    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= onGameStateChanged;
    }

    IEnumerator targetSpawner()
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
                    Vector3 spawnPosition = GetSpawnAreas();

                    // Instantiate and set the container as the parent
                    GameObject instantiatedObject = Instantiate(spawnedObject, spawnPosition, Quaternion.identity);
                    instantiatedObject.transform.parent = objectsContainer;

                    targetCount++;
                    //Debug.Log($"Spawned: {spawnedObject.name} {""} {targetCount} at position: {spawnPosition}");


                    // Change spawn delay based on the spawned object
                    if (spawnedObject == Targets[0] && ScoreManager.scoreCount == 5)
                    {
                        spawnDelay = 0.8f;
                        StartCoroutine(DestroyAfterDelay(instantiatedObject, decayTime));

                    }
                    else if (spawnedObject == Targets[0] && ScoreManager.scoreCount == 15)
                    {
                        spawnDelay = 0.4f;
                        StartCoroutine(DestroyAfterDelay(instantiatedObject, decayTime));
                    }
                    else if (spawnedObject == Targets[1] && ScoreManager.scoreCount == 30)
                    {
                        spawnDelay = 0.8f;
                        StartCoroutine(DestroyAfterDelay(instantiatedObject, decayTime));
                    }
                    else if (spawnedObject == Targets[1] && ScoreManager.scoreCount == 40)
                    {
                        spawnDelay = 0.3f;
                        StartCoroutine(DestroyAfterDelay(instantiatedObject, decayTime));
                    }
                    else
                    {
                        spawnDelay = 1.5f; // Reset to default if spawning Targets
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
            yield return new WaitForSeconds(delay);
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

    // Function to determine the spawned target based on the player's score
    GameObject GetSpawnedObject()
    {
        if (ScoreManager.scoreCount >= 100)
        {
            Debug.Log("Selected TargetL");
            Debug.Log("Teleporter is active");
            teleporter.SetActive(true); // Activate the teleporter
            //playerMovement.EnableMovement();
            Debug.Log("Score is 100 or higher, no targets will spawn.");
            return null;
        }
        else if (ScoreManager.scoreCount >= 25)
        {
            Debug.Log("Selected TargetL");
            Debug.Log("Teleporter is active");
            teleporter.SetActive(true); // Activate the teleporter
            Debug.Log("Score is 25 or higher, TargetM will spawn.");
            maxObjects = maxObjectsTargetM; // Reset maxObjects of maxObjectsTargetM
            return Targets[1];
        }
        else if (ScoreManager.scoreCount >= 50) // Deactivates the teleporter
        {
            Debug.Log("Selected TargetL");
            Debug.Log("Teleporter is active");
            teleporter.SetActive(true); // Activate the teleporter
            Debug.Log("Score is 50 or higher, TargetS will spawn.");
            maxObjects = maxObjectsTargetS; // Reset maxObjects of maxObjectsTargetS
            return Targets[2];
        }
        // Your existing logic for selecting targets based on score
        /*if (ScoreManager.scoreCount >= 20 && TargetL != null)
        {
            Debug.Log("Selected TargetS");
            maxObjects = maxObjectsTargetL; // Change maxObjects for TargetS

            return TargetL;
        }
        else if (ScoreManager.scoreCount >= 10 && TargetL != null)
        {
            Debug.Log("Selected TargetM");
            maxObjects = maxObjectsTargetL; // Change maxObjects for TargetM

            return TargetL;
        }*/
        else
        {
            Debug.Log("Selected TargetL");
            maxObjects = maxObjectsTargetL; // Reset maxObjects of maxObjectsTargetL

            Debug.Log("Teleporter is not active");
            teleporter.SetActive(false); // Deactivate the teleporter
            //playerMovement.DisableMovement();

            return Targets[0];
        }
    }

    // Function to get the spawn position based on the player's score
    Vector3 GetSpawnAreas()
    {
        if (ScoreManager.scoreCount < 25 && spawnAreas[0] != null)
        {
            AreaSpawner areaSpawnerPos = spawnAreas[0].GetComponent<AreaSpawner>();

            if (areaSpawnerPos != null)
            {
                Debug.Log("Spawn area" + spawnAreas[0]);
                return areaSpawnerPos.TargetArea(); // Return the position for the first spawn area
            }
        }
        else if (ScoreManager.scoreCount < 50 && spawnAreas[1] != null)
        {
            AreaSpawner2 areaSpawnerPos = spawnAreas[1].GetComponent<AreaSpawner2>();

            if (areaSpawnerPos != null)
            {
                Debug.Log("Spawn area" + spawnAreas[1]);
                return areaSpawnerPos.TargetArea(); // Return the position for the second spawn area
            }
        }
        else if (ScoreManager.scoreCount >= 50 && spawnAreas[2] != null)
        {
            AreaSpawner3 areaSpawnerPos = spawnAreas[2].GetComponent<AreaSpawner3>();

            if (areaSpawnerPos != null)
            {
                Debug.Log("Spawn area" + spawnAreas[2]);
                return areaSpawnerPos.TargetArea(); // Return the position for the third spawn area
            }
        }
        else
        {
            Debug.LogError("Spawn area 0 is null.");
            return Vector3.zero;
        }
        
        return Vector3.zero; // Handle the case where spawnAreas[0] is null (return another default position or handle as needed)
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