using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class CamMove : MonoBehaviour
{
    public float speed = 0f;//1 or -1
    public float MVspeed = 3f;//camera moving speed
    public float MVspeed1 = 3f;//background moving speed
    public float MVspeed2 = 3f;

    public GameObject bg1;
    public GameObject bg2;
    public void setSpeed(float setspeed)
    {
        speed = setspeed;
    }
    void Update()
    { //limit = +-12, speed = +-1 
        if(transform.position.x + speed <= 13f && transform.position.x + speed >= -13f)
        {
            transform.Translate(MVspeed * speed * Time.deltaTime, 0, 0);//카메라 이동
            bg1.transform.Translate(MVspeed1 * speed * Time.deltaTime, 0, 0);//배경 패럴렉스
            bg2.transform.Translate(MVspeed2 * speed * Time.deltaTime, 0, 0);
        }
    }
}
