using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    // 0 : ����, 1 : ���� ��, 2 : ���� ��
    public string[] SceneName = { "MainSecne", "UpScene", "BattleScene" };
    //�� �̵� �Լ�, �� �ڵ带 ���ڷ� �޾� �̵�
    public void SceneMV(int SceneCode)
    {
        SceneManager.LoadScene(SceneName[SceneCode]);
    }
}
