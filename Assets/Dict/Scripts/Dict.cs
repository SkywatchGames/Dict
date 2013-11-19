using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable 649

public class Dict : ScriptableObject
{
    public enum Type { STRING, INTEGER, FLOAT, OBJECT }

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

    void OnEnabled()
    {
        name = "Dictionary";
    }

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
            {
                ClearAll();
            }
        }
    }

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
            {
                ClearAll();
            }
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
        }
    }

    public T Get<T>(object key)
    {
        return (T)GetValue(key);
    }

    public int KeyCount
    {
        get
        {
            //Debug.Log("get keycount for " + KeyType + " returning " + GetKeyList().Count);
            return GetKeyList().Count;
        }
    }

    public IList GetKeyList()
    {
        switch (keyType)
        {
            case Type.STRING:
                return s_keys;
            case Type.INTEGER:
                return i_keys;
            case Type.FLOAT:
                return f_keys;
        }
        return o_keys;
    }

    public IList GetValueList()
    {
        switch (valueType)
        {
            case Type.STRING:
                return s_values;
            case Type.INTEGER:
                return i_values;
            case Type.FLOAT:
                return f_values;
        }
        return o_values;
    }

    public object GetValue(object key)
    {
        
        int index = 0;
        index = GetKeyList().IndexOf(key);
        object o = GetValueList()[index];
        return o;
    }

    public object GetKeyAt(int index)
    {
        object value = GetKeyList()[index];
        if (value == null)
        {
            switch (keyType)
            {
                case Type.STRING:
                    return string.Empty;
                case Type.INTEGER:
                case Type.FLOAT:
                    return 0;
            }
        }
        return value;
    }

    public object GetValueAt(int index)
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
            }
        }
        return value;
    }

    public void SetKeyAt(int index, object key)
    {
        GetKeyList()[index] = key;
    }

    public void SetValueAt(int index, object key)
    {
        GetValueList()[index] = key;
    }

    public void AddBlankEntry()
    {
        AddBlankElement(GetKeyList(), KeyType);
        AddBlankElement(GetValueList(), ValueType);
    }

    private void AddBlankElement(IList list, Type t)
    {
        if (t == Type.INTEGER)
            list.Add(0);
        else if (t == Type.FLOAT)
            list.Add(0f);
        else if (t == Type.STRING)
            list.Add(string.Empty);
        else
            list.Add(null);
    }

    public void RemoveAt(int i)
    {
        s_keys.RemoveAt(i);
        s_values.RemoveAt(i);
    }

    public void Clear()
    {
        GetKeyList().Clear();
        GetValueList().Clear();
    }

    public void ClearAll()
    {
        s_keys.Clear();
        i_keys.Clear();
        f_keys.Clear();
        o_keys.Clear();

        s_values.Clear();
        i_values.Clear();
        f_values.Clear();
        o_values.Clear();
    }

    public bool Contains(object key)
    {
        return GetKeyList().Contains(key);
    }

    public IEnumerable<T> Keys<T>()
    {
        return GetKeyList() as IEnumerable<T>;
    }

    public IEnumerable<T> Values<T>()
    {
        return GetValueList() as IEnumerable<T>;
    }
}

#pragma warning restore 649