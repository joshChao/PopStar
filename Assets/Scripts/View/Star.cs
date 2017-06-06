using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public enum StarColorEnum
{
    GREEN,
    YELLOW,
    BLUE,
    PURPLE,
    RED,
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
        m_rectTran = GetComponent<RectTransform>();
        Tweener tween = transform.DOScale(0.9f, 0.5f);
        //  tween.SetEase(Ease.);
        tween.SetLoops(-1, LoopType.Yoyo);
        tween.Kill();
    }
    public Button GetButton()
    {
        if (m_btn == null)
            m_btn = GetComponent<Button>();
        return m_btn;

    }
    Tweener tween;
    public void ShowStarTip(bool isShow)
    {
        if (isShow)
        {
            tween = transform.DOScale(0.9f, 0.6f);
            //  tween.SetEase(Ease.);
            tween.SetLoops(-1, LoopType.Yoyo);


        }
        else
        {
            transform.localScale = Vector3.one;
            if (tween != null) tween.Kill();
        }

    }

    public void SetColor(StarColorEnum color)
    {
        m_color = color;
        /*  switch (m_color)
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
          }*/
        switch (m_color)
        {
            case StarColorEnum.GREEN:
                GUIHelper.ChangeUIImage(m_image, ResoucesPathEnum.starImagePath, "kuai_01");
                break;
            case StarColorEnum.YELLOW:
                GUIHelper.ChangeUIImage(m_image, ResoucesPathEnum.starImagePath, "kuai_02");
                break;
            case StarColorEnum.BLUE:
                GUIHelper.ChangeUIImage(m_image, ResoucesPathEnum.starImagePath, "kuai_03");
                break;
            case StarColorEnum.PURPLE:
                GUIHelper.ChangeUIImage(m_image, ResoucesPathEnum.starImagePath, "kuai_04");
                break;
            case StarColorEnum.RED:
                GUIHelper.ChangeUIImage(m_image, ResoucesPathEnum.starImagePath, "kuai_05");
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

    public RectTransform GetRectTransform()
    {
        return m_rectTran;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
