using System;
using System.Reflection;

public class Member
{
    private Kind type;
    private FieldInfo fieldInfo;
    private PropertyInfo propertyInfo;

    public Member(Type type, string name, bool writable)
    {
        fieldInfo = type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        if (fieldInfo != null)
        {
            this.type = Kind.FIELD;
        }
        else
        {
            propertyInfo = type.GetProperty(name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            this.type = Kind.PROPERTY;
            if (propertyInfo == null)
            {
                throw new SystemException(string.Format("Cannot find field or property named \"{0}\" of type {1}", name, type));
            }

            if (writable && !propertyInfo.CanWrite)
            {
                throw new SystemException(string.Format("Property \"{0}\" of {1} is not writable", name, type));
            }
        }
    }

    public Type NodeType()
    {
        return type == Kind.FIELD ? fieldInfo.FieldType : propertyInfo.PropertyType;
    }

    public void SetValue(object target, object value)
    {
        if (type == Kind.FIELD)
            fieldInfo.SetValue(target, value);
        else
            propertyInfo.SetValue(target, value, null);
    }

    public object GetValue(object target)
    {
        return type == Kind.FIELD ? fieldInfo.GetValue(target) : propertyInfo.GetValue(target, null);
    }

    public override string ToString()
    {
        return string.Format("[Member: kind = {0}, type = {1}]", type, NodeType());
    }

    public enum Kind
    {
        FIELD,
        PROPERTY
    }
}
