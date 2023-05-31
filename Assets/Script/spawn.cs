using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public class spawn : MonoBehaviour
{
    public GameObject _UnitPrefab;
    public IObjectPool<Unit> _Pool;

    //��ȯ��ư
    public GameObject spawnPoint;

    public int eMana; //�ִ� ����
    public int ecMana;
    public float eManaCool; //���� ȸ�� �ֱ�
    WaitForSeconds emanaCycle; //���� ȸ�� �ֱ�

    public int patternNum;
    public int[] enemyCost = { 5, 7, 9 };

    private void Awake()
    {
        _Pool = new ObjectPool<Unit>(CreateUnit, onGetUnit, OnReleaseUnit, OnDestroyUnit, maxSize:20);
    }

    void Start()
    {
        emanaCycle = new WaitForSeconds(eManaCool);
        StartCoroutine(eManaGen()); //���� ȸ�� ����
    }
 
    IEnumerator eManaGen() //�� ��ȯ�� ȸ��
    {
        while (true)
        {
            yield return emanaCycle; //���� ����Ŭ���� ȸ��
            if (ecMana < eMana) //�� �� ������
            {
                ecMana += 1; //1�� ȸ��
            }
            chkSpawn();
        }
    }
    //void spawnPattern()
    //{
    //    patternNum = Random.Range(0, 3);
    //}
    void chkSpawn()
    {
        if (ecMana > enemyCost[1])
        {
            ecMana -= enemyCost[1];
            var unit = _Pool.Get();
            unit.transform.position = spawnPoint.transform.position;
            //spawnPattern(); //���� ����
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

