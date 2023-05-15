using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    // 0 : 메인, 1 : 업글 씬, 2 : 전투 씬
    public string[] SceneName = { "MainSecne", "UpScene", "BattleScene" };
    //씬 이동 함수, 씬 코드를 인자로 받아 이동
    public void SceneMV(int SceneCode)
    {
        SceneManager.LoadScene(SceneName[SceneCode]);
    }
}
