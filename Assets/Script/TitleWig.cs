using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleWig : MonoBehaviour
{

    RectTransform rectTransform;
    float frame = 0f;
    float firX;
    float firY;
    public float g = 0.2f;//프레임 단위
    public float bae = 15;//진폭, 프레임 수는 변하지 않기 때문에 끊겨 보일 수도 
    public float delay = 0.015f;//다음 프레임까지의 시간
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
            yield return new WaitForSeconds(delay);
        }
    }

}
