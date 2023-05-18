using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int Hp;
    public int Dmg;
    public int Def;
    public float delay;
    public float range;
    public float speed;
    public float heal;
    public int dropMoney;

    public GameObject RayPos; //raycast 쏘는 기준점이 되는 empty 게임오브젝트
    RaycastHit2D rayhit;
    Animator Anim;

    //1 : 아군, -1 : 적군
    public int isOwn;
   

    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    void Update()
    {

        Debug.DrawRay(RayPos.transform.position, new Vector3(isOwn, 0, 0), new Color(0, 1, 0)); //레이 시각화(에디터에서만)

        if (isOwn == 1) //아군 : 적 레이어만 감지 ->
        {
            rayhit = Physics2D.Raycast(RayPos.transform.position, Vector2.right, range, LayerMask.GetMask("enermy"));
        }

        else if(isOwn == -1) //적군 : 아군 레이어만 감지 <-
        {
            rayhit = Physics2D.Raycast(RayPos.transform.position, Vector2.left, range, LayerMask.GetMask("team"));
        }

        if (rayhit.collider !=null) //감지 했음
        {
            Debug.Log("null 아님");
            Anim.SetBool("isMove", false); //비이동 상태
        }
        else // 감지 못함 : 아무도 앞에 없음 = 이동
        {
            Debug.Log("null 임");
            transform.Translate(isOwn * speed * Time.deltaTime, 0, 0); //방향 * 속도 * 렉 처리
            Anim.SetBool("isMove", true); //이동 상태
        }
    }

}
