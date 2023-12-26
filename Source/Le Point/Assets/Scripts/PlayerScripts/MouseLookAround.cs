using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookAround : MonoBehaviour
{
    // Start when the player moves its mouse around

    public Transform player;
    public float mouseSensitivity = 2f; // Mouse sensitivity
    float cameraVerticalRotation = 0f;

    private static MouseLookAround instance; // allows you to connnect other script to it

    // Singleton pattern to make the script accessible from other scripts
    public static MouseLookAround Instance // An instance to ObjectSpawnerManager
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MouseLookAround>();
            }
            Debug.Log("Instance found");
            return instance;
        }
    }
    void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += onGameStateChanged;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= onGameStateChanged;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the mouse input
        float cameraX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float cameraY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate the camera around the local X Axis
        cameraVerticalRotation -= cameraY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        // Rotate the player around the local Y Axis
        player.Rotate(Vector3.up * cameraX);
    }

    private void onGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }
}
