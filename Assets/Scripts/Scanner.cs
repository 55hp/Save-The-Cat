using System.Collections;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    public static Scanner I;

    CircleCollider2D scanCollider;
    SpriteRenderer sr;

    void Awake()
    {
        I = this;
        scanCollider = GetComponent<CircleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.enabled = false; // spento finché non viene usato
    }

    public void Activate()
    {
        StartCoroutine(PulseAndReveal());
    }

    IEnumerator PulseAndReveal()
    {
        if (sr != null) sr.enabled = true;

        float radius = scanCollider != null ? scanCollider.radius : 1.5f;
        var hits = Physics2D.OverlapCircleAll(transform.position, radius);
        foreach (var h in hits)
        {
            var so = h.GetComponent<SceneObject>();
            if (so == null) continue;

            string label = GameManager.I.translations.ContainsKey(so.id)
                ? GameManager.I.translations[so.id]
                : so.id;

            var floatingLabel = h.GetComponent<WordLabel>();
            if (floatingLabel != null) floatingLabel.Show();
        }

        yield return new WaitForSeconds(5f);
        if (sr != null) sr.enabled = false;
    }
}