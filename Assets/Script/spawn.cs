using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using Redcode.Pools;

public class spawn : MonoBehaviour
{/*
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

            Unit unit = poolManager.GetFromPool<Unit>(0);
            unit.transform.position = spawnPoint[0].transform.position;
            //spawnPattern(); //���� ����
        }
    }
    */

        
}

