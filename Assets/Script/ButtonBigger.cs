using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBigger : MonoBehaviour
{
    public float dur = 0.1f;
    public void PointerEnter()
    {
        //transform.localScale = new Vector2(1.2f, 1.2f);
        LeanTween.scale(gameObject, new Vector2(1.1f, 1.1f), dur);
    }
    public void PointerExit()
    {
        //transform.localScale = new Vector2(1f, 1f);
        LeanTween.scale(gameObject, new Vector2(1f, 1f), dur);
    }
}
