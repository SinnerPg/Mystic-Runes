using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionManager : MonoBehaviour
{
    public UnityEvent enterEvent, exitEvent;
    public Room1Manager room1Manager;
    public Room2Manager room2Manager;
    public int value;

    public void checkInteractionGlyphUp()
    {
        room1Manager.enableInteractionGlyphUp(value);
    }

    public void checkInteractionGlyph()
    {
        room1Manager.enableInteractionGlyph(value);
    }

    void OnTriggerEnter(Collider other)
    {
        enterEvent.Invoke();
    }


    private void OnTriggerExit(Collider other)
    {
        exitEvent.Invoke();
    }
}
