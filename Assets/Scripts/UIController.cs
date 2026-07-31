using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public InputField keyInputField;
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
        OpenRouterClient.I.apiKey = keyInputField.text;
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
            Debug.LogError("Errore OpenRouter: " + aiError); // NUOVO
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
            Debug.LogError("Errore parsing JSON. Risposta raw: " + clean); // NUOVO
            feedbackText.text = "Il gatto ti guarda confuso.";
            sendButton.interactable = true;
            yield break;
        }

        feedbackText.text = parsed.message;
        Debug.Log("Comandi ricevuti: " + string.Join(", ", parsed.commands.Select(c =>
            $"{c.verb}({c.target}{(string.IsNullOrEmpty(c.target2) ? "" : "," + c.target2)})")));
        
        if (!string.IsNullOrEmpty(parsed.language) && !GameManager.languageLocked)
        {
            GameManager.systemLanguage = parsed.language;
            GameManager.languageLocked = true;
            StartCoroutine(TranslationClient.TranslateSceneObjects(GameManager.systemLanguage, () =>
                Debug.Log("Lingua di sistema impostata: " + GameManager.systemLanguage)));
        }

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
            Debug.Log("Hai salvato il gatto!");
            gameFlow?.OnGameWon(); // vedi sotto
        }
    }
}