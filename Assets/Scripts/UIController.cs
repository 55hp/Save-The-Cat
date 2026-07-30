using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public InputField inputField;
    public Button sendButton;
    public Text feedbackText;
    public GameFlow gameFlow; // collega SOLO sulla UI del Livello 1, lascia vuoto sul Livello 2

    void Start()
    {
        sendButton.onClick.AddListener(OnSend);
    }

    void OnSend()
    {
        string playerText = inputField.text;
        if (string.IsNullOrWhiteSpace(playerText)) return;

        sendButton.interactable = false;
        StartCoroutine(SendFlow(playerText));
    }

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

        CheckWinCondition();
        sendButton.interactable = true;
    }

    void CheckWinCondition()
    {
        var cat = GameManager.I.FindObject("cat");
        if (cat != null && cat.state == "held")
        {
            Debug.Log("Livello completato!");
            gameFlow?.StartLevel2(); // sul Livello 2, gameFlow resta null: mostra qui uno schermo di fine gioco
        }
    }
}