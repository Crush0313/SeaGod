using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleWig : MonoBehaviour
{

    RectTransform rectTransform;
    float frame = 0f;
    float firX;
    float firY;
    public float g = 0.1f;
    public float bae = 10;
    public static bool on = true;

    public void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        firX = rectTransform.position.x;
        firY = rectTransform.position.y;
        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        while (on == true)
        {
            frame += g;
            rectTransform.position = new Vector2(firX, firY + Mathf.Sin(frame) * bae);
            yield return new WaitForSeconds(0.015f);
        }
    }

}
