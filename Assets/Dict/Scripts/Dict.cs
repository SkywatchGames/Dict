using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable 649

/// <summary>
/// Dictionary class that maps basic key types to other types in a one-to-many relationship.
/// Although no error will be thrown, repeated keys will trigger incorrect behaviour.
/// </summary>
public class Dict : ScriptableObject
{
    public enum Type { STRING, INTEGER, FLOAT, OBJECT, COLOR}

    [SerializeField]
    private Type keyType = Type.STRING, valueType = Type.STRING;

    [SerializeField]
    private List<string> s_keys, s_values;
    [SerializeField]
    private List<int> i_keys, i_values;
    [SerializeField]
    private List<float> f_keys, f_values;
    [SerializeField]
    private List<Object> o_keys, o_values;
    [SerializeField]
    private List<Color> c_keys, c_values;

    /// <summary>
    /// Returns the type of the keys.
    /// </summary>
    public Type KeyType
    {
        get
        {
            return keyType;
        }
        set
        {
            Type oldType = keyType;
            keyType = value;
            if (value != oldType)
                Clear();
        }
    }

    /// <summary>
    /// Returns the type of the values.
    /// </summary>
    public Type ValueType
    {
        get
        {
            return valueType;
        }
        set
        {
            Type oldType = valueType;
            valueType = value;
            if (value != oldType)
                Clear();
        }
    }

    public T Get<T>(object key)
    {
        if (!key.GetType().Equals(InnerKeyType))
            throw new System.Exception(string.Format("Incorrect key type: expected {0} but got {1}", KeyType, key.GetType()));
        if (!typeof(T).Equals(InnerValueType))
            throw new System.Exception(string.Format("Incorrect value type: expected {0} but got {1}", InnerValueType, typeof(T)));

        return (T)GetValue(key);
    }

    public int KeyCount
    {
        get
        {
            return _GetKeyList().Count;
        }
    }

    public object GetValue(object key)
    {
        if (!key.GetType().Equals(InnerKeyType))
            throw new System.Exception(string.Format("Incorrect key type: expected {0} but got {1}", InnerKeyType, key.GetType()));
        
        int index = 0;
        index = _GetKeyList().IndexOf(key);
        object o = GetValueList()[index];
        return o;
    }

    /// <summary>
    /// Erases all entries.
    /// </summary>
    public void Clear()
    {
        IList[] lists = 
        { 
            s_keys, i_keys, f_keys, o_keys, c_keys,
            s_values, i_values, f_values, o_values, c_values
        };

        foreach (IList l in lists)
            l.Clear();
    }

    public bool Contains(object key)
    {
        if (!key.GetType().Equals(InnerKeyType))
            throw new System.Exception(string.Format("Incorrect key type: expected {0} but got {1}", InnerKeyType, key.GetType()));

        return _GetKeyList().Contains(key);
    }

    public IEnumerable<T> Keys<T>()
    {
        if (!typeof(T).Equals(InnerKeyType))
            throw new System.Exception(string.Format("Incorrect key type: expected {0} but got {1}", InnerKeyType, typeof(T)));

        return _GetKeyList() as IEnumerable<T>;
    }

    public IEnumerable<T> Values<T>()
    {
        if (!typeof(T).Equals(InnerValueType))
            throw new System.Exception(string.Format("Incorrect value type: expected {0} but got {1}", InnerValueType, typeof(T)));

        return GetValueList() as IEnumerable<T>;
    }

    #region GUI_EDITOR_METHODS
    public IList _GetKeyList()
    {
        switch (keyType)
        {
            case Type.STRING:
                return s_keys;
            case Type.INTEGER:
                return i_keys;
            case Type.FLOAT:
                return f_keys;
            case Type.COLOR:
                return c_keys;
        }
        return o_keys;
    }

    public object _GetKeyAt(int index)
    {
        object value = _GetKeyList()[index];
        if (value == null)
        {
            switch (keyType)
            {
                case Type.STRING:
                    return string.Empty;
                case Type.INTEGER:
                case Type.FLOAT:
                    return 0;
                case Type.COLOR:
                    return Color.black;
            }
        }
        return value;
    }

    public object _GetValueAt(int index)
    {
        object value = GetValueList()[index];
        if (value == null)
        {
            switch (valueType)
            {
                case Type.STRING:
                    return string.Empty;
                case Type.INTEGER:
                case Type.FLOAT:
                    return 0;
                case Type.COLOR:
                    return Color.black;
            }
        }
        return value;
    }

    public void _SetKeyAt(int index, object key)
    {
        _GetKeyList()[index] = key;
    }

    public void _SetValueAt(int index, object key)
    {
        GetValueList()[index] = key;
    }

    public void _AddBlankEntry()
    {
        AddBlankElement(_GetKeyList(), KeyType);
        AddBlankElement(GetValueList(), ValueType);
    }



    public void _RemoveAt(int i)
    {
        _GetKeyList().RemoveAt(i);
        GetValueList().RemoveAt(i);
    }
    #endregion

    #region PRIVATE_METHODS
    private System.Type InnerKeyType
    {
        get
        {
            switch (KeyType)
            {
                case Type.STRING:
                    return typeof(string);
                case Type.INTEGER:
                    return typeof(int);
                case Type.FLOAT:
                    return typeof(float);
                case Type.COLOR:
                    return typeof(Color);
            }
            return typeof(Object);
        }
    }

    private System.Type InnerValueType
    {
        get
        {
            switch (ValueType)
            {
                case Type.STRING:
                    return typeof(string);
                case Type.INTEGER:
                    return typeof(int);
                case Type.FLOAT:
                    return typeof(float);
                case Type.COLOR:
                    return typeof(Color);
            }
            return typeof(Object);
        }
    }

    void OnEnable()
    {
        //acabou de criar o Dict
        //se não estava serializado, inicializa as listas

        if (s_keys == null)
        {
            s_keys = new List<string>();
            s_values = new List<string>();

            i_keys = new List<int>();
            i_values = new List<int>();

            f_keys = new List<float>();
            f_values = new List<float>();

            o_keys = new List<Object>();
            o_values = new List<Object>();

            c_keys = new List<Color>();
            c_values = new List<Color>();
        }
    }

    private IList GetValueList()
    {
        switch (valueType)
        {
            case Type.STRING:
                return s_values;
            case Type.INTEGER:
                return i_values;
            case Type.FLOAT:
                return f_values;
            case Type.COLOR:
                return c_values;
        }
        return o_values;
    }

    private void AddBlankElement(IList list, Type t)
    {
        if (t == Type.INTEGER)
            list.Add(0);
        else if (t == Type.FLOAT)
            list.Add(0f);
        else if (t == Type.STRING)
            list.Add(string.Empty);
        else if (t == Type.COLOR)
            list.Add(Color.black);
        else
            list.Add(null);
    }
    #endregion
}

#pragma warning restore 649