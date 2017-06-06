using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class StarMainMenuContext : BaseContext
{
    public StarMainMenuContext()
        : base(UIType.StarMainMenu)
    {

    }
}
public class StarMainMenuView : AnimateView
{
    [SerializeField]
    private Button _NormalModle;
    public override void OnEnter(BaseContext context)
    {
        base.OnEnter(context);
        List<string>playList=new List<string>();
        playList.Add("ARENA");
        playList.Add("ARENA");
     //   AudioHelper.PlayList(playList); 
       // AudioController.PlayMusicPlaylist("testList");
    }

    public override void OnExit(BaseContext context)
    {
        base.OnExit(context);
    }

    public override void OnPause(BaseContext context)
    {
        _animator.SetTrigger("OnExit");
    }

    public override void OnResume(BaseContext context)
    {
        _animator.SetTrigger("OnEnter");
    }
    
    public void NormalModelCallBack()
    {
        Singleton<ContextManager>.Instance.Push(new StarGameMainContext());
    }
  
    public void OpenSetting(){
         Singleton<ContextManager>.Instance.Push(new StarSettingContext());
    }
}
