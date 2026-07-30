using UnityEngine;

public class SceneObject : MonoBehaviour
{
    public string id;
    public string color;
    public string[] aka;
    public string state = "on_ground";
    public string reachState = "ground"; // "ground" o "elevated"

    void Start() { GameManager.I.RegisterObject(this); }
}