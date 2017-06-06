using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StarSettingContext : BaseContext
{
    public StarSettingContext()
        : base(UIType.StarSetting)
    {

    }
}
public class StarSettingView : AnimateView {

	 public override void OnEnter(BaseContext context)
    {
        base.OnEnter(context);     
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
	public void Close(){
		Singleton<ContextManager>.Instance.Pop();
	}
}
