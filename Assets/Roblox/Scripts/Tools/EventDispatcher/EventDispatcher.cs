using UnityEngine;
using System.Collections;

public class EventDispatcher : PriorityEventDispatcher<EventName, object, EventTypeComparer> 
{
	static EventDispatcher instance = new EventDispatcher();

	public static EventDispatcher Instance {
		get { return instance; }
	}

}