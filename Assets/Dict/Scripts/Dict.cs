/*  Copyright (C) 2014 Skywatch Entretenimento Digital LTDA - ME
    This is free software. Please refer to LICENSE for more information. */

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
    public enum Type { 
        /// <summary>
        /// The built-in string type.
        /// </summary>
        STRING,

        /// <summary>
        /// The built-in integer type.
        /// </summary>
        INTEGER,

        /// <summary>
        /// The built-in single precision float type.
        /// </summary>
        FLOAT,

        /// <summary>
        /// The UnityEngine.Object type. Use this for GameObjects, Components, ScriptableObjects etc.
        /// </summary>
        OBJECT,
        /// <summary>
        /// The UnityEngine.Color type.
        /// </summary>
        COLOR
    }

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
    /// Retrieves the value associated to the given key.
    /// </summary>
    /// <typeparam name="V">Desired return type. Should be the same as this Dict's value type; otherwise, an exception will be thrown.</typeparam>
    /// <param name="key">Key object to be searched. The associated value will be returned. In case no value is retured, an exception will be thrown.</param>
    /// <returns></returns>
    public V Get<V>(object key)
    {
        ValidateKey(key);
        ValidateValueType(typeof(V));

        return (V)GetValue(key);
    }

    /// <summary>
    /// Adds or updates the entry for the specified key.
    /// </summary>
    /// <param name="key">Key to be added or updated.</param>
    /// <param name="value">The value to be associated to the key.</param>
    public void Set(object key, object value)
    {
        ValidateKey(key);
        ValidateValue(value);

        if (IsFloat(InnerKeyType)) //evitar problema de System.Single e System.Float
            key = System.Convert.ToSingle(key);
        if (IsFloat(InnerValueType))
            value = System.Convert.ToSingle(value);

        if (!Contains(key))
        {
            _GetKeyList().Add(key);
            GetValueList().Add(value);
        }
        else
        {
            int index = _GetKeyList().IndexOf(key);
            GetValueList()[index] = value;
        }
    }

    /// <summary>
    /// Returns the number of entries present in the dictionary.
    /// </summary>
    public int KeyCount
    {
        get
        {
            return _GetKeyList().Count;
        }
    }


    /// <summary>
    /// Returns an untyped value associated to a key.
    /// </summary>
    /// <param name="key">Key to be searched.</param>
    /// <returns>The value associated to the given key. Throws an exception if there isn't such key in the dictionary.</returns>
    public object GetValue(object key)
    {
        ValidateKey(key);
        
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
    /// Checks if this dictionary has an entry for the given key.
    /// </summary>
    /// <param name="key">Key to be checked.</param>
    /// <returns>True if there is an entry with the specified key.</returns>
    public bool Contains(object key)
    {
        //ValidateKeyType(key);
        return _GetKeyList().Contains(key);
    }

    /// <summary>
    /// Returns an enumerator with all the keys.
    /// </summary>
    /// <typeparam name="T">Expected key type. Must be the same as the dict key type.</typeparam>
    /// <returns></returns>
    public IEnumerable<K> Keys<K>()
    {
        ValidateKeyType(typeof(K), TypePolicy.STRICT);
        return _GetKeyList() as IEnumerable<K>;
    }

    /// <summary>
    /// Returns an enumerator with all the values.
    /// </summary>
    /// <typeparam name="V">Expected value type. Must be the same as the dict value type.</typeparam>
    /// <returns></returns>
    public IEnumerable<V> Values<V>()
    {
        ValidateValueType(typeof(V), TypePolicy.STRICT);
        return GetValueList() as IEnumerable<V>;
    }

    /// <summary>
    /// Retrieves a KeyValuePair enumerator. When used in a 'foreach loop', must be called explicitly.
    /// </summary>
    /// <typeparam name="K">Key type.</typeparam>
    /// <typeparam name="V">Value type.</typeparam>
    /// <returns>A KeyValueEnumerator.</returns>
    public IEnumerable<KeyValuePair<K, V>> GetEnumerator<K, V>()
    {
        ValidateKeyType(typeof(K), TypePolicy.STRICT);
        ValidateValueType(typeof(V), TypePolicy.STRICT);

        List<KeyValuePair<K, V>> pairList = new List<KeyValuePair<K, V>>();
        foreach (K key in this.Keys<K>())
        {
            pairList.Add(new KeyValuePair<K, V>(key, this.Get<V>(key)));
        }
        return pairList;
    }

    /// <summary>
    /// Removes the entry associated to the given key.
    /// </summary>
    /// <param name="key">Key to be removed.</param>
    /// <returns>True if the element was present. False otherwise.</returns>
    public bool Remove(object key)
    {
        ValidateKey(key);
        int index = _GetKeyList().IndexOf(key);
        if (index == -1) 
            return false;
        else
        {
            _RemoveAt(index);
            return true;
        }
    }

    #region TYPE_CHECK_LOGIC

    private void ValidateKey(object key, TypePolicy policy = TypePolicy.ALLOW_SUBTYPES)
    {
        bool nulltypeOk = key == null && (KeyType == Type.OBJECT || KeyType == Type.STRING);

        if(!nulltypeOk) //null keys are ok for objects and strings
            ValidateKeyType(key.GetType(), policy);
    }

    private void ValidateValue(object value, TypePolicy policy = TypePolicy.ALLOW_SUBTYPES)
    {
        bool nulltypeOk = value == null && (ValueType == Type.OBJECT || ValueType == Type.STRING);

        if(!nulltypeOk) //null keys are ok for objects and strings
            ValidateValueType(value.GetType(), policy);
    }

    private void ValidateKeyType(System.Type givenType, TypePolicy policy = TypePolicy.ALLOW_SUBTYPES)
    {
        if (!ValidateTypes(givenType, InnerKeyType, policy))
            throw new System.Exception(string.Format("Incorrect key type: expected {0} but got {1}", InnerKeyType, givenType));
    }

    private void ValidateValueType(System.Type givenType, TypePolicy policy = TypePolicy.ALLOW_SUBTYPES)
    {
        if (!ValidateTypes(givenType, InnerValueType, policy))
            throw new System.Exception(string.Format("Incorrect value type: expected {0} but got {1}", InnerValueType, givenType));
    }

    private static bool ValidateTypes(System.Type given, System.Type expected, TypePolicy policy)
    {
        bool typesAlike = IsInteger(given) && IsInteger(expected); //verifica se são inteiros
        bool areEquals = given.Equals(expected);                   //verifica se são exatamente iguais
        bool fitSubclass = policy == TypePolicy.ALLOW_SUBTYPES && (given.IsSubclassOf(expected) || IsInteger(given) && IsNumber(expected)); //verifica se são subclasse, se permitido
        

        return typesAlike || areEquals || fitSubclass;
    }

    private static bool IsInteger(System.Type t)
    {
        return t.Equals(typeof(int)) || t.Equals(typeof(byte)) || t.Equals(typeof(short));
    }

    private static bool IsFloat(System.Type t)
    {
        return t.Equals(typeof(float)) || t.Equals(typeof(System.Single)) || t.Equals(typeof(double));
    }

    private static bool IsNumber(System.Type t)
    {
        return IsFloat(t) || IsInteger(t);
    }

    private enum TypePolicy { STRICT, ALLOW_SUBTYPES}

    #endregion

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