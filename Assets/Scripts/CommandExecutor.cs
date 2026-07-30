using System.Collections;
using UnityEngine;

public static class CommandExecutor
{
    /// <summary>
    /// Executes a sequence of commands for the player character.
    /// </summary>
    /// <param name="commands"></param>
    /// <param name="onComplete"></param>
    /// <returns></returns>
    public static IEnumerator Run(Command[] commands, System.Action<bool, string> onComplete)
    {
        var player = GameManager.I.player;

        foreach (var cmd in commands)
        {
            SceneObject target = string.IsNullOrEmpty(cmd.target) ? null : GameManager.I.FindObject(cmd.target);
            SceneObject target2 = string.IsNullOrEmpty(cmd.target2) ? null : GameManager.I.FindObject(cmd.target2);

            switch (cmd.verb)
            {
                case "WALK_TO":
                    if (target == null) { onComplete(false, "fail_not_found"); yield break; }
                    yield return player.WalkTo(target.transform);
                    break;

                case "PICK_UP":
                    if (target == null || !player.CanPickUp(target)) { onComplete(false, "fail_pickup"); yield break; }
                    yield return player.PickUp(target);
                    break;

                case "DROP":
                    if (!player.CanDrop()) { onComplete(false, "fail_drop"); yield break; }
                    yield return player.Drop();
                    break;

                case "PLACE":
                    if (target2 == null || !player.CanPlace(target2)) { onComplete(false, "fail_place"); yield break; }
                    yield return player.Place(target2.transform);
                    break;

                case "CLIMB":
                    if (target == null || !player.CanClimb(target)) { onComplete(false, "fail_climb"); yield break; }
                    yield return player.Climb(target.transform);
                    break;

                case "USE_SCANNER":
                    Scanner.I.Activate();
                    yield return new WaitForSeconds(0.5f);
                    break;

                default:
                    onComplete(false, "fail_unknown_verb");
                    yield break;
            }
        }

        onComplete(true, "success");
    }

    /// <summary>
    /// Runs a predefined sequence of commands for debugging purposes, logging the result to the console.
    /// </summary>
    /// <param name="runner"></param>
    public static void DebugTestSequence(MonoBehaviour runner)
    {
        var testCommands = new[]
        {
            new Command { verb = "WALK_TO", target = "ladder" },
            new Command { verb = "PICK_UP", target = "ladder" },
            new Command { verb = "WALK_TO", target = "tree" },
            new Command { verb = "PLACE", target = "ladder", target2 = "tree" },
            new Command { verb = "CLIMB", target = "ladder" },
            new Command { verb = "PICK_UP", target = "cat" }
        };
        runner.StartCoroutine(Run(testCommands, (ok, reason) => Debug.Log("Debug test: " + ok + " (" + reason + ")")));
    }
}