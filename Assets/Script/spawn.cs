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

    //소환버튼
    public GameObject spawnPoint;

    public int eMana; //최대 마나
    public int ecMana;
    public float eManaCool; //마나 회복 주기
    WaitForSeconds emanaCycle; //마나 회복 주기

    public int patternNum;
    public int[] enemyCost = { 5, 7, 9 };

    private void Awake()
    {

    }

    void Start()
    {
        emanaCycle = new WaitForSeconds(eManaCool);
        StartCoroutine(eManaGen()); //마나 회복 시작
    }
 
    IEnumerator eManaGen() //적 소환력 회복
    {
        while (true)
        {
            yield return emanaCycle; //마나 사이클마다 회복
            if (ecMana < eMana) //꽉 찰 때까지
            {
                ecMana += 1; //1씩 회복
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
            //spawnPattern(); //패턴 변경
        }
    }
    */

        
}

