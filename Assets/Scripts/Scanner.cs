using UnityEngine;

public class Scanner : MonoBehaviour
{
    public static Scanner I;

    public Transform player;
    public Vector2 auraSize = new Vector2(3f, 2f);

    void Awake() { I = this; }
    
    /// <summary>
    /// Activates the scanner, checking for objects within the defined aura size around the player. If any objects are detected, it retrieves their labels from the GameManager's translations and displays them using the FloatingLabel component.
    /// </summary>
    public void Activate()
    {
        var hits = Physics2D.OverlapBoxAll(player.position, auraSize, 0f);
        foreach (var h in hits)
        {
            var so = h.GetComponent<SceneObject>();
            if (so == null) continue;

            string label = GameManager.I.translations.ContainsKey(so.id)
                ? GameManager.I.translations[so.id]
                : so.id;

            var floatingLabel = h.GetComponent<FloatingLabel>();
            if (floatingLabel != null) floatingLabel.Show(label);
        }
    }
}