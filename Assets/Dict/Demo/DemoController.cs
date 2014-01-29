using UnityEngine;

public class DemoController : MonoBehaviour
{
    public GameObject colorHolder;

    public void ToggleColors()
    {
        colorHolder.SetActive(!colorHolder.activeSelf);
    }
}
