using UnityEngine;

public class DictDemo : MonoBehaviour
{
    public Dict d;

    public GameObject quadPrefab;
    public Transform quadHolder;

    void Start()
    {
        int i = 0;
        foreach (string colorName in d.Keys<string>())
        {
            Color c = d.Get<Color>(colorName);
            GameObject obj = Instantiate(quadPrefab) as GameObject;
            Material mat = obj.renderer.material;
            mat.color = c;

            obj.GetComponentInChildren<TextMesh>().text = colorName;
            obj.transform.Translate(Vector3.down * i / 3.0f);
            obj.transform.parent = quadHolder;
            i++;
        }

        
    }
}
