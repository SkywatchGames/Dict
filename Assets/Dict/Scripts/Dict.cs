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
    /// <summary>
    /// This enum represents the object types that can be used as key or values.
    /// </summary>
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
    /// Returns or sets the type of the keys. In case the type changes, all entires from the dictionary are removed.
    /// </summary>
    ///
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
    /// Returns or sets the type of the values. In case the type changes, all entires from the dictionary are removed.
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

    /// <summary>
    /// Retrieve the value associated to the given key.
    /// </summary>
    /// <typeparam name="V">Desired return type. Should be the same as this Dict's value type; otherwise, an exception will be thrown.</typeparam>
    /// <param name="key">Key object to be searched. The associated value will be returned. In case no value is retured, an exception will be thrown.</param>
    /// <returns></returns>
    public V Get<V>(object key)
    {
        if (!key.GetType().Equals(InnerKeyType))
            throw new System.Exception(string.Format("Incorrect key type: expected {0} but got {1}", KeyType, key.GetType()));
        if (!typeof(V).Equals(InnerValueType))
            throw new System.Exception(string.Format("Incorrect value type: expected {0} but got {1}", InnerValueType, typeof(V)));

        return (V)GetValue(key);
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
        if (index == -1)
            throw new System.Exception(string.Format("Key {0} was not found in the dictionary.", key));

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

    /// <summary>
    /// Checks of this dictionary has an entry for the given key.
    /// </summary>
    /// <param name="key">Key to be checked.</param>
    /// <returns>True if there is an entry with the specified key.</returns>
    public bool Contains(object key)
    {
        if (!key.GetType().Equals(InnerKeyType))
            throw new System.Exception(string.Format("Incorrect key type: expected {0} but got {1}", InnerKeyType, key.GetType()));

        return _GetKeyList().Contains(key);
    }

    /// <summary>
    /// Returns an enumerator with all the keys.
    /// </summary>
    /// <typeparam name="T">Expected key type. Must be the same as the dict key type.</typeparam>
    /// <returns></returns>
    public IEnumerable<K> Keys<K>()
    {
        if (!typeof(K).Equals(InnerKeyType))
            throw new System.Exception(string.Format("Incorrect key type: expected {0} but got {1}", InnerKeyType, typeof(K)));

        return _GetKeyList() as IEnumerable<K>;
    }

    /// <summary>
    /// Returns an enumerator with all the values.
    /// </summary>
    /// <typeparam name="V">Expected value type. Must be the same as the dict value type.</typeparam>
    /// <returns></returns>
    public IEnumerable<V> Values<V>()
    {
        if (!typeof(V).Equals(InnerValueType))
            throw new System.Exception(string.Format("Incorrect value type: expected {0} but got {1}", InnerValueType, typeof(V)));

        return GetValueList() as IEnumerable<V>;
    }

    /// <summary>
    /// Retrieves a KeyValuePair enumerator.
    /// </summary>
    /// <typeparam name="K">Key type.</typeparam>
    /// <typeparam name="V">Value type.</typeparam>
    /// <returns></returns>
    public IEnumerable<KeyValuePair<K, V>> GetEnumerator<K, V>()
    {
        if (!typeof(K).Equals(InnerKeyType))
            throw new System.Exception(string.Format("Incorrect key type: expected {0} but got {1}", InnerKeyType, typeof(K)));
        if (!typeof(V).Equals(InnerValueType))
            throw new System.Exception(string.Format("Incorrect value type: expected {0} but got {1}", InnerValueType, typeof(V)));

        List<KeyValuePair<K, V>> pairList = new List<KeyValuePair<K, V>>();
        foreach (K key in this.Keys<K>())
        {
            pairList.Add(new KeyValuePair<K, V>(key, this.Get<V>(key)));
        }
        return pairList;
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