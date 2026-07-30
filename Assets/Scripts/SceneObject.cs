using UnityEngine;

/// <summary>
/// Represents an object in the scene with properties such as ID, color, aliases (aka), and state. This class is responsible for registering itself with the GameManager upon initialization.
/// </summary>
public class SceneObject : MonoBehaviour
{
    public string id;
    public string color;
    public string[] aka;
    public string state = "on_ground";

    void Start() { GameManager.I.RegisterObject(this); }
}