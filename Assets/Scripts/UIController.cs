using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public InputField inputField;
    public Button sendButton;
    public Text feedbackText;

    void Start()
    {
        sendButton.onClick.AddListener(OnSend);
    }

    /// <summary>
    /// Called when the send button is clicked. It retrieves the player's input, checks if it's valid, disables the send button to prevent multiple submissions, and starts the coroutine to handle sending the input to the AI and processing the response.
    /// </summary>
    void OnSend()
    {
        string playerText = inputField.text;
        if (string.IsNullOrWhiteSpace(playerText)) return;

        sendButton.interactable = false;
        StartCoroutine(SendFlow(playerText));
    }

    /// <summary>
    /// Coroutine that handles the flow of sending the player's input to the AI, receiving the response, parsing it, and executing any commands. It also manages UI feedback and button interactivity.
    /// </summary>
    /// <param name="playerText"></param>
    /// <returns></returns>
    IEnumerator SendFlow(string playerText)
    {
        string system = PromptBuilder.BuildSystemPrompt();

        string aiRaw = null;
        string aiError = null;

        yield return OpenRouterClient.I.SendPrompt(system, playerText,
            (r) => aiRaw = r,
            (e) => aiError = e);

        if (aiError != null)
        {
            feedbackText.text = "Il gatto si distrae per un attimo... riprova!";
            sendButton.interactable = true;
            yield break;
        }

        string clean = OpenRouterClient.StripMarkdownFences(aiRaw);
        AIResponse parsed;
        try
        {
            parsed = JsonUtility.FromJson<AIResponse>(clean);
        }
        catch
        {
            feedbackText.text = "Il gatto ti guarda confuso.";
            sendButton.interactable = true;
            yield break;
        }

        feedbackText.text = parsed.message;
        if (!string.IsNullOrEmpty(parsed.language))
            GameManager.I.detectedLanguage = parsed.language;

        yield return CommandExecutor.Run(parsed.commands, (success, reason) =>
        {
            Debug.Log("Esito comandi: " + success + " (" + reason + ")");
        });

        sendButton.interactable = true;
    }
}