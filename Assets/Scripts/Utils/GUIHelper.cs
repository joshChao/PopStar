using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GUIHelper
{
    public static void ChangeUIImage(Image image,string path,string name){
        Sprite sp=Resources.Load(path+name,typeof(Sprite)) as Sprite;
        image.sprite=sp;
    }
    // Use this for initialization
    public static int GetDeviceWidth(int width)
    {
        return (int)(((float)width / 1080) * Screen.width);
    }

    public static int GetDeviceHeight(int height)
    {
        return (int)(((float)height / 1920) * Screen.height);
    }

    public static void PlayEffect(Transform root,Vector3 pos, string name,StarColorEnum color,Vector3 offset=default(Vector3))
    {
        Debug.Log(name);
        GameObject obj = Resources.Load(ResoucesPathEnum.effectPath + name, typeof(GameObject)) as GameObject;
        GameObject effect = GameObject.Instantiate(obj);
        effect.transform.SetParent(root);
        effect.SetActive(true);
        effect.transform.position = pos;
         effect.transform.localScale=Vector3.one;
         effect.transform.localRotation=Quaternion.identity;
         effect.transform.localPosition=effect.transform.localPosition+offset;
          EffectDestroy destory=effect.AddComponent<EffectDestroy>();
        ParticleSystem particle = effect.GetComponent<ParticleSystem>();
        Color tempColor=new Color();
        switch (color)
        {
            case StarColorEnum.BLUE:
               tempColor = ColorUtils.GetColorByRGBA(68, 190, 255);
                break;
            case StarColorEnum.GREEN:
               tempColor = ColorUtils.GetColorByRGBA(102, 202, 27);
                break;
            case StarColorEnum.PURPLE:
               tempColor = ColorUtils.GetColorByRGBA(192, 60, 255);
                break;
            case StarColorEnum.RED:
               tempColor = ColorUtils.GetColorByRGBA(225, 69, 110);
                break;
            case StarColorEnum.YELLOW:
               tempColor = ColorUtils.GetColorByRGBA(253, 181, 13);
                break;
        }
        float showTime = 0;
        if (particle != null) {
            showTime = particle.startLifetime;
            Renderer render=    particle.GetComponent<Renderer>();
           // render.material.SetColor("_Color",tempColor);
         }
         destory.SetDelayTime(showTime);
    }

 
}
