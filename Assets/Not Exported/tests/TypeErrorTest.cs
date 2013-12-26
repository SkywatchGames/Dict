using UnityEngine;
using System.Collections;

public class TypeErrorTest : MonoBehaviour {

    public Dict d;

    void Start()
    {
        
    }

    void OnGUI()
    {
        for (int i = 0; i < 4; i++)
        {
            if (GUILayout.Button("Test " + i))
            {
                int n = 10;
                switch (i)
                {
                    case 0:
                        d.GetValue(n);
                        break;
                    case 1:
                        d.Keys<int>();
                        break;
                    case 2:
                        d.Values<int>();
                        break;
                    case 3:
                        d.Contains(n);
                        break;
                }
            }
        }
    }
}
