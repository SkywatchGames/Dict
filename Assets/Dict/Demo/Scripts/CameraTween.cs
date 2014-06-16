/*  Copyright (C) 2014 Skywatch Entretenimento Digital LTDA - ME
    This is free software. Please refer to LICENSE for more information. */

using UnityEngine;

public class CameraTween : MonoBehaviour
{
    private float duration = 1f;
    private Camera src, dest;

    private float elapsed = 0;

    void Update()
    {
        elapsed += Time.deltaTime;

        float pctg = Mathf.Clamp01(elapsed / duration);

        Vector3 pos = Vector3.Lerp(src.transform.position, dest.transform.position, pctg);
        float frustrum = Mathf.Lerp(src.orthographicSize, dest.orthographicSize, pctg);

        Camera.main.orthographicSize = frustrum;
        Camera.main.transform.position = pos;

        if (elapsed >= duration)
            Destroy(this);
    }

    public static void Do(GameObject target, Camera src, Camera dest)
    {
        CameraTween tween = target.AddComponent<CameraTween>();
        tween.src = src;
        tween.dest = dest;
    }
}
