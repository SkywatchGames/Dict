using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Dict))]
public class DictDrawer : PropertyDrawer
{
    private const float ELEMENT_HEIGHT =    20f, KEY_VERTICAL_SPACING = 3f;
    private bool foldoutOpen = true;
    private const float H_MARGIN = 5f, V_MARGIN = 5f;


    //meu teste
    private bool allowCloning = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Dict d = GetDict(property);

        

        Rect allRect = new Rect(position);
        GUI.Box(allRect, string.Empty);

        Rect r = new Rect(position);
        r.height = ELEMENT_HEIGHT;

        foldoutOpen = EditorGUI.InspectorTitlebar(r, foldoutOpen, d);

        
        
        position.y += ELEMENT_HEIGHT + V_MARGIN;
        position.width -= 2 * H_MARGIN;
        position.x += H_MARGIN;

        if (foldoutOpen)
        {
            DrawDictInspector(position, d);
            if (allowCloning && GUI.Button(new Rect(r.x, r.y + GetPropertyHeight(property, null) - ELEMENT_HEIGHT, r.width, ELEMENT_HEIGHT), "Clone"))
            {
                property.objectReferenceValue = Object.Instantiate(d);
            }
        }
    }


    public static void DrawDictInspector(Rect r, Dict d)
    {
        d.KeyType = (Dict.Type)EditorGUI.EnumPopup(new Rect(r.x, r.y, r.width, ELEMENT_HEIGHT), "Key type", d.KeyType);
        d.ValueType = (Dict.Type)EditorGUI.EnumPopup(new Rect(r.x, r.y + ELEMENT_HEIGHT, r.width, ELEMENT_HEIGHT), "Value type", d.ValueType);

        GUI.Label(new Rect(r.x, r.y + 2 * ELEMENT_HEIGHT, r.width, r.height), "Key/Value table");


        float labelWidth = 40f;
        float buttonWidth = 20f;
        float objectFieldWidth = (r.width - 2 * labelWidth - buttonWidth) / 2f;

        Rect entryRect = new Rect(r.x, r.y + ELEMENT_HEIGHT * 3, labelWidth, ELEMENT_HEIGHT);
        for (int i = 0; i < d.KeyCount; i++)
        {
            Rect tempRect = new Rect(entryRect);

            GUI.Label(tempRect, "Key:");
            tempRect.x += tempRect.width;
            tempRect.width = objectFieldWidth;
            d._SetKeyAt(i, EditorField(tempRect, d._GetKeyAt(i), d.KeyType));

            tempRect.x += tempRect.width;
            tempRect.width = labelWidth;

            GUI.Label(tempRect, "Value:");
            tempRect.x += tempRect.width;
            tempRect.width = objectFieldWidth;
            d._SetValueAt(i, EditorField(tempRect, d._GetValueAt(i), d.ValueType));

            tempRect.x += tempRect.width;
            tempRect.width = buttonWidth;
            if (GUI.Button(tempRect, "X"))
            {
                d._RemoveAt(i);
                i--;
            }

            entryRect.y += ELEMENT_HEIGHT + KEY_VERTICAL_SPACING;
        }

        float tableBottomY = entryRect.y;

        const float BUTTONS_SPACING = 35f;
        buttonWidth = (r.width - BUTTONS_SPACING) / 2f;
        if (GUI.Button(new Rect(r.x, tableBottomY, buttonWidth, ELEMENT_HEIGHT), "+"))
            d._AddBlankEntry();
        if (GUI.Button(new Rect(r.x + buttonWidth + BUTTONS_SPACING, tableBottomY, buttonWidth, ELEMENT_HEIGHT), "Clear"))
            d.Clear();

        if (HasRepeatedKeys(d))
            EditorGUI.HelpBox(new Rect(r.x, tableBottomY + ELEMENT_HEIGHT + KEY_VERTICAL_SPACING, r.width, ELEMENT_HEIGHT), "There are repeated keys in the dictionary!", MessageType.Error);

        if (GUI.changed)
            EditorUtility.SetDirty(d);
    }

    

    private static bool HasRepeatedKeys(Dict d)
    {
        System.Collections.IList keys = d._GetKeyList();
        for (int i = 0; i < keys.Count - 1; i++)
            for (int j = i + 1; j < keys.Count; j++)
                if (keys[i] != null && keys[i].Equals(keys[j]))
                    return true;
        return false;
    }

    private static object EditorField(Rect r, object original, Dict.Type type)
    {
        switch (type)
        {
            case Dict.Type.STRING:
                return GUI.TextField(r, (string)original);
            case Dict.Type.INTEGER:
                return EditorGUI.IntField(r, (int)original);
            case Dict.Type.FLOAT:
                return EditorGUI.FloatField(r, (float)original);
            case Dict.Type.COLOR:
                return EditorGUI.ColorField(r, (Color)original);
        }
        return EditorGUI.ObjectField(r, (Object)original, typeof(Object), true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!foldoutOpen)
            return ELEMENT_HEIGHT;

        Dict d = GetDict(property);
        int keyCount = d.KeyCount;


        int elementsCount = 5;
        if (allowCloning)
            elementsCount++;

        float resp = keyCount * (ELEMENT_HEIGHT + KEY_VERTICAL_SPACING) + elementsCount * ELEMENT_HEIGHT + 2 * V_MARGIN;
        if (HasRepeatedKeys(d))
            resp += ELEMENT_HEIGHT;
        return resp;
    }

    private static Dict GetDict(SerializedProperty property)
    {
        Dict d = property.objectReferenceValue as Dict;
        if (d == null)
        {
            d = Dict.CreateInstance<Dict>();
            property.objectReferenceValue = d;
        }
        return d;
    }
}
