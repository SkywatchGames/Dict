using UnityEngine;

namespace DictDemo
{
    public class TouchReceiver : MonoBehaviour
    {
        private void Update()
        {
            if (InputPressed)
            {
                //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit hit;
                Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(r, out hit))
                {
                    if (hit.collider == collider)
                        SendMessage("OnTouched", SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        private bool InputPressed
        {
            get
            {
                foreach (Touch t in Input.touches)
                    if (t.phase == TouchPhase.Began)
                        return true;
                return Input.GetMouseButtonDown(0);
            }
        }
    }
}