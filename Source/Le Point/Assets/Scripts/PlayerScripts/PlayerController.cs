using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.InputSystem;
using TMPro;
//using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerController : MonoBehaviour
{

    Camera cam;
    private RayCastController rayCastController; // Reference to RayCastController
    public LayerMask mask;
    public ObjectSpawnerManager spawnerManager; // Reference to ObjectSpawnerManager

    private float deactivationDelay = 0.2f; // Adjust the delay duration as needed

    // Start is called before the first frame update
    void Start()
    {
        rayCastController = GetComponent<RayCastController>(); // Get the reference to RayCastController
        cam = Camera.main;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }

    void OnDestroy()
    {
        //prevent memory leaks and errors after it being destroyed
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }

    IEnumerator DeactivateObjectAfterDelay(GameObject obj)
    {
        yield return new WaitForSeconds(deactivationDelay);
        obj.SetActive(false);
        spawnerManager.OnObjectHit(); // Inform the ObjectSpawnerManager that an object was hit & spawns a new object

    }

    // Update is called once per frame
    void Update()
    {
        rayCastController.HandleRaycast(mask); // Use RayCastController for raycasting

        // Your additional logic specific to PlayerController
        // For example, handle additional behavior on mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, mask))
            {
                if (hit.transform != null)
                {
                    Renderer renderer = hit.transform.GetComponent<Renderer>();

                    if (renderer != null)
                    {
                        if (renderer.material.color != Color.red)
                        {
                            renderer.material.color = Color.red;
                            if (hit.transform.gameObject.CompareTag("Clicked"))
                            {
                                StartCoroutine(DeactivateObjectAfterDelay(hit.transform.gameObject));
                                ScoreManager.scoreCount++;
                                spawnerManager.OnObjectHit();
                                // Debug.Log(obj.name + " is hit successfully");
                            }
                        }
                    }
                }
            }
        }
    }


}


