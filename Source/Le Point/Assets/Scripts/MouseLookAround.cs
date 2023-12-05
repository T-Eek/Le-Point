using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookAround : MonoBehaviour
{
    // Start when the player moves its mouse around

    public Transform player;
    public float mouseSensitivity = 2f; // Mouse sensitivity
    float cameraVerticalRotation = 0f;

    void Start()
    {
        //
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player controls are enabled (not in pause state)
        if (IsPlayerControlsEnabled())
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
    }

    // Function to check if player controls are enabled
    private bool IsPlayerControlsEnabled()
    {
        // Check the game state or any other condition
        // You may need to adjust this based on your specific implementation
        return GameStateManager.Instance.CurrentGameState == GameState.Gameplay;
    }
}
