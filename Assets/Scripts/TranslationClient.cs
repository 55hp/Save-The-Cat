using System.Collections;
using System.Text;
using UnityEngine;

public static class TranslationClient
{
    /// <summary>
    /// Translates the names of scene objects into the specified target language using an external translation service. The method constructs a prompt containing the object IDs and sends it to the translation service. Upon receiving the response, it updates the translations in the GameManager and invokes the provided callback action.
    /// </summary>
    /// <param name="targetLanguage"></param>
    /// <param name="onDone"></param>
    /// <returns></returns>
    public static IEnumerator TranslateSceneObjects(string targetLanguage, System.Action onDone)
    {
        var sb = new StringBuilder();
        sb.Append("Translate these game object ids into ").Append(targetLanguage).Append(". ");
        sb.Append("Return ONLY JSON in this shape: {\"translations\":[{\"id\":\"ladder\",\"name\":\"...\"}]}. ");
        sb.Append("Object ids: ");
        foreach (var o in GameManager.I.sceneObjects)
            sb.Append(o.id).Append(", ");

        yield return OpenRouterClient.I.SendPrompt(
            "You are a translation assistant for a children's game. Output only JSON, nothing else.",
            sb.ToString(),
            (raw) =>
            {
                string clean = OpenRouterClient.StripMarkdownFences(raw);
                try
                {
                    var resp = JsonUtility.FromJson<TranslationResponse>(clean);
                    foreach (var pair in resp.translations)
                        GameManager.I.translations[pair.id] = pair.name;
                }
                catch
                {
                    Debug.LogWarning("Traduzione fallita, fallback inglese");
                }
                onDone?.Invoke();
            },
            (err) =>
            {
                Debug.LogWarning("Errore chiamata traduzione: " + err);
                onDone?.Invoke();
            }
        );
    }
}