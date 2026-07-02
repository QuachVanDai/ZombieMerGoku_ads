using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;

public class ReflectionUtils
{

	public static string[] GetStaticStringFieldValues (Type type)
	{
		var names = new List<string> ();
		var properties = type.GetFields (BindingFlags.Public | BindingFlags.Static);
		foreach (var info in properties) {
			if (info.FieldType == typeof(string)) 
			{
				names.Add ((string)info.GetValue(null));	
			}
		}

		var query = names.GroupBy(x => x)
			.Where(g => g.Count() > 1)
			.Select(y => y.Key)
			.ToList();
		if (query.Count > 0) {
			Debug.LogErrorFormat ("Duplicate Keys {0}", query.Aggregate ("", (i, j) => i + " " + j));
			return new string[0];
		} else {
			return names.ToArray ();
		}
	}

    public static List<string> GetInstanceFields<T, K>(bool includeNonPublic=true, bool inherit=true)
    {
        List<string> names = new List<string>();
        var flags = BindingFlags.Instance | BindingFlags.Public;
        if (includeNonPublic) flags |= BindingFlags.NonPublic;

        var fields = typeof(K).GetFields(flags);
        foreach(var f in fields)
        {
            if (inherit && typeof(T).IsAssignableFrom(f.FieldType))
            {
                names.Add(f.Name);
            }
            else if (typeof(T) == f.FieldType)
            {
                names.Add(f.Name);
            }
        }

        return names;
    }

    public static List<string> GetInstanceFields(Type type, Type fieldType, bool includeNonPublic = true, bool covariance = false)
    {
        List<string> names = new List<string>();
        var flags = BindingFlags.Instance | BindingFlags.Public;
        if (includeNonPublic) flags |= BindingFlags.NonPublic;

        var fields = type.GetFields(flags);
        foreach (var f in fields)
        {
            if ((covariance && fieldType.IsAssignableFrom(f.FieldType))
                || (!covariance && f.FieldType.IsAssignableFrom(fieldType))
               )
            {
                names.Add(f.Name);
            }
        }

        return names;
    }

    public static List<string> GetInstanceProperties(Type type, Type propertyType, bool includeNonPublic = true, bool covariance = false, bool canWrite=true)
    {
        List<string> names = new List<string>();
        var flags = BindingFlags.Instance | BindingFlags.Public;
        if (includeNonPublic) flags |= BindingFlags.NonPublic;

        var fields = type.GetProperties(flags);
        foreach (var f in fields)
        {
            if ((covariance && propertyType.IsAssignableFrom(f.PropertyType))
                || (!covariance && f.PropertyType.IsAssignableFrom(propertyType)))
            {
                if (!canWrite || f.CanWrite)
                {
                    names.Add(f.Name);   
                }
            }
        }

        return names;
    }

    public static List<string> GetInstanceMembers (Type type, Type memberType, bool includeNonPublic = true, bool covariance = false, bool canWrite = true)
    {
        return GetInstanceFields(type, memberType, includeNonPublic, covariance).Concat(GetInstanceProperties(type, memberType, includeNonPublic, covariance, canWrite)).ToList();
    }
}
