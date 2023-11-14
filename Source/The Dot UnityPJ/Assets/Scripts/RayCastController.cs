using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastController : MonoBehaviour
{
    Camera cam;
    public Color rayColor = Color.red; // Set the desired color for the ray
    public float mousePosZ = 10f; // Set the desired value for mousePos.z
    public float rayLength = 100f;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Draw Ray
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mousePosZ; // Set the desired value for mousePos.z
        mousePos = cam.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(transform.position, mousePos - transform.position, rayColor);
    }

    public void HandleRaycast(LayerMask mask)
    {
        // Draw Ray
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mousePosZ;
        mousePos = cam.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(transform.position, mousePos - transform.position, rayColor);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayLength, mask))
            {
                // Handle the ray hit based on your requirements
                Debug.Log("Ray hit object: " + hit.transform.name);
            }
        }
    }
}
