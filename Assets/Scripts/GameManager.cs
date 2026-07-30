using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I;
    public PlayerController player;
    public List<SceneObject> sceneObjects = new List<SceneObject>();
    public Dictionary<string, string> translations = new Dictionary<string, string>();
    public string detectedLanguage = "en";

    void Awake() { I = this; }

    /// <summary>
    /// Finds a SceneObject in the sceneObjects list by its ID. If an object with the specified ID is found, it returns that object; otherwise, it returns null.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public SceneObject FindObject(string id)
    {
        foreach (var o in sceneObjects) if (o.id == id) return o;
        return null;
    }

    /// <summary>
    /// Registers a SceneObject in the sceneObjects list if it is not already present. This method ensures that each SceneObject is only added once to the list.
    /// </summary>
    /// <param name="o"></param>
    public void RegisterObject(SceneObject o)
    {
        if (!sceneObjects.Contains(o)) sceneObjects.Add(o);
    }
}