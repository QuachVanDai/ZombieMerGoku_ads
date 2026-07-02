using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EventDataBinder : MonoBehaviour
{
    public string eventName;
    public Component component;
    public string field;
    public StringFilter formatter;
    public bool format;

    EventName EventName;
    Member member;

    void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(EventName));
        int index = Array.IndexOf(names, eventName);
        if (index < 0)
        {
            EventName = EventName.NONE;
        }
        else
        {
            EventName = (EventName)Enum.GetValues(typeof(EventName)).GetValue(index);
        }

        if (EventName == EventName.NONE || component == null || string.IsNullOrEmpty(field))
        {
            Debug.LogErrorFormat("Either event type, component or target field is not valid");
            return;
        }

        try
        {
            member = new Member(component.GetType(), field, true);
            EventDispatcher.Instance.AddListener(EventName, OnEvent);
        }
        catch (Exception ex)
        {
            Debug.LogErrorFormat("Cannot find member: {0} {1}", ex.Message, ex.StackTrace);
        }
    }

    void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener(EventName, OnEvent);
    }

    void OnEvent(EventName key, object data)
    {
        if (member != null && component != null)
        {
            if (format && formatter != null)
            {
                data = formatter.Filter(data);
            }
            member.SetValue(component, data);
        }
    }

    [Serializable]
    public class FilterData
    {
        public string className;
        public string data;
    }

    [Serializable]
    public abstract class AbstractFilter
    {
        public abstract System.Object Filter(System.Object data);
    }

    [Serializable]
    public class StringFilter : AbstractFilter
    {
        public string format = "{0}";

        public override object Filter(object data)
        {
            return string.Format(format, data);
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(EventDataBinder))]
public class EventDataBinderEditor : Editor
{
    EventDataBinder binder;

    private void OnEnable()
    {
        binder = target as EventDataBinder;
    }

    public override void OnInspectorGUI()
    {
        // Event Type
        string[] options = Enum.GetNames(typeof(EventName));
        int selected = Array.IndexOf(options, binder.eventName);
        int selection = EditorGUILayout.Popup("Event", selected, options);
        if (selection != selected && selection >= 0)
        {
            binder.eventName = options[selection];
            EditorUtility.SetDirty(binder);
        }

        // Component
        var components = binder.GetComponentsInChildren<Component>();
        selected = Array.IndexOf(components, binder.component);
        selection = EditorGUILayout.Popup("Traget Component", selected, components.Select(i => i.ToString()).ToArray());
        if (selected != selection)
        {
            binder.component = components[selection];
            EditorUtility.SetDirty(binder);
        }

        // Field
        if (binder.component != null)
        {
            var members = ReflectionUtils.GetInstanceMembers(binder.component.GetType(), typeof(System.Object), false, true, true);
            selected = members.IndexOf(binder.field);
            selection = EditorGUILayout.Popup("Target Field", selected, members.ToArray());
            if (selected != selection)
            {
                binder.field = members[selection];
                EditorUtility.SetDirty(binder);
            }
        }
        else
        {
            EditorGUILayout.Popup("Target Field", -1, new string[0]);
        }

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();
        {
            binder.format = EditorGUILayout.Toggle("Format", binder.format);
            if (binder.format)
            {
                if (binder.formatter == null)
                {
                    binder.formatter = new EventDataBinder.StringFilter();
                }
                binder.formatter.format = EditorGUILayout.TextField(binder.formatter.format);
            }    
        }
        EditorGUILayout.EndHorizontal();
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(binder);
        }
    }
}
#endif