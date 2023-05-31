using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;

public class spawn : MonoBehaviour
{
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
        _Pool = new ObjectPool<Unit>(CreateUnit, onGetUnit, OnReleaseUnit, OnDestroyUnit, maxSize:20);
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
            var unit = _Pool.Get();
            unit.transform.position = spawnPoint.transform.position;
            //spawnPattern(); //패턴 변경
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

