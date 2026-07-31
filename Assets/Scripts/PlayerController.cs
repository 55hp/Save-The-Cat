using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float climbSpeed = 2f;
    public Transform heldSlot;
    public GameObject heldObject = null;
    public bool isElevated = false;

    public float interactionRange = 1f; // quanto vicino serve essere per interagire — aggiustabile se sembra troppo severo/permissivo

    public bool IsNear(SceneObject obj) =>
        Vector2.Distance(transform.position, obj.transform.position) <= interactionRange;
    
    public IEnumerator WalkTo(Transform target)
    {
        isElevated = false;
        while (Vector2.Distance(transform.position, target.position) > 0.3f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, walkSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public bool CanPickUp(SceneObject obj) =>
        heldObject == null && (obj.reachState != "elevated" || isElevated);

    public IEnumerator PickUp(SceneObject obj)
    {
        heldObject = obj.gameObject;
        obj.transform.SetParent(heldSlot);
        obj.transform.localPosition = Vector3.zero;
        obj.state = "held";
        yield return null;
    }

    public bool CanDrop() => heldObject != null;

    public IEnumerator Drop()
    {
        if (heldObject == null) yield break;
        var obj = heldObject.GetComponent<SceneObject>();
        heldObject.transform.SetParent(null);
        obj.state = "on_ground";
        heldObject = null;
        yield return null;
    }

    public bool CanPlace(SceneObject target) => heldObject != null;

    public IEnumerator Place(Transform target)
    {
        if (heldObject == null) yield break;
        var obj = heldObject.GetComponent<SceneObject>();
        heldObject.transform.SetParent(null);
        heldObject.transform.position = target.position + Vector3.up * 0.5f;
        obj.state = "placed_on_" + target.GetComponent<SceneObject>()?.id;
        heldObject = null;
        yield return null;
    }

    public bool CanClimb(SceneObject obj) => obj.state.StartsWith("placed_on");

    public IEnumerator Climb(Transform target)
    {
        Vector3 top = target.position + Vector3.up * 2f;
        while (Vector2.Distance(transform.position, top) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, top, climbSpeed * Time.deltaTime);
            yield return null;
        }
        isElevated = true;
    }
}