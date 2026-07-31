using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I;

    public static string systemLanguage = "en"; // impostata una sola volta, al primo prompt
    public static bool languageLocked = false;

    public PlayerController player;

    [Header("Tutti gli oggetti della scena — trascinali qui dalla Hierarchy")]
    public List<SceneObject> sceneObjects = new List<SceneObject>();

    public Dictionary<string, string> translations = new Dictionary<string, string>();

    private Dictionary<SceneObject, Vector3> initialPositions = new Dictionary<SceneObject, Vector3>();
    private Dictionary<SceneObject, Transform> initialParents = new Dictionary<SceneObject, Transform>();

    void Awake()
    {
        I = this;
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

    public void ResetObject(SceneObject o)
    {
        o.transform.SetParent(initialParents[o]);
        o.transform.position = initialPositions[o];
        o.state = "on_ground";
        o.gameObject.SetActive(true);
    }
}