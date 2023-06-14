using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Redcode.Pools;

public class summon : MonoBehaviour
{
    public static summon instance;
    PoolManager poolManager;

    //소환버튼
    public GameObject[] spawnPoint;
    public Image[] HideSkillButtons; //숨길 소환 버튼
    public Button[] button;
    public Image[] hideSkill; //소환 버튼 가림막
    public bool[] isHide = { false };
    public float[] SkillTime = { 4 };
    public float[] cSkillTime = { 0 };

    //소환력 = 마나
    public Image Manabar;
    public Text ManaText;
    public int Mana; //최대 마나
    public int cMana=0;
    public float ManaCool; //마나 회복 주기
    public float cManaCool = 0; //마나 회복 주기
    public int[] needMana;
    //public int eMana; //최대 마나
    //public int ecMana;
    //public float eManaCool; //마나 회복 주기
    //WaitForSeconds emanaCycle; //마나 회복 주기
    public int[] creatTime; // min, max
    public int SizeUpChance = 10;
    public int[] enemyChance;
    public int[] enemyNum;
    public float cCreatTime;
    public int patternNum;
    public int[] enemyCost = { 5, 7, 9 };

    private void Awake()
    {
        poolManager = GetComponent<PoolManager>();
        instance = this;
    }

    void Start()
    {
        cCreatTime = Random.Range(creatTime[0], creatTime[1]);//최초 적 생성 딜레이
        for (int i = 0; i < HideSkillButtons.Length; i++)
        {
            HideSkillButtons[i].raycastTarget = false; //모든 버튼에 대해 비활성 상태로 만듦
            isHide[i] = true;
        }
    }
    void Update()
    {
        HideSkillChk();

        summonEnemy();//애는 시간조건을 함수 안에 넣음

        cManaCool += Time.deltaTime;//이쪽은 시간적용을 함수 밖으로 빼냄.지금은 합연산 - 비교연산이지만 else로 아래조건 아래로 빼내면 비교연산 - 합연산. 어느게 좋은지는 모름. 그냥 가독성 차이인가
        if (cManaCool >= ManaCool) //0부터 시작해서 쿨보다 커지면 발동. 단점 : 마나 꽉차도 호출(마나가 늘진 않음). 마나조건까지 넣으면 마나가 넘칠 때에는 움직이지 않겠지만, 그 외 일반적인 상황에서도 마나조건까지 검사하기 때문에 비효율적일 수도. 마나가 넘칠때도 합연산 하든말든 내버려두는 것이 답일 수도
        {
            cManaCool = 0; //쿨 초기화 (다시 0으로)
            ManaGen(); //함수 호출
        }
    }

    void HideSkillChk()
    {
        if (isHide[0]) //비활성 상태
        {
            StartCoroutine(SkillTimeChk(0));
        }
    } 

    IEnumerator SkillTimeChk(int skillNum) //버튼 가리기
    {//?함수로 바꿔서 업데이트문에 넣는 게 나을지 코루틴으로 두는게 나을지 모르겠음...
        yield return null; //1프레임
        if (cSkillTime[skillNum] >= 0) //쿨타임이 0보다 큼, 쿨타임 중
        {
            cSkillTime[skillNum] -= Time.deltaTime; //deltatime만큼 뺌
            if (cSkillTime[skillNum] < 0) //0보다 작아짐, 쿨타임 끝
            {
                cSkillTime[skillNum] = 0; //0으로 조정

                isHide[skillNum] = false; //활성화 상태로
                HideSkillButtons[skillNum].raycastTarget = true; //버튼을 활성화 상태로, 사실 다른 레이캐스트를 받는 ui가 위에 있으면 얘를 꺼도 눌리지만 그렇게 안 만들 거임
            }
            float time = cSkillTime[skillNum] / SkillTime[skillNum]; // (현재 쿨/최대 쿨) 을
            hideSkill[skillNum].fillAmount = time; //버튼 fill amount에 적용
        }
        //?종료를 안 시켜도 되는 건가
    }

    void ChkButton()
    {
        for(int i = 0; i < 2/*9*/; i++) //0~9
        {
            if (needMana[i] <= cMana)
                button[i].interactable = true;
            else
                button[i].interactable = false;

        }
    }
    void ManaGen() //소환력 회복
    {
        if (cMana < Mana) //꽉 찰 때까지
        {
            cMana += 1; //1씩 회복
            Manabar.fillAmount = (float)cMana / (float)Mana; //비율 ui바 적용
            ManaText.text = $"{cMana} / {Mana}"; //비율 텍스트에 적용
            ChkButton();
        }
    }
    public void summonEnemy()
    {
        cCreatTime -= Time.deltaTime; 
        if (cCreatTime <= 0) //점점 감소해서 0이 되면 쿨타임 끝
        {
            int TheR = Random.Range(0, 101); //소환 몹을 정할 랜덤변수

            if (TheR >= 90)
                Summon(1, 1);//몹4 10%
            else if (TheR >= 80)
                Summon(1, 1);//몹3 20%
            else if (TheR >= 70)
                Summon(1, 1);//몹2 30%
            else
                Summon(1, 1);//몹1 (기본) 나머지 70%

            cCreatTime = Random.Range(creatTime[0], creatTime[1]); //다음 생성 딜레이
        }
    }
    public void SummonUnit(int Num)
    {
        if (cMana >= needMana[Num])
        {
            cMana -= needMana[Num];
            cSkillTime[Num] = SkillTime[Num];

            Summon(Num, 0);

            isHide[Num] = true; //숨김 상태로
            HideSkillButtons[Num].raycastTarget = false; //버튼을 비활성화 상태로
            Manabar.fillAmount = (float)cMana / (float)Mana;
            ManaText.text = $"{cMana} / {Mana}";
            ChkButton();
        }
    }

    public void Summon(int TargetNum, int SpawnPoint)
    {
        Unit unit = poolManager.GetFromPool<Unit>(TargetNum);
        unit.transform.position = spawnPoint[SpawnPoint].transform.position;
    }

    public void ReturnPool(Unit clone)
    {
        Debug.Log(clone.idName);
        Debug.Log("듀금");
        poolManager.TakeToPool<Unit>(clone.idName, clone);//회수
    }

    //IEnumerator eManaGen() //적 소환력 회복
    //{
    //    while (true)
    //    {
    //        yield return emanaCycle; //마나 사이클마다 회복
    //        if (ecMana < eMana) //꽉 찰 때까지
    //        {
    //            ecMana += 1; //1씩 회복
    //        }
    //        chkSpawn();
    //    }
    ////}
    //void spawnPattern()
    //{
    //    patternNum = Random.Range(0, 3);
    //}
    //void chkSpawn()
    //{
    //    if (ecMana > enemyCost[patternNum])
    //    {
    //        ecMana -= enemyCost[patternNum];
    //        var unit = _Pool.Get();
    //        unit.transform.position = spawnPoint[1].transform.position;
    //        spawnPattern(); //패턴 변경
    //    }
    //}

}

