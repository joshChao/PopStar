using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameRoot : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    public void Start()
    {
        Singleton<UIManager>.Create();
        Singleton<ContextManager>.Create();
        Singleton<Localization>.Create();
    }
    private void SetViewPort(float width,float height){
            float xScale=width/Screen.width;
            float yScale=height/(xScale*Screen.height);        
            float y=(Screen.height-height/xScale)/(2*Screen.height);
            mainCamera.rect=new Rect(0,y,1,yScale);
    }
    void OnGUI()
    {
   /*     float posy = 50;
        if (GUI.Button(new Rect(0, posy, 100, 50), "480x800"))
        {
            SetViewPort(480,800);
        }
        posy += 70;
         if (GUI.Button(new Rect(0, posy, 100, 50), "480x854"))
        {
            SetViewPort(480,854);
        }
         posy += 70;
         if (GUI.Button(new Rect(0, posy, 100, 50), "720x1280"))
        {
            SetViewPort(720,1280);
        }
         posy += 70;
         if (GUI.Button(new Rect(0, posy, 100, 50), "768x1280"))
        {
            SetViewPort(768,1280);
        }*/
    }
}

