using UnityEngine;

namespace DictDemo
{
    public class Controller : MonoBehaviour
    {
        public Dict d;

        void Start()
        {
			SetupDict();
			SetupGUI();
        }

		private void SetupGUI(){
			codeHolder.SetActive(false);
		}

		private void SetupDict(){
			int i = 0;
			foreach (string colorName in d.Keys<string>()) //iterate over the string keys
			{
				Color c = d.Get<Color>(colorName); //retrieve a color value
				
				
				GameObject obj = Instantiate(quadPrefab) as GameObject;
                (obj.renderer as SpriteRenderer).color = c;
				
				obj.GetComponentInChildren<TextMesh>().text = colorName;
				obj.transform.Translate(Vector3.down * i / 3.0f);
				obj.transform.parent = quadHolder;
				i++;
			}
		}

        public void ToggleColors()
        {
            colorHolder.SetActive(!colorHolder.activeSelf);
        }

        public void ToggleCode()
        {
            codeHolder.SetActive(!codeHolder.activeSelf);
            if (codeHolder.activeSelf)
                CameraTween.Do(gameObject, originalCamera, codeCamera);
            else
                CameraTween.Do(gameObject, codeCamera, originalCamera);
        }

		public GameObject colorHolder, codeHolder;
		public GameObject quadPrefab;
		public Transform quadHolder;
        public Camera originalCamera, codeCamera;
    }
}