using UnityEngine;

public class ColourBtn : MonoBehaviour
{
    public DemoController ctrl;
    public Sprite[] sprites;
    private int currSprite = 0;

    void OnMouseDown()
    {
        ctrl.ToggleColors();
        currSprite = Mathf.Abs(1 - currSprite);
        GetComponent<SpriteRenderer>().sprite = sprites[currSprite];
    }
}
