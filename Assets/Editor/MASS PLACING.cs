using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class SceneCreator : EditorWindow
{
    private Transform PLACING;
    private GameObject prefab;
    private int numberOfObjects = 10;
    private float spacing = 5f;
    private GameObject[] GameArr;

    [MenuItem("Tools/Mass Placing")]
    public static void ShowWindow()
    {
        GetWindow<SceneCreator>("Mass Placing");
    }

    void OnGUI()
    {
        GUILayout.Label("Mass Placing Tool", EditorStyles.boldLabel);

        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), true);
        PLACING = (Transform)EditorGUILayout.ObjectField("Placing pos", PLACING, typeof(Transform), true);
        numberOfObjects = EditorGUILayout.IntField("Number of Objects", numberOfObjects);
        spacing = EditorGUILayout.FloatField("Spacing", spacing);

        if (GUILayout.Button("Create Objects"))
        {
            CreateObjects();
        }

        if (GUILayout.Button("DELETE OBJECTS"))
        {
            DeleteObjects();
        }

        if (GUILayout.Button("Update Rotation"))
        {
            UpdateRotation(); // Call it directly
        }
    }

    void CreateObjects()
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is not assigned!");
            return;
        }
        if (PLACING == null)
        {
            Debug.LogError("Placing position is not assigned!");
            return;
        }

        GameArr = new GameObject[numberOfObjects];

        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 position = new Vector3(PLACING.position.x + (i * spacing), PLACING.position.y, PLACING.position.z);
            GameArr[i] = Instantiate(prefab, position, Quaternion.identity);
        }
    }

    void DeleteObjects()
    {
        if (GameArr == null || GameArr.Length == 0)
        {
            Debug.LogWarning("No objects to delete!");
            return;
        }

        foreach (GameObject obj in GameArr)
        {
            if (obj != null)
                DestroyImmediate(obj); // Use DestroyImmediate for Editor
        }

        GameArr = null; // Reset reference
    }

    void UpdateRotation()
    {
        if (GameArr == null || GameArr.Length == 0)
        {
            Debug.LogError("No objects to update!");
            return;
        }

        if (PLACING == null)
        {
            Debug.LogError("Placing position is not assigned!");
            return;
        }

        float rotationAngle = PLACING.eulerAngles.y * Mathf.Deg2Rad; // Convert rotation to radians
        float cosTheta = Mathf.Cos(rotationAngle);
        float sinTheta = Mathf.Sin(rotationAngle);

        for (int i = 0; i < GameArr.Length; i++)
        {
            float localZ = i * spacing; // Spacing along Z-axis
            float localX = 0;           // Objects remain aligned on X-axis

            // Apply rotation manually (Complex number rotation)
            float rotatedX = cosTheta * localX - sinTheta * localZ;
            float rotatedZ = sinTheta * localX + cosTheta * localZ;

            // Set position relative to PLACING
            GameArr[i].transform.position = new Vector3(
                PLACING.position.x + rotatedX,
                PLACING.position.y,
                PLACING.position.z + rotatedZ
            );

            // Apply rotation to match PLACING
            GameArr[i].transform.rotation = PLACING.rotation;
        }

        Debug.Log("Rotation Updated with Correct Direction!");
    }


}
