using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class OpenRouterClient : MonoBehaviour
{
    public static OpenRouterClient I;

    const string URL = "https://openrouter.ai/api/v1/chat/completions";

    public string apiKey;
    public string model = "anthropic/claude-haiku-4-5";

    void Awake() { I = this; }

    /// <summary>
    /// Sends a prompt to the OpenRouter API and retrieves the response. The method constructs a request payload with the specified system and user prompts, sends it to the API, and invokes the provided callbacks upon completion or error.
    /// </summary>
    /// <param name="systemPrompt"></param>
    /// <param name="userPrompt"></param>
    /// <param name="onDone"></param>
    /// <param name="onError"></param>
    /// <returns></returns>
    public IEnumerator SendPrompt(string systemPrompt, string userPrompt, Action<string> onDone, Action<string> onError)
    {
        var payload = new ORRequest
        {
            model = model,
            messages = new[]
            {
                new ORMessage { role = "system", content = systemPrompt },
                new ORMessage { role = "user", content = userPrompt }
            }
        };

        string json = JsonUtility.ToJson(payload);

        using (var req = new UnityWebRequest(URL, "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke(req.error + " | " + req.downloadHandler.text);
                yield break;
            }

            var resp = JsonUtility.FromJson<ORResponseWrapper>(req.downloadHandler.text);
            onDone?.Invoke(resp.choices[0].message.content);
        }
    }

    public static string StripMarkdownFences(string raw)
    {
        raw = raw.Trim();
        if (raw.StartsWith("```"))
        {
            int firstNewline = raw.IndexOf('\n');
            raw = raw.Substring(firstNewline + 1);
            int lastFence = raw.LastIndexOf("```");
            if (lastFence >= 0) raw = raw.Substring(0, lastFence);
        }
        return raw.Trim();
    }
}