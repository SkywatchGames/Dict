using UnityEngine;

namespace DictDemo
{
    public class Quad : MonoBehaviour
    {
        void OnTouched()
        {
            Color c = (renderer as SpriteRenderer).color;
            SpriteRenderer[] r = GameObject.Find("Character").GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer rend in r)
                rend.color = c;
        }
    }
}