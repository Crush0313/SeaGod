using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Unit : MonoBehaviour 
{
    private IObjectPool<Unit> _ManageredPool; 

    public static int[] sthp = {7};

    public int num;

    public int UpLv;
    public int Hp;
    public int cHp;
    public int Dmg;
    public int Def;
    public float Delay;
    public float cDelay;
    public float adelay1;
    public float range;
    public float speed;
    public float heal;
    public int dropMoney;
    WaitForSeconds delay1;

    public GameObject RayPos; //raycast ��� �������� �Ǵ� empty ���ӿ�����Ʈ
    RaycastHit2D rayhit;
    Animator Anim;

    //1 : �Ʊ�, -1 : ����
    public int isOwn;
   
     void Awake()
    {
        delay1 = new WaitForSeconds(adelay1);
        //Hp = sthp[num];//�Ʊ��� ���� ������******���� ��ũ��Ʈ
        cHp = Hp;
        cDelay = Delay;//ù Ÿ�� ������x
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
            //Debug.Log("null �ƴ�");
            Anim.SetBool("isMove", false); //���̵� ����
            if (cDelay >= Delay)//��Ÿ�� ��
            {
                Anim.SetTrigger("Atk");
                cDelay = 0;
                StartCoroutine(Attack(rayhit.collider.gameObject, adelay1, Dmg));//�Ű����� ���� invoke ��� �ڷ�ƾ
            }
            else //��Ÿ�� �����̸�
            {
                cDelay += Time.deltaTime; //�ð����� ���ϱ�
            }
        }
        else // ���� ���� : �ƹ��� �տ� ���� = �̵�
        {
            //Debug.Log("null ��");
            transform.Translate(isOwn * speed * Time.deltaTime, 0, 0); //���� * �ӵ� * �� ó��
            Anim.SetBool("isMove", true); //�̵� ����
        }
    }

    IEnumerator Attack(GameObject obj, float aniDelay, int dmg)
    {
        yield return aniDelay; //�ִϸ��̼��� �ǰ� ������ ���� �ǰ� �ð��� ����
        obj.GetComponent<Unit>().damaged(dmg);
    }

    public void damaged(int dmg, bool isBleed = false)
    {
        if (cHp <= dmg) //���� ü�º��� ������� ŭ = death
        {
            DestroyUnit();
        }
        else
        {
            cHp -= dmg;
        }
    }
    public void SetManageredPool(IObjectPool<Unit> pool) //
    {
        _ManageredPool = pool;
    }
    public void DestroyUnit() //��� = ����
    {
        Debug.Log("���");
        _ManageredPool.Release(this); //�ٵ� ������
    }

}
