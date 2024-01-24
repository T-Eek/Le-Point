using System.Collections;
using UnityEngine;

public class teleport : MonoBehaviour
{
    public Transform player, destination;
    public GameObject playerg;
    public AudioClip teleportSound;
    private AudioSource audioSource;
    private Movement movementScript;
    private bool hasPlayedSound = false;

    void Start()
    {
        movementScript = player.GetComponent<Movement>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasPlayedSound)
        {
            if (teleportSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(teleportSound);
                hasPlayedSound = true;
                StartCoroutine(StopSoundAfterDelay(1.0f)); // Change 2.0f to the duration you want
            }

            playerg.SetActive(false);
            player.position = destination.position;
            playerg.SetActive(true);
        }
    }

    IEnumerator StopSoundAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.Stop();
    }
}
