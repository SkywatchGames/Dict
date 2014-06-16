/*  Copyright (C) 2014 Skywatch Entretenimento Digital LTDA - ME
    This is free software. Please refer to LICENSE for more information. */

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