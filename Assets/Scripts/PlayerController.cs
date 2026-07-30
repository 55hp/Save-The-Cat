using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float climbSpeed = 2f;
    public Transform heldSlot;
    public GameObject heldObject = null;

    /// <summary>
    /// Moves the player towards the specified target position at a defined walking speed. The movement continues until the player is within a certain distance (0.3 units) of the target. This method is implemented as a coroutine, allowing it to be executed over multiple frames.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public IEnumerator WalkTo(Transform target)
    {
        while (Vector2.Distance(transform.position, target.position) > 0.3f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, walkSpeed * Time.deltaTime);
            yield return null;
        }
    }

    /// <summary>
    /// Determines if the player can pick up the specified SceneObject. The player can pick up an object if they are not currently holding anything.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool CanPickUp(SceneObject obj) => heldObject == null;

    /// <summary>
    /// Picks up the specified SceneObject and places it in the player's held slot. The object is then marked as being held.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public IEnumerator PickUp(SceneObject obj)
    {
        heldObject = obj.gameObject;
        obj.transform.SetParent(heldSlot);
        obj.transform.localPosition = Vector3.zero;
        obj.state = "held";
        yield return null;
    }

    /// <summary>
    /// Determines if the player can drop the currently held object. The player can drop an object if they are currently holding one.
    /// </summary>
    /// <returns></returns>
    public bool CanDrop() => heldObject != null;

    /// <summary>
    /// Drops the currently held object, removing it from the player's held slot and placing it back into the scene. The object's state is updated to indicate that it is now on the ground.
    /// </summary>
    /// <returns></returns>
    public IEnumerator Drop()
    {
        if (heldObject == null) yield break;
        var obj = heldObject.GetComponent<SceneObject>();
        heldObject.transform.SetParent(null);
        obj.state = "on_ground";
        heldObject = null;
        yield return null;
    }

    /// <summary>
    /// Determines if the player can place the currently held object onto the specified target SceneObject. The player can place an object if they are currently holding one.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool CanPlace(SceneObject target) => heldObject != null;

    /// <summary>
    /// Places the currently held object onto the specified target SceneObject. The held object is detached from the player's held slot and positioned above the target object. The state of the held object is updated to indicate that it has been placed on the target object.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Determines if the player can climb the specified SceneObject. The player can climb an object if its state indicates that it has been placed on another object (i.e., its state starts with "placed_on").
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool CanClimb(SceneObject obj) => obj.state.StartsWith("placed_on");

    public IEnumerator Climb(Transform target)
    {
        Vector3 top = target.position + Vector3.up * 2f;
        while (Vector2.Distance(transform.position, top) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, top, climbSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public IEnumerator Jump(Transform target)
    {
        Vector3 top = target.position + Vector3.up * 2f;
        while (Vector2.Distance(transform.position, top) > 0.1f)
        {
            transform.position = Vector2.MoveTowards(transform.position, top, climbSpeed * Time.deltaTime);
            yield return null;
        }
    }
}