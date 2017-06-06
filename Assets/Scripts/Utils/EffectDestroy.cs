using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroy : MonoBehaviour
{

    float delayTime = 0;
    void Start()
    {
        StartCoroutine(DelayDestory(delayTime));
    }
    public void SetDelayTime(float tdelayTime)
    {
        delayTime = tdelayTime;
    }

    IEnumerator DelayDestory(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
