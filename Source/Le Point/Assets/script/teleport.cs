using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    public Transform player, destination;
    public GameObject playerg;

    private Movement movementScript;

    // Ensure that the Movement script is set in the Unity Editor
    void Start()
    {
        movementScript = player.GetComponent<Movement>();
    }

  void OnTriggerEnter(Collider other){
  if(other.CompareTag("Player")){

   playerg.SetActive(false);
   player.position = destination.position;
   playerg.SetActive(true);

   
   if (movementScript != null) // Disable player movement after teleporting
   {
     movementScript.DisableMovement(); // Disable movement
   }
  }
 }
}