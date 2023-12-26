using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSpawner : MonoBehaviour
{
    public Vector3 center; // TargetArea parameters
    public Vector3 size; // TargetArea parameters
    public UnityEngine.Color gizmoColor = new UnityEngine.Color(0, 1, 0, 0.5f);

    private static AreaSpawner instance;

    // Singleton pattern to make the script accessible from other scripts
    public static AreaSpawner Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AreaSpawner>();
            }
            Debug.Log("Instance found");
            return instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        TargetArea();
    }

    void Update()
    {
        //
    }

    public Vector3 TargetArea()
    {
        Debug.Log("pos found");
        Vector3 pos = center + new Vector3(
            Random.Range(-size.x / 2, size.x / 2),
            Random.Range(-size.y / 2, size.y / 2),
            Random.Range(-size.z / 2, size.z / 2)
        );
        Debug.Log(pos);
        return pos;
    }

    // Add this method to get the Gizmo color
    public UnityEngine.Color GetGizmoColor()
    {
        return gizmoColor;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = GetGizmoColor();
        Gizmos.DrawCube(center, size);
    }
}
