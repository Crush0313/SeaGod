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

    public GameObject RayPos; //raycast ��� �������� �Ǵ� empty ���ӿ�����Ʈ
    RaycastHit2D rayhit;
    Animator Anim;

    //1 : �Ʊ�, -1 : ����
    public int isOwn;
   

    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    void Update()
    {

        Debug.DrawRay(RayPos.transform.position, new Vector3(isOwn, 0, 0), new Color(0, 1, 0)); //���� �ð�ȭ(�����Ϳ�����)

        if (isOwn == 1) //�Ʊ� : �� ���̾ ���� ->
        {
            rayhit = Physics2D.Raycast(RayPos.transform.position, Vector2.right, range, LayerMask.GetMask("enermy"));
        }

        else if(isOwn == -1) //���� : �Ʊ� ���̾ ���� <-
        {
            rayhit = Physics2D.Raycast(RayPos.transform.position, Vector2.left, range, LayerMask.GetMask("team"));
        }

        if (rayhit.collider !=null) //���� ����
        {
            Debug.Log("null �ƴ�");
            Anim.SetBool("isMove", false); //���̵� ����
        }
        else // ���� ���� : �ƹ��� �տ� ���� = �̵�
        {
            Debug.Log("null ��");
            transform.Translate(isOwn * speed * Time.deltaTime, 0, 0); //���� * �ӵ� * �� ó��
            Anim.SetBool("isMove", true); //�̵� ����
        }
    }

}
