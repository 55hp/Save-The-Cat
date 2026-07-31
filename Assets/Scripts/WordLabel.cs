using UnityEngine;
using TMPro;
using System.Collections;

public class WordLabel : MonoBehaviour
{
    [SerializeField] string englishWord; // scritta a mano — deve coincidere con l'id dello SceneObject collegato
    public TMP_Text label;               // il testo da mostrare, assegnalo tu
    public Transform wordTransform;      // il transform che fluttua — di solito lo stesso oggetto del label

    public float showDuration = 2f;
    public float floatAmplitude = 0.15f;
    public float floatSpeed = 2f;

    Vector3 basePosition;
    Coroutine activeRoutine;

    void Awake()
    {
        if (wordTransform != null) basePosition = wordTransform.localPosition;
        if (label != null) label.enabled = false;
    }

    public void Show()
    {
        if (activeRoutine != null) StopCoroutine(activeRoutine);
        activeRoutine = StartCoroutine(ShowRoutine());
    }

    IEnumerator ShowRoutine()
    {
        string translated = GameManager.I.translations.TryGetValue(englishWord, out var t) ? t : englishWord;
        if (label != null)
        {
            label.text = translated;
            label.enabled = true;
        }

        float elapsed = 0f;
        while (elapsed < showDuration)
        {
            if (wordTransform != null)
            {
                float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
                wordTransform.localPosition = basePosition + new Vector3(0f, offsetY, 0f);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (wordTransform != null) wordTransform.localPosition = basePosition;
        if (label != null) label.enabled = false;
    }
}