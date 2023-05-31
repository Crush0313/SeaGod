using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int Money;
    public static int[] Upgrade;
    public GameObject Darker;
    public bool isPause = false;
    public bool isFast = false;

   
    void Start()
    {
        //Upgrade[9] = {1,0,0,0,0,0,0,0,0};
    }

    public void pMoney()
    {
        Money += 1000;
    }
    public void mMoney()
    {
        Money -= 1000;
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Money", Money);
        for (int i = 0; i < 9; i++)
        {
            PlayerPrefs.SetInt("Up0" +i, Upgrade[i]);
        }
        PlayerPrefs.Save();

    }
    public void Load()
    {
        if (!PlayerPrefs.HasKey("Up00"))
            return;
        Money = PlayerPrefs.GetInt("Money");
        int Qindex = PlayerPrefs.GetInt("Qindex");

        for (int i = 0; i < 9; i++)
        {
            Upgrade[i] = PlayerPrefs.GetInt("Up0" + i);
        }

    }
    public void TheWorld()
    {
        if (isPause) //멈춰있음 - 이제 되돌림
        {
            Time.timeScale = 1;
            isPause = false;
            Darker.SetActive(false);
        }
        else //안 멈춘 상태 - 이제 멈춤
        {
            Time.timeScale = 0;
            isPause = true;
            Darker.SetActive(true);
        }
    }
    public void TheKorean()
    {
        if (isFast) //빠름 - 이제 되돌림
        {
            Time.timeScale = 1;
            isFast = false;
        }
        else //안 빠른 상태 - 이제 빠름
        {
            Time.timeScale = 2;
            isFast = true;
        }
    }
}
