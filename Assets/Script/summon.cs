using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Redcode.Pools;

public class summon : MonoBehaviour
{
    public static summon instance;
    PoolManager poolManager;

    //��ȯ��ư
    public GameObject[] spawnPoint;
    public Image[] HideSkillButtons; //���� ��ȯ ��ư
    public Button[] button;
    public Image[] hideSkill; //��ȯ ��ư ������
    public bool[] isHide = { false };
    public float[] SkillTime = { 4 };
    public float[] cSkillTime = { 0 };

    //��ȯ�� = ����
    public Image Manabar;
    public Text ManaText;
    public int Mana; //�ִ� ����
    public int cMana=0;
    public float ManaCool; //���� ȸ�� �ֱ�
    public float cManaCool = 0; //���� ȸ�� �ֱ�
    public int[] needMana;
    //public int eMana; //�ִ� ����
    //public int ecMana;
    //public float eManaCool; //���� ȸ�� �ֱ�
    //WaitForSeconds emanaCycle; //���� ȸ�� �ֱ�
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
        cCreatTime = Random.Range(creatTime[0], creatTime[1]);//���� �� ���� ������
        for (int i = 0; i < HideSkillButtons.Length; i++)
        {
            HideSkillButtons[i].raycastTarget = false; //��� ��ư�� ���� ��Ȱ�� ���·� ����
            isHide[i] = true;
        }
    }
    void Update()
    {
        HideSkillChk();

        summonEnemy();//�ִ� �ð������� �Լ� �ȿ� ����

        cManaCool += Time.deltaTime;//������ �ð������� �Լ� ������ ����.������ �տ��� - �񱳿��������� else�� �Ʒ����� �Ʒ��� ������ �񱳿��� - �տ���. ����� �������� ��. �׳� ������ �����ΰ�
        if (cManaCool >= ManaCool) //0���� �����ؼ� �𺸴� Ŀ���� �ߵ�. ���� : ���� ������ ȣ��(������ ���� ����). �������Ǳ��� ������ ������ ��ĥ ������ �������� �ʰ�����, �� �� �Ϲ����� ��Ȳ������ �������Ǳ��� �˻��ϱ� ������ ��ȿ������ ����. ������ ��ĥ���� �տ��� �ϵ縻�� �������δ� ���� ���� ����
        {
            cManaCool = 0; //�� �ʱ�ȭ (�ٽ� 0����)
            ManaGen(); //�Լ� ȣ��
        }
    }

    void HideSkillChk()
    {
        if (isHide[0]) //��Ȱ�� ����
        {
            StartCoroutine(SkillTimeChk(0));
        }
    } 

    IEnumerator SkillTimeChk(int skillNum) //��ư ������
    {//?�Լ��� �ٲ㼭 ������Ʈ���� �ִ� �� ������ �ڷ�ƾ���� �δ°� ������ �𸣰���...
        yield return null; //1������
        if (cSkillTime[skillNum] >= 0) //��Ÿ���� 0���� ŭ, ��Ÿ�� ��
        {
            cSkillTime[skillNum] -= Time.deltaTime; //deltatime��ŭ ��
            if (cSkillTime[skillNum] < 0) //0���� �۾���, ��Ÿ�� ��
            {
                cSkillTime[skillNum] = 0; //0���� ����

                isHide[skillNum] = false; //Ȱ��ȭ ���·�
                HideSkillButtons[skillNum].raycastTarget = true; //��ư�� Ȱ��ȭ ���·�, ��� �ٸ� ����ĳ��Ʈ�� �޴� ui�� ���� ������ �긦 ���� �������� �׷��� �� ���� ����
            }
            float time = cSkillTime[skillNum] / SkillTime[skillNum]; // (���� ��/�ִ� ��) ��
            hideSkill[skillNum].fillAmount = time; //��ư fill amount�� ����
        }
        //?���Ḧ �� ���ѵ� �Ǵ� �ǰ�
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
    void ManaGen() //��ȯ�� ȸ��
    {
        if (cMana < Mana) //�� �� ������
        {
            cMana += 1; //1�� ȸ��
            Manabar.fillAmount = (float)cMana / (float)Mana; //���� ui�� ����
            ManaText.text = $"{cMana} / {Mana}"; //���� �ؽ�Ʈ�� ����
            ChkButton();
        }
    }
    public void summonEnemy()
    {
        cCreatTime -= Time.deltaTime; 
        if (cCreatTime <= 0) //���� �����ؼ� 0�� �Ǹ� ��Ÿ�� ��
        {
            int TheR = Random.Range(0, 101); //��ȯ ���� ���� ��������

            if (TheR >= 90)
                Summon(1, 1);//��4 10%
            else if (TheR >= 80)
                Summon(1, 1);//��3 20%
            else if (TheR >= 70)
                Summon(1, 1);//��2 30%
            else
                Summon(1, 1);//��1 (�⺻) ������ 70%

            cCreatTime = Random.Range(creatTime[0], creatTime[1]); //���� ���� ������
        }
    }
    public void SummonUnit(int Num)
    {
        if (cMana >= needMana[Num])
        {
            cMana -= needMana[Num];
            cSkillTime[Num] = SkillTime[Num];

            Summon(Num, 0);

            isHide[Num] = true; //���� ���·�
            HideSkillButtons[Num].raycastTarget = false; //��ư�� ��Ȱ��ȭ ���·�
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
        Debug.Log("���");
        poolManager.TakeToPool<Unit>(clone.idName, clone);//ȸ��
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

}

