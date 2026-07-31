using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public void StartLevel2()
    {
        GameManager.I.DeactivateGroup(GameManager.I.level1Objects);
        GameManager.I.player.heldObject = null;

        foreach (var o in GameManager.I.level2Objects)
            o.gameObject.SetActive(true);

        StartCoroutine(TranslationClient.TranslateSceneObjects(GameManager.I.detectedLanguage, () =>
        {
            Debug.Log("Traduzioni pronte, lingua: " + GameManager.I.detectedLanguage);
        }));
    }
}