using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
using Redcode.Pools;

public class Unit : MonoBehaviour, IPoolObject
{
    public string idName;

    //1 : �Ʊ�, -1 : ����, 0: ����
    public int isOwn;
    public static int[] sthp = {7};

    public int num;

    public int UpLv;

    public int Hp;
    public int cHp;
    public int hp10;

    public int Dmg;
    public int Def;
    public float knockDef;
    public float range;
    public float speed;
    public float heal;

    public int dropMoney;

    public float Delay;
    public float cDelay;

    public float adelay1;

    WaitForSeconds delay1;

    public SpriteRenderer[] Parts; //�ǰ� �� ���� ���� ĥ�� �ڽ� ��ü(�̹��� �κ�)
    public GameObject RayPos; //raycast ��� �������� �Ǵ� empty ���ӿ�����Ʈ
    public GameObject Hpbar;
    public Color unHitColor; //255, 255, 255
    public Color HitRed; //255, 135, 135
    RaycastHit2D rayhit;
    Animator Anim;

   
     void Awake() //���� ����
    {
        delay1 = new WaitForSeconds(adelay1);
        Anim = GetComponent<Animator>();

        if (isOwn == 1) //�Ʊ�
        {
            //Hp = sthp[num];//�Ʊ��� ���� ������******���� ��ũ��Ʈ
        }
        if (isOwn != 0)//������ �ƴ�
        {
            StartCoroutine(Move()); //disable �� Ȱ��ȭ �Ǿ��� ������ �׷���
        }
    }

    IEnumerator Attack(GameObject obj, WaitForSeconds aniDelay, int dmg)
    {
        yield return aniDelay; //�ִϸ��̼��� �ǰ� ������ ���� �ǰ� �ð��� ����
        obj.GetComponent<Unit>().damaged(dmg);
    }

    public void damaged(int dmg, bool isBleed = false)
    {
        if (cHp <= dmg) //���� ü�º��� ������� ŭ = death
        {
            Invoke("DestroyUnit", 5f);
        }
        else
        {
            cHp -= dmg;
            if (cHp <= hp10)
            {//ü�� 10% ���� : ������
                Knockback(1f);
                Debug.Log("���");
            }
            else //��� �˹�
                Knockback(0.2f);

            ChangedHp();
            HitEffect();
        }
    }
    public void Knockback(float knock)
    {
        if (knock >= knockDef)
        {
            transform.Translate(-1f * isOwn * knock, 0f, 0f);
        }
    }

    public void DestroyUnit()
    {
        if (isOwn == 0)
            Destroy(this);
        else
            summon.instance.ReturnPool(this);
    }
    public void ChangedHp()
    {
        Hpbar.transform.LeanScaleX((float)cHp / (float)Hp, 0.1f);

    }
    public void HitEffect()
    {
        for (int i = 0; i < Parts.Length; i++)
        {
            Parts[i].color = HitRed;
        }
        Invoke("ReColor", 0.2f);
    }
    public void ReColor()
    {
        for (int i = 0; i < Parts.Length; i++)
        {
            Parts[i].color = Color.white;
        }
    }
    IEnumerator Move() //��ġ �ص� ��Ʈ����...
    {
        while (true)
        {
            //yield return null;
            yield return null;
            Debug.DrawRay(RayPos.transform.position, new Vector3(isOwn, 0, 0), new Color(0, 1, 0)); //���� �ð�ȭ(�����Ϳ�����)

            if (isOwn == 1) //�Ʊ� : �� ���̾ ���� ->
            {
                rayhit = Physics2D.Raycast(RayPos.transform.position, Vector2.right, range, LayerMask.GetMask("enermy"));
            }

            else if (isOwn == -1) //���� : �Ʊ� ���̾ ���� <-
            {
                rayhit = Physics2D.Raycast(RayPos.transform.position, Vector2.left, range, LayerMask.GetMask("team"));
            }

            if (rayhit.collider != null) //���� ����
            {
                //Debug.Log("null �ƴ�");
                Anim.SetBool("isMove", false); //���̵� ����
                if (cDelay >= Delay)//��Ÿ�� ��
                {
                    Anim.SetTrigger("Atk");
                    cDelay = 0;
                    StartCoroutine(Attack(rayhit.collider.gameObject, delay1, Dmg));//�Ű����� ���� invoke ��� �ڷ�ƾ
                }
                else //��Ÿ�� �����̸�
                {
                    cDelay += Time.deltaTime; //�ð����� ���ϱ�
                }
            }
            else // ���� ���� : �ƹ��� �տ� ���� = �̵�
            {
                Anim.SetBool("isMove", true); //�̵� ����
                transform.Translate(isOwn * speed * Time.deltaTime, 0, 0); //���� * �ӵ� * �� ó��
            }
        }
    }

    void IPoolObject.OnCreatedInPool()
    {
        Debug.Log("����");
    }

    void IPoolObject.OnGettingFromPool()
    {
        cHp = Hp;
        hp10 = Hp / 10;
        cDelay = Delay;//ù Ÿ�� ������x
        ChangedHp();
        Debug.Log("����");
    }
}
