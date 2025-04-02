using UnityEngine;

public class TrackSpawner : MonoBehaviour
{ 
    public GameObject trackPrefab; // Assign your prefab in the Inspector
    public const int numberOfTracks = 10; // Number of tracks to spawn
    public float distanceBetweenTracks = 12f; // Spacing between tracks
    private GameObject[] GameArr = new GameObject[numberOfTracks]; // Array to hold track objects

    void Start()
    {
        Vector3 position = new Vector3(0,0,0) ;
        for (int i = 1; i < numberOfTracks; i++)
        {  
            // Spawn each track
            GameArr[i] = Instantiate(trackPrefab, position, trackPrefab.transform.rotation);
        }
        
    }

    private void Update()
    {
        for (int i = 1; i < GameArr.Length; i++)
        {
            // Get the rotation angle (in radians) from the prefab
            float rotationAngle = -trackPrefab.transform.eulerAngles.y * Mathf.Deg2Rad;

            // Calculate the relative position
            float localZ = i * distanceBetweenTracks; // Original z offset
            float localX = 0; // No x offset in the original configuration

            // Apply rotation to calculate world-space position
            float rotatedX = Mathf.Cos(rotationAngle) * localX - Mathf.Sin(rotationAngle) * localZ;
            float rotatedZ = Mathf.Sin(rotationAngle) * localX + Mathf.Cos(rotationAngle) * localZ;

            // Update the position
            Vector3 position = new Vector3(
                trackPrefab.transform.position.x + rotatedX,
                trackPrefab.transform.position.y,
                trackPrefab.transform.position.z + rotatedZ
            );
            GameArr[i].transform.position = position;

            // Update rotation to match the prefab
            GameArr[i].transform.rotation = trackPrefab.transform.rotation;
        }
    }
}