using UnityEngine;

public class DictTest : MonoBehaviour
{
    public Dict d;
    //public string keyToTest = "oi";
    public int keyToTest = 0;

    void Start()
    {
        //print(d.Get<int>(keyToTest));
        object value = d.GetValue(keyToTest);
        print(value);


        foreach (int key in d.Keys<int>())
            print(key + ": " + d.GetValue(key));
    }
}
