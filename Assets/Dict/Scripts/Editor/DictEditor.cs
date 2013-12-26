using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Dict))]
public class DictEditor : Editor
{
    private const float H_OFFSET = 5f;



    public override void OnInspectorGUI()
    {
        Dict d = target as Dict;
        DictDrawer.DrawDictInspector(new Rect(H_OFFSET, 50, Screen.width - 2 * H_OFFSET, Screen.height), d);
    }
}