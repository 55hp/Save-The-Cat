using System;

[Serializable] public class Command
{
    public string verb;
    public string target;
    public string target2;
}

[Serializable] public class AIResponse
{
    public Command[] commands;
    public string message;
    public string language;
}

[Serializable] public class SceneManifestObj
{
    public string id;
    public string color;
    public string[] aka;
    public string state;
}

[Serializable] public class SceneManifestWrapper
{
    public SceneManifestObj[] objects;
}

[Serializable] public class TranslationPair
{
    public string id;
    public string name;
}

[Serializable] public class TranslationResponse
{
    public TranslationPair[] translations;
}

[Serializable] public class ORMessage
{
    public string role;
    public string content;
}

[Serializable] public class ORRequest
{
    public string model;
    public ORMessage[] messages;
    public int max_tokens;
}

[Serializable] public class ORChoice
{
    public ORMessage message;
}

[Serializable] public class ORResponseWrapper
{
    public ORChoice[] choices;
}





