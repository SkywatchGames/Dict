using UnityEngine;
using System.Collections;

public class Sine : MonoBehaviour {

    public float magnitude;
    public float duration = 3f;

    public Vector3 direction;
    private float elapsed = 0;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    public void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= duration)
            elapsed -= duration;

        transform.position = startPos + direction.normalized * Mathf.Sin(2f * Mathf.PI * elapsed / duration) * magnitude;
    }
}
