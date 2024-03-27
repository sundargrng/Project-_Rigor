using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public Message[] messages;
    public Actor[] actors;

    public void StartDialogue()
    {
        FindAnyObjectByType<DialogManager>().OpenDialogue(messages, actors);
    }


}

[System.Serializable]
public class Message
{
    public int actorID;
    public string message;
}


[System.Serializable]
public class Actor
{
    public string actorName;
    public Sprite sprite;
}
