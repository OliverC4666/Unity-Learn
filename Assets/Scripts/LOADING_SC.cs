using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen; // UI Panel for loading screen
    public Slider slider; // Progress bar slider
    public TMP_Text progressInPercent; // Floating percentage text
// RectTransform of percentage text
    //public RectTransform sliderTransform; // RectTransform of the slider
    
    public void LoadScene(int sceneIndex)
    {
        loadingScreen.SetActive(true); // Show loading screen
        StartCoroutine(AsyncLoad(sceneIndex));
  
    }

    IEnumerator AsyncLoad(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false; // Prevent automatic activation

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // Normalize progress

            // Update slider fill
            slider.value = progress;

            // Update percentage text
            progressInPercent.text = (progress * 100f).ToString("0") + "%";

            // Move the percentage text to follow the slider's fill
            //MoveTextAlongSlider(progress);

            // Debug log
            Debug.Log("Loading Progress: " + (progress * 100f) + "%");

            // Activate scene when fully loaded
            if (progress >= 1f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    //void MoveTextAlongSlider(float progress)
    //{
    //    // Get the slider width
    //    float sliderWidth = sliderTransform.rect.width;

    //    // Calculate the new text position based on progress
    //    Vector3 newPosition = sliderTransform.position;
    //    newPosition.x = sliderTransform.position.x - (sliderWidth / 2) + (sliderWidth * progress);

    //    // Apply the new position to the text
    //    progressInPercent.rectTransform.position = newPosition;
    //}
}
