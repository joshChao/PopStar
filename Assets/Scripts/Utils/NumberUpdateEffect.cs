using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//数字刷新效果
public class NumberUpdateEffect : MonoBehaviour
{

    private int m_startNum;
    private int m_endNum;
    private int m_curNum;
    private float m_duration;
    private Text m_text;
    private int perFrameNum;//每帧变化量
                            // Use this for initialization
    private bool isStart = false;
    private float costTime;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            costTime += Time.deltaTime;
            if (costTime >= m_duration)
            {
                m_text.text = m_endNum.ToString();
                isStart = false;
            }
            else
            {
                m_curNum += perFrameNum;
                if ((perFrameNum > 0 && m_curNum > m_endNum) || (perFrameNum < 0 && m_curNum < m_endNum))
                {
                    m_curNum = m_endNum;
                    isStart = false;
                }
                m_text.text = m_curNum.ToString();
            }
        }
    }

    public void Init(Text text, int startNum, int endNum, float duration)
    {
        m_text = text;
        m_startNum = startNum;
        m_endNum = endNum;
        m_duration = duration;
        perFrameNum = (m_endNum - m_startNum) / 15;
        if (perFrameNum == 0)
            perFrameNum = (m_endNum - m_startNum) / Mathf.Abs(m_endNum - m_startNum);
        costTime = 0;
        m_curNum = startNum;
        isStart = true;
    }

    public static void ShowEffect(Text text, int finalNum, float duration)
    {
        NumberUpdateEffect effect = text.gameObject.GetComponent<NumberUpdateEffect>();
        if (effect == null)
        {
            effect = text.gameObject.AddComponent<NumberUpdateEffect>();
        }
        int tstartNum;
        if (!int.TryParse(text.text, out tstartNum))
        {
            Debug.LogError("this Text is no a Number text!");
            return;
        }
		if(tstartNum==finalNum)return;
        effect.Init(text, tstartNum, finalNum, duration);
    }
}
