using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I;
    
    public static string systemLanguage = "en"; // impostata una sola volta, al primo prompt
    public static bool languageLocked = false;

    public PlayerController player;

    [Header("Oggetti Livello 1 — trascinali qui dalla Hierarchy")]
    public List<SceneObject> level1Objects = new List<SceneObject>();

    [Header("Oggetti Livello 2 — trascinali qui dalla Hierarchy")]
    public List<SceneObject> level2Objects = new List<SceneObject>();

    // Costruita da sola in Awake unendo le due liste sopra — non toccarla a mano
    public List<SceneObject> sceneObjects = new List<SceneObject>();

    public Dictionary<string, string> translations = new Dictionary<string, string>();
    public string detectedLanguage = "en";

    // Posizione e genitore di partenza di ogni oggetto, salvati da soli all'avvio
    private Dictionary<SceneObject, Vector3> initialPositions = new Dictionary<SceneObject, Vector3>();
    private Dictionary<SceneObject, Transform> initialParents = new Dictionary<SceneObject, Transform>();

    void Awake()
    {
        I = this;

        sceneObjects.Clear();
        sceneObjects.AddRange(level1Objects);
        sceneObjects.AddRange(level2Objects);

        foreach (var o in sceneObjects)
        {
            initialPositions[o] = o.transform.position;
            initialParents[o] = o.transform.parent;
        }
    }

    public SceneObject FindObject(string id)
    {
        foreach (var o in sceneObjects)
            if (o.id == id && o.gameObject.activeInHierarchy) return o;
        return null;
    }

    // Riporta un oggetto a posizione, genitore e stato con cui è partito — utile per un futuro "riprova"
    public void ResetObject(SceneObject o)
    {
        o.transform.SetParent(initialParents[o]);
        o.transform.position = initialPositions[o];
        o.state = "on_ground";
        o.gameObject.SetActive(true);
    }

    // Disattiva ogni oggetto di un gruppo, indipendentemente da dove si trova ora in gerarchia
    public void DeactivateGroup(List<SceneObject> group)
    {
        foreach (var o in group) o.gameObject.SetActive(false);
    }
}