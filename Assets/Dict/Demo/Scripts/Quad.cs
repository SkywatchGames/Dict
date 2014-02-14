using UnityEngine;

namespace DictDemo
{
    public class Quad : MonoBehaviour
    {
        void OnMouseDown()
        {
            Color c = (renderer as SpriteRenderer).color;
            SpriteRenderer[] r = GameObject.Find("Character").GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer rend in r)
                rend.color = c;
        }
    }
}