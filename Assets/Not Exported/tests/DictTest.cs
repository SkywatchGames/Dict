using UnityEngine;

public class DictTest : MonoBehaviour
{
    public Dict d;
    public int keyToTest = 0;

    void Start()
    {
        object value = d.GetValue(keyToTest);
        print(value);


        foreach (int key in d.Keys<int>())
            print(key + ": " + d.GetValue(key));
    }
}
