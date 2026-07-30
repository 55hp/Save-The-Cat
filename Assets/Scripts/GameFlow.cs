using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public GameObject level1Root;
    public GameObject level2Root;

    /// <summary>
    /// Starts Level 1 by activating the level1Root GameObject and deactivating the level2Root GameObject. It also initiates the translation of scene objects based on the detected language in the GameManager. Once translations are ready, it logs a message indicating the current language.
    /// </summary>
    public void StartLevel2()
    {
        level1Root.SetActive(false);
        level2Root.SetActive(true);
        StartCoroutine(TranslationClient.TranslateSceneObjects(GameManager.I.detectedLanguage, () =>
        {
            Debug.Log("Traduzioni pronte, lingua: " + GameManager.I.detectedLanguage);
        }));
    }
}