using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovingCard : MonoBehaviour
{
    public Transform box1;
    public Transform box2;
    public Transform box3;

    public CanvasGroup background;

    public Transform box4;
    public GameObject window;

    public Image obj;


    // Start is called before the first frame update

    private void Start()
    {
        background.alpha = 0;

        box1.localPosition = new Vector2(-740, -Screen.height - 160);
        box2.localPosition = new Vector2(0, -Screen.height - 160);
        box3.localPosition = new Vector2(740, -Screen.height - 160);
        obj.material.SetFloat("_Size", 0f);
    }

    public void Opened()
    {
        background.LeanAlpha(0.4f, 1f);
        obj.material.SetFloat("_Size", 10f);
        box1.LeanMoveLocalY(0, 0.8f).setEaseOutExpo().delay = 0.1f;
        box2.LeanMoveLocalY(0, 0.8f).setEaseOutExpo().delay = 0.2f;
        box3.LeanMoveLocalY(0, 0.8f).setEaseOutExpo().delay = 0.3f;
    }

    public void Closed()
    {
        box4.LeanMoveLocalY(box4.localPosition.y * 3, 1f).setEaseOutQuart().delay = 0.2f;
        background.LeanAlpha(0, 0.3f);
        TitleWig.on = false;
        Invoke("windo", 0.9f);
        obj.material.SetFloat("_Size", 0f);

    }

    public void windo()
    {
        CanvasGroup CG = window.GetComponent<CanvasGroup>();
        window.SetActive(true);
        CG.alpha = 0f;
        CG.LeanAlpha(1f, 0.6f);
    }

    public void QQ()
    {
        Closed();
        box1.LeanMoveLocalY(Screen.height + 160, 0.5f).setEaseOutExpo();
        box2.LeanMoveLocalY(-Screen.height - 160, 0.5f).setEaseOutExpo();
        box3.LeanMoveLocalY(-Screen.height - 160, 0.5f).setEaseOutExpo();
    }
    public void WW()
    {
        Closed();
        box1.LeanMoveLocalY(-Screen.height - 160, 0.5f).setEaseOutExpo();
        box2.LeanMoveLocalY(Screen.height + 160, 0.5f).setEaseOutExpo();
        box3.LeanMoveLocalY(-Screen.height - 160, 0.5f).setEaseOutExpo();
    }
    public void EE()
    {
        Closed();
        box1.LeanMoveLocalY(-Screen.height - 160, 0.5f).setEaseOutExpo();
        box2.LeanMoveLocalY(-Screen.height - 160, 0.5f).setEaseOutExpo();
        box3.LeanMoveLocalY(Screen.height + 160, 0.5f).setEaseOutExpo();
    }

}
