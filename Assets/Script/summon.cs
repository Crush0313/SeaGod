using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public class summon : MonoBehaviour
{
    public GameObject _UnitPrefab;
    public IObjectPool<Unit> _Pool;

    //소환버튼
    public GameObject[] spawnPoint;
    public Image[] HideSkillButtons; //숨길 소환 버튼
    public Image[] hideSkill; //소환 버튼 가림막
    public bool[] isHide = { false };
    public float[] SkillTime = { 4 };
    public float[] cSkillTime = { 0 };

    //소환력 = 마나
    public Image Manabar;
    public Text ManaText;
    public int Mana; //최대 마나
    public int cMana;
    public float ManaCool; //마나 회복 주기
    WaitForSeconds manaCycle; //마나 회복 주기

    //public int eMana; //최대 마나
    //public int ecMana;
    //public float eManaCool; //마나 회복 주기
    //WaitForSeconds emanaCycle; //마나 회복 주기

    public int patternNum;
    public int[] enemyCost = { 5, 7, 9 };

    private void Awake()
    {
        _Pool = new ObjectPool<Unit>(CreateUnit, onGetUnit, OnReleaseUnit, OnDestroyUnit, maxSize:20);
    }

    void Start()
    {
        manaCycle = new WaitForSeconds(ManaCool); //매번 new로 만드는 것보다, 한번 만드는 것이 자원소모가 
        //emanaCycle = new WaitForSeconds(eManaCool);
        for (int i = 0; i < HideSkillButtons.Length; i++)
        {
            HideSkillButtons[i].raycastTarget = false; //모든 버튼에 대해 비활성 상태로 만듦
            isHide[i] = true;
        }
        StartCoroutine(ManaGen()); //마나 회복 시작
        //StartCoroutine(eManaGen()); //마나 회복 시작
    }
    void Update()
    {
        HideSkillChk();
    }

    void HideSkillChk()
    {
        if (isHide[0]) //비활성 상태
        {
            StartCoroutine(SkillTimeChk(0));
        }
    } 

    IEnumerator SkillTimeChk(int skillNum)
    {
        yield return null;
        if (cSkillTime[skillNum] >= 0) //쿨타임이 0보다 큼, 쿨타임 중
        {
            cSkillTime[skillNum] -= Time.deltaTime; //deltatime만큼 뺌
            if (cSkillTime[skillNum] < 0) //0보다 작아짐, 쿨타임 끝
            {
                cSkillTime[skillNum] = 0; //0으로 조정

                isHide[skillNum] = false; //활성화 상태로
                HideSkillButtons[skillNum].raycastTarget = true; //버튼을 활성화 상태로
            }
            float time = cSkillTime[skillNum] / SkillTime[skillNum]; // 현재 쿨/최대 쿨 을
            hideSkill[skillNum].fillAmount = time; //버튼 fill amount에 적용
        }
    }
    IEnumerator ManaGen() //소환력 회복
    {
        while (true)
        {
            yield return manaCycle; //마나 사이클마다 회복
            if (cMana < Mana) //꽉 찰 때까지
            {
                cMana += 1; //1씩 회복
                Manabar.fillAmount = (float)cMana / (float)Mana;
                ManaText.text = $"{cMana} / {Mana}";
            }
        }
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
    public void Summon1(int needMana)
    {
        if (cMana >= needMana)
        {
            cMana -= needMana;
            cSkillTime[0] = SkillTime[0];
            var unit = _Pool.Get();

            unit.transform.position = spawnPoint[0].transform.position;
            isHide[0] = true; //활성화 상태로
            HideSkillButtons[0].raycastTarget = false; //버튼을 활성화 상태로
            Manabar.fillAmount = (float)cMana / (float)Mana;
            ManaText.text = $"{cMana} / {Mana}";
        }
    }
    private Unit CreateUnit()
    {
        Unit unit = Instantiate(_UnitPrefab).GetComponent<Unit>();
        unit.SetManageredPool(_Pool);
        return unit;
    }
    private void onGetUnit(Unit unit) //활성화
    {
        unit.gameObject.SetActive(true);
    }
    private void OnReleaseUnit(Unit unit) //비활성화
    {
        unit.gameObject.SetActive(false);
    }
    private void OnDestroyUnit(Unit unit) //삭제
    {
        Destroy(unit.gameObject);
    }
        
}

