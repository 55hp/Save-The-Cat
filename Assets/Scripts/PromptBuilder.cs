using System.Linq;
using System.Text;

public static class PromptBuilder
{
    public static string BuildSystemPrompt()
    {
        var sb = new StringBuilder();

        sb.AppendLine("You are the command interpreter for a children's puzzle game (ages 5-9).");
        sb.AppendLine("The player describes a plan in free text, in any language. Translate it into a sequence of commands using ONLY the verbs listed below. Do not invent verbs.");
        sb.AppendLine();
        sb.AppendLine("VALID VERBS:");
        sb.AppendLine("WALK_TO <target> - move next to an object");
        sb.AppendLine("PICK_UP <target> - pick up an object, requires empty hands");
        sb.AppendLine("DROP - drop whatever is held");
        sb.AppendLine("PLACE <held> <target2> - place held object against target2");
        sb.AppendLine("CLIMB <target> - climb an object, requires it to be placed against something");
        sb.AppendLine("USE_SCANNER - reveal names of nearby objects (level 2 only)");
        sb.AppendLine();
        sb.AppendLine("SCENE OBJECTS (JSON):");
        sb.AppendLine(BuildSceneManifestJson());
        sb.AppendLine();
        sb.AppendLine("If the player's plan is missing a step, still output the commands they described — the game will show a comic failure. Do not fix their plan for them.");
        sb.AppendLine();
        sb.AppendLine("OUTPUT RULES:");
        sb.AppendLine("Return ONLY valid JSON, no markdown, no explanation, in this exact shape:");
        sb.AppendLine("{\"commands\":[{\"verb\":\"WALK_TO\",\"target\":\"ladder\"}],\"message\":\"...\",\"language\":\"it\"}");
        sb.AppendLine("\"commands\" must use ONLY the English verbs and object ids listed above — never translate them.");
        sb.AppendLine("\"message\" must be written in the SAME language the player used.");
        sb.AppendLine("\"language\" must be the ISO 639-1 code of the language the player used (e.g. \"it\", \"es\", \"sv\").");

        return sb.ToString();
    }

    static string BuildSceneManifestJson()
    {
        var objs = GameManager.I.sceneObjects.Where(o => o.gameObject.activeInHierarchy).ToArray();
        var wrapper = new SceneManifestWrapper { objects = new SceneManifestObj[objs.Length] };
        for (int i = 0; i < objs.Length; i++)
        {
            wrapper.objects[i] = new SceneManifestObj
            {
                id = objs[i].id,
                color = objs[i].color,
                aka = objs[i].aka,
                state = objs[i].state
            };
        }
        return UnityEngine.JsonUtility.ToJson(wrapper);
    }
}