/*  Copyright (C) 2014 Skywatch Entretenimento Digital LTDA - ME
    This is free software. Please refer to LICENSE for more information. */

using UnityEngine;

namespace DictDemo
{
    public class GUIBtn : MonoBehaviour
    {
        public Controller ctrl;
        public string controllerMsg;
        public Sprite[] sprites;

        
        private int currSprite = 0;


        void OnTouched()
        {
            ctrl.SendMessage(controllerMsg);
            currSprite = Mathf.Abs(1 - currSprite);
            GetComponent<SpriteRenderer>().sprite = sprites[currSprite];
        }
    }

}

