using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class Unit : MonoBehaviour 
{
    public IObjectPool<Unit> _ManageredPool;

    //1 : 아군, -1 : 적군
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

    public SpriteRenderer[] Parts; //피격 시 붉은 색을 칠할 자식 개체(이미지 부분)
    public GameObject RayPos; //raycast 쏘는 기준점이 되는 empty 게임오브젝트
    public GameObject Hpbar;
    public Color unHitColor; //255, 255, 255
    public Color HitRed; //255, 135, 135
    RaycastHit2D rayhit;
    Animator Anim;

   
     void Awake()
    {
        delay1 = new WaitForSeconds(adelay1);
        //Hp = sthp[num];//아군만 스텟 가져옴******별도 스크립트
        cHp = Hp;
        hp10 = Hp / 10;
        cDelay = Delay;//첫 타는 딜레이x
        Anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Debug.Log("이너블"); //시작시에도 반응
        ChangedHp();
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
    public void SetManageredPool(IObjectPool<Unit> pool) //
    {
        _ManageredPool = pool;
    }
    public void DestroyUnit() //사망 = 해제
    {
        Debug.Log("쥬금");
        _ManageredPool.Release(this); //근데 오류남
    }
    public void Knockback(float knock)
    {
        if (knock >= knockDef)
        {
            transform.Translate(-1f * isOwn * knock, 0f, 0f);
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
            Parts[i].color = Color.white;
        }
    }
}
