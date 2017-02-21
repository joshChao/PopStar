using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum StarColorEnum
{
    RED,
    YELLOW,
    GREEN,
    PURPLE,
    BLUE,
    MAX
}
public class Star : MonoBehaviour
{
    private int m_x;
    private int m_y;
    private StarColorEnum m_color;
    private Button m_btn;
    private Image m_image;
	private RectTransform m_rectTran;
    // Use this for initialization
    void Awake()
    {
        m_btn = GetComponent<Button>();
        m_image = GetComponent<Image>();
		m_rectTran=GetComponent<RectTransform>();
    }
    public Button GetButton()
    {
        if (m_btn == null)
            m_btn = GetComponent<Button>();
        return m_btn;

    }
    public void SetColor(StarColorEnum color)
    {
        m_color = color;
        switch (m_color)
        {
            case StarColorEnum.BLUE:
                m_image.color = ColorUtils.GetColorByRGBA(68, 190, 255);
                break;
            case StarColorEnum.GREEN:
                m_image.color = ColorUtils.GetColorByRGBA(102, 202, 27);
                break;
            case StarColorEnum.PURPLE:
                m_image.color = ColorUtils.GetColorByRGBA(192, 60, 255);
                break;
            case StarColorEnum.RED:
                m_image.color = ColorUtils.GetColorByRGBA(225, 69, 110);
                break;
            case StarColorEnum.YELLOW:
                m_image.color = ColorUtils.GetColorByRGBA(253, 181, 13);
                break;
        }
    }

    public void SetIndex(int x, int y)
    {
        m_x = x;
        m_y = y;
       // m_image.rectTransform.sizeDelta=new Vector2(GUIHelper.GetDeviceWidth(107),GUIHelper.GetDeviceWidth(107));
    }

    public int GetX()
    {
        return m_x;
    }

    public int GetY()
    {
        return m_y;
    }

    public StarColorEnum GetColor()
    {
        return m_color;
    }

	public RectTransform GetRectTransform(){
		return m_rectTran;
	}
    // Update is called once per frame
    void Update()
    {

    }
}
