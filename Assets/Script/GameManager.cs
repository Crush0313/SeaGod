using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamManager : MonoBehaviour
{
    public static int Money;
    public static int[] Upgrade;

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
}
