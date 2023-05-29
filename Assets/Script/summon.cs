using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public class summon : MonoBehaviour
{
    public GameObject _UnitPrefab;
    public IObjectPool<Unit> _Pool;

    //��ȯ��ư
    public GameObject spawnPoint;
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

    private void Awake()
    {
        _Pool = new ObjectPool<Unit>(CreateUnit, onGetUnit, OnReleaseUnit, OnDestroyUnit, maxSize:20);
    }

    void Start()
    {
        manaCycle = new WaitForSeconds(ManaCool); //�Ź� new�� ����� �ͺ���, �ѹ� ����� ���� �ڿ��Ҹ� ����
        for (int i = 0; i < HideSkillButtons.Length; i++)
        {
            HideSkillButtons[i].raycastTarget = false; //��� ��ư�� ���� ��Ȱ�� ���·� ����
            isHide[i] = true;
        }
        StartCoroutine(ManaGen()); //���� ȸ�� ����
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

    public void Summon1(int needMana)
    {
        if (cMana >= needMana)
        {
            cMana -= needMana;
            cSkillTime[0] = SkillTime[0];
            var unit = _Pool.Get();

            unit.transform.position = spawnPoint.transform.position;
            isHide[0] = true; //Ȱ��ȭ ���·�
            HideSkillButtons[0].raycastTarget = false; //��ư�� Ȱ��ȭ ���·�
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
    private void onGetUnit(Unit unit) //Ȱ��ȭ
    {
        unit.gameObject.SetActive(true);
    }
    private void OnReleaseUnit(Unit unit) //��Ȱ��ȭ
    {
        unit.gameObject.SetActive(false);
    }
    private void OnDestroyUnit(Unit unit) //����
    {
        Destroy(unit.gameObject);
    }
        
}

