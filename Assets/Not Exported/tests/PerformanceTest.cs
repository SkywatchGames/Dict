using UnityEngine;
using System.Collections;

public class PerformanceTest : MonoBehaviour {

    public Dict d;

    void Update()
    {
        d.Get<int>("abacaxi");
    }
}
