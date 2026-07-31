using UnityEngine;

public class SceneObject : MonoBehaviour
{
    public string id;
    public string color;
    public string[] aka;
    public string state = "on_ground";
    public string reachState = "ground"; // "ground" o "elevated"

    public bool isTrap = false;              // NUOVO — solo sulla corda
    public bool trapTriggered = false;       // NUOVO — si spezza una volta sola
    public bool requiresDitchCrossing = false;
    
    void Start()
    {
        if (!GameManager.I.sceneObjects.Contains(this))
            Debug.LogWarning($"{name}: non è in sceneObjects su GameManager — l'AI non lo vedrà mai.");
    }
}