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

