using UnityEngine;

public class SpawnHouse : MonoBehaviour
{
    public GameObject HousePrefab; // Assign your prefab in the Inspector
    public const int numberOfHouses = 11; // Number of houses to spawn
    public float distanceBetweenHouses = 12f; // Spacing between houses
    private GameObject[,] GameArr = new GameObject[numberOfHouses, numberOfHouses]; // Array to hold house objects

    void Start()
    {
        // Loop through rows and columns
        for (int i = 0; i < numberOfHouses; i++)
        {
            for (int j = 0; j < numberOfHouses; j++)
            {
                // Calculate position for each house
                Vector3 position = new Vector3(
                    i * distanceBetweenHouses,
                    HousePrefab.transform.position.y,
                    j * distanceBetweenHouses
                );

                // Spawn each house
                GameArr[i, j] = Instantiate(HousePrefab, position, HousePrefab.transform.rotation);
            }
        }
    }

    void Update()
    {
        // Loop through all houses
        for (int i = 0; i < numberOfHouses; i++)
        {
            for (int j = 0; j < numberOfHouses; j++)
            {
                // Calculate rotation angle in radians
                float rotationAngle = -HousePrefab.transform.eulerAngles.y * Mathf.Deg2Rad;

                // Calculate rotated positions
                float localX = i * distanceBetweenHouses;
                float localZ = j * distanceBetweenHouses;

                float rotatedX = Mathf.Cos(rotationAngle) * localX - Mathf.Sin(rotationAngle) * localZ;
                float rotatedZ = Mathf.Sin(rotationAngle) * localX + Mathf.Cos(rotationAngle) * localZ;

                Vector3 position = new Vector3(
                    HousePrefab.transform.position.x + rotatedX,
                    HousePrefab.transform.position.y,
                    HousePrefab.transform.position.z + rotatedZ
                );

                // Update position and rotation
                GameArr[i, j].transform.position = position;
                GameArr[i, j].transform.rotation = HousePrefab.transform.rotation;
            }
        }
    }
}