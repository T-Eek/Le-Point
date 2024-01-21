using System.Collections;
using UnityEngine;

[System.Serializable]
public class ScoreSound
{
    public int scoreNumber;
    public AudioClip sound;
}

public class PlayerController : MonoBehaviour
{
    Camera cam;
    private RayCastController rayCastController;
    public LayerMask mask;
    public ObjectSpawnerManager spawnerManager;

    public ScoreSound[] scoreSounds;
    private AudioSource audioSource;

    private float deactivationDelay = 0.2f;

    void Start()
    {
        rayCastController = GetComponent<RayCastController>();
        cam = Camera.main;
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void OnDestroy()
    {
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
        spawnerManager.OnObjectHit();
    }

    void Update()
    {
        rayCastController.HandleRaycast(mask);

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

                                // Check if the current score has a corresponding sound
                                foreach (ScoreSound scoreSound in scoreSounds)
                                {
                                    if (scoreSound.scoreNumber == ScoreManager.scoreCount)
                                    {
                                        // Play the audio clip for the current score
                                        if (audioSource != null && scoreSound.sound != null)
                                        {
                                            audioSource.clip = scoreSound.sound;
                                            audioSource.Play();
                                        }
                                    }
                                }

                                spawnerManager.OnObjectHit();
                            }
                        }
                    }
                }
            }
        }
    }
}
