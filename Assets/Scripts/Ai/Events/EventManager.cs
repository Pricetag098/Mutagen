using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public List<Event> events = new List<Event>();

    private void Start()
    {
        for(int i = 0; i < events.Count; i++)
        {
            events[i] = Instantiate(events[i]);
        }
    }
}
