using UnityEngine;
using TMPro;
using System.Collections;

public class WordLabel : MonoBehaviour
{
    [SerializeField] string englishWord; // scritta a mano — deve coincidere con l'id dello SceneObject collegato
    public TMP_Text label;               // assegnalo tu

    public float showDuration = 4f;
    public float floatAmplitude = 0.15f;
    public float floatSpeed = 2f;

    Vector3 basePosition;
    Coroutine activeRoutine;

    void Awake()
    {
        basePosition = transform.localPosition;
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
            float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.localPosition = basePosition + new Vector3(0f, offsetY, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = basePosition;
        if (label != null) label.enabled = false;
    }
}