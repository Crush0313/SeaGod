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
    //1 : 아군, -1 : 적군, 0: 기지
    public int isOwn;
    public static int[] sthp = {7};

    public int num;

    public int UpLv;

    int delHp;
    public int Hp;
    public int cHp;
    public int hp10;

    int delDmg;
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

    public bool isBig = false;

    WaitForSeconds delay1;

    public SpriteRenderer[] Parts; //피격 시 붉은 색을 칠할 자식 개체(이미지 부분)
    public GameObject RayPos; //raycast 쏘는 기준점이 되는 empty 게임오브젝트
    public GameObject Hpbar;
    public Color unHitColor; //255, 255, 255
    public Color HitRed; //255, 135, 135
    RaycastHit2D rayhit;
    Animator Anim;

   
     void Awake() //최초 생성, 두번째는 x
    {
        Debug.Log("내가 와따");
        delay1 = new WaitForSeconds(adelay1);
        Anim = GetComponent<Animator>();

        if (isOwn != 0)//기지가 아님
        {
            StartCoroutine(Move()); //disable 후 활성화 되었을 때에도 그런가
            if (isOwn == 1) //아군
            {
                //Hp = sthp[num];//아군만 스텟 가져옴******별도 스크립트
            }
            int R = Random.Range(0, 101);
            if (R <= 10) //사이즈업
            {
                transform.localScale = new Vector3(1.1f, 1.1f, 1);
                delHp = (int)(Hp * 0.3f);//체력 변화량
                Hp += delHp;
                delDmg = (int)(Dmg * 0.3f);//공격력 변화량
                Dmg += delDmg;
                isBig = true;
            }
        }
    }

    IEnumerator Attack(GameObject obj, WaitForSeconds aniDelay, int dmg)
    {
        yield return aniDelay; //애니메이션의 피격 순간과 실제 피격 시간을 맞춤
        obj.GetComponent<Unit>().damaged(dmg);
    }

    public void damaged(int dmg, bool isBleed = false)
    {
        if (cHp <= dmg) //남은 체력보다 대미지가 큼 = death
        {
            Invoke("DestroyUnit", 5f);
        }
        else
        {
            cHp -= dmg;
            if (cHp <= hp10)
            {//체력 10% 이하 : 빈사상태
                Knockback(1f);
                Debug.Log("빈사");
            }
            else //상시 넉백
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
        if (isOwn == 0) //풀로 안 만든 기지 예외처리
            Destroy(this);
        else
        {
            if (isBig) //원상복구
            {
                transform.localScale = new Vector3(1, 1, 1);
                Hp += delHp;
                Dmg += delDmg;
                isBig = false;
            }
            summon.instance.ReturnPool(this); //반환
        }
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
            Parts[i].color = unHitColor;
        }
    }
    IEnumerator Move() //서치 앤드 디스트로이...
    {
        while (true)
        {
            //yield return null;
            yield return null;
            Debug.DrawRay(RayPos.transform.position, new Vector3(isOwn, 0, 0), new Color(0, 1, 0)); //레이 시각화(에디터에서만)

            if (isOwn == 1) //아군 : 적 레이어만 감지 ->
            {
                rayhit = Physics2D.Raycast(RayPos.transform.position, Vector2.right, range, LayerMask.GetMask("enermy"));
            }

            else if (isOwn == -1) //적군 : 아군 레이어만 감지 <-
            {
                rayhit = Physics2D.Raycast(RayPos.transform.position, Vector2.left, range, LayerMask.GetMask("team"));
            }

            if (rayhit.collider != null) //감지 했음
            {
                //Debug.Log("null 아님");
                Anim.SetBool("isMove", false); //비이동 상태
                if (cDelay >= Delay)//쿨타임 참
                {
                    Anim.SetTrigger("Atk");
                    cDelay = 0;
                    StartCoroutine(Attack(rayhit.collider.gameObject, delay1, Dmg));//매개변수 땜시 invoke 대신 코루틴
                }
                else //쿨타임 아직이면
                {
                    cDelay += Time.deltaTime; //시간조각 더하기
                }
            }
            else // 감지 못함 : 아무도 앞에 없음 = 이동
            {
                Anim.SetBool("isMove", true); //이동 상태
                transform.Translate(isOwn * speed * Time.deltaTime, 0, 0); //방향 * 속도 * 렉 처리
            }
        }
    }

    void IPoolObject.OnCreatedInPool() //두번째는 x
    {
        Debug.Log("생성");
    }

    void IPoolObject.OnGettingFromPool()
    {
        cHp = Hp;
        hp10 = Hp / 10;
        cDelay = Delay;//첫 타는 딜레이x
        ChangedHp();
        Debug.Log("히히");
    }
}
