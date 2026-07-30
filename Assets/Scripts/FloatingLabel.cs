using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatingLabel : MonoBehaviour
{
    public Text label;
    public float showDuration = 2f;

    /// <summary>
    /// Shows the floating label with the specified text for a set duration. If the label is already being displayed, it will reset the timer and show the new text.
    /// </summary>
    /// <param name="text"></param>
    public void Show(string text)
    {
        StopAllCoroutines();
        StartCoroutine(ShowRoutine(text));
    }
    
    /// <summary>
    /// Coroutine that handles the display of the floating label. It sets the label's text, enables it, waits for the specified duration, and then disables it.
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    IEnumerator ShowRoutine(string text)
    {
        label.text = text;
        label.enabled = true;
        yield return new WaitForSeconds(showDuration);
        label.enabled = false;
    }
}