using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using Redcode.Pools;

public class summon : MonoBehaviour
{
    public static summon instance;
    PoolManager poolManager;

    //��ȯ��ư
    public GameObject[] spawnPoint;
    public Image[] HideSkillButtons; //���� ��ȯ ��ư
    public Image[] hideSkill; //��ȯ ��ư ������
    public bool[] isHide = { false };
    public float[] SkillTime = { 4 };
    public float[] cSkillTime = { 0 };

    //��ȯ�� = ����
    public Image Manabar;
    public Text ManaText;
    public int Mana; //�ִ� ����
    public int cMana;
    public float ManaCool; //���� ȸ�� �ֱ�
    WaitForSeconds manaCycle; //���� ȸ�� �ֱ�

    //public int eMana; //�ִ� ����
    //public int ecMana;
    //public float eManaCool; //���� ȸ�� �ֱ�
    //WaitForSeconds emanaCycle; //���� ȸ�� �ֱ�

    public int patternNum;
    public int[] enemyCost = { 5, 7, 9 };

    private void Awake()
    {
        poolManager = GetComponent<PoolManager>();
    }

    void Start()
    {
        manaCycle = new WaitForSeconds(ManaCool); //�Ź� new�� ����� �ͺ���, �ѹ� ����� ���� �ڿ��Ҹ� 
        //emanaCycle = new WaitForSeconds(eManaCool);
        for (int i = 0; i < HideSkillButtons.Length; i++)
        {
            HideSkillButtons[i].raycastTarget = false; //��� ��ư�� ���� ��Ȱ�� ���·� ����
            isHide[i] = true;
        }
        StartCoroutine(ManaGen()); //���� ȸ�� ����
        //StartCoroutine(eManaGen()); //���� ȸ�� ����
    }
    void Update()
    {
        HideSkillChk();
    }

    void HideSkillChk()
    {
        if (isHide[0]) //��Ȱ�� ����
        {
            StartCoroutine(SkillTimeChk(0));
        }
    } 

    IEnumerator SkillTimeChk(int skillNum)
    {
        yield return null;
        if (cSkillTime[skillNum] >= 0) //��Ÿ���� 0���� ŭ, ��Ÿ�� ��
        {
            cSkillTime[skillNum] -= Time.deltaTime; //deltatime��ŭ ��
            if (cSkillTime[skillNum] < 0) //0���� �۾���, ��Ÿ�� ��
            {
                cSkillTime[skillNum] = 0; //0���� ����

                isHide[skillNum] = false; //Ȱ��ȭ ���·�
                HideSkillButtons[skillNum].raycastTarget = true; //��ư�� Ȱ��ȭ ���·�
            }
            float time = cSkillTime[skillNum] / SkillTime[skillNum]; // ���� ��/�ִ� �� ��
            hideSkill[skillNum].fillAmount = time; //��ư fill amount�� ����
        }
    }
    IEnumerator ManaGen() //��ȯ�� ȸ��
    {
        while (true)
        {
            yield return manaCycle; //���� ����Ŭ���� ȸ��
            if (cMana < Mana) //�� �� ������
            {
                cMana += 1; //1�� ȸ��
                Manabar.fillAmount = (float)cMana / (float)Mana;
                ManaText.text = $"{cMana} / {Mana}";
            }
        }
    }
    //IEnumerator eManaGen() //�� ��ȯ�� ȸ��
    //{
    //    while (true)
    //    {
    //        yield return emanaCycle; //���� ����Ŭ���� ȸ��
    //        if (ecMana < eMana) //�� �� ������
    //        {
    //            ecMana += 1; //1�� ȸ��
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
    //        spawnPattern(); //���� ����
    //    }
    //}
    public void Summon1(int needMana)
    {
        if (cMana >= needMana)
        {
            cMana -= needMana;
            cSkillTime[0] = SkillTime[0];

            //var unit = _Pool.Get();
            //
            //isHide[0] = true; //Ȱ��ȭ ���·�

            Unit unit = poolManager.GetFromPool<Unit>(0);
            unit.transform.position = spawnPoint[0].transform.position;

            HideSkillButtons[0].raycastTarget = false; //��ư�� Ȱ��ȭ ���·�
            Manabar.fillAmount = (float)cMana / (float)Mana;
            ManaText.text = $"{cMana} / {Mana}";
        }
    }
    public void ReturnPool(Unit clone)
    {
        poolManager.TakeToPool<Unit>(clone.idName, clone);//ȸ��
    }


}

