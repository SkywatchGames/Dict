using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Dict))]
public class DictDrawer : PropertyDrawer
{
    private const float ELEMENT_HEIGHT =    20f;
    private bool foldoutOpen = true;
    private const float H_MARGIN = 5f, V_MARGIN = 5f;

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

        if(foldoutOpen)
            DrawDictInspector(position, d);
    }


    public static void DrawDictInspector(Rect r, Dict d)
    {
        d.KeyType = (Dict.Type)EditorGUI.EnumPopup(new Rect(r.x, r.y, r.width, ELEMENT_HEIGHT), "Key type", d.KeyType);
        d.ValueType = (Dict.Type)EditorGUI.EnumPopup(new Rect(r.x, r.y + ELEMENT_HEIGHT, r.width, ELEMENT_HEIGHT), "Value type", d.ValueType);

        GUI.Label(new Rect(r.x, r.y + 2 * ELEMENT_HEIGHT, r.width, r.height), "Key/Value table");


        float labelWidth = 40f;
        float buttonWidth = 20f;
        float objectFieldWidth = (r.width - 2 * labelWidth - buttonWidth) / 2f;
        for (int i = 0; i < d.KeyCount; i++)
        {
            Rect tempRect = new Rect(r.x, r.y + ELEMENT_HEIGHT * (i + 3), labelWidth, ELEMENT_HEIGHT);

            GUI.Label(tempRect, "Key:");
            tempRect.x += tempRect.width;
            tempRect.width = objectFieldWidth;
            d._SetKeyAt(i, EditorField(tempRect, d._GetKeyAt(i), d.KeyType));

            tempRect.x += tempRect.width;
            tempRect.width = labelWidth;

            //tempRect = new Rect(r.x + buttonWidth + objectFieldWidth, r.y + ELEMENT_HEIGHT * (i + 3), width5, ELEMENT_HEIGHT);
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
        }

        float tableBottomY = r.y + ELEMENT_HEIGHT * (d.KeyCount + 3);

        const float BUTTONS_SPACING = 50f;
        if (GUI.Button(new Rect(r.x, tableBottomY, r.width / 2f - BUTTONS_SPACING, ELEMENT_HEIGHT), "+"))
            d._AddBlankEntry();
        if (GUI.Button(new Rect(r.x + r.width / 2 + BUTTONS_SPACING, tableBottomY, r.width / 2f - BUTTONS_SPACING, ELEMENT_HEIGHT), "Clear all"))
            d.Clear();

        if (HasRepeatedKeys(d))
            EditorGUI.HelpBox(new Rect(r.x, tableBottomY + 2 * ELEMENT_HEIGHT, r.width, ELEMENT_HEIGHT), "There are repeated keys in the dictionary!", MessageType.Error);

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

        float resp = (keyCount + 5) * ELEMENT_HEIGHT + 2 * V_MARGIN;
        if (HasRepeatedKeys(d))
            resp += 2 * ELEMENT_HEIGHT;
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
