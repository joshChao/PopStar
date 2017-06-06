using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHelper
{
    public static void PlayList(List<string> playList)
    {				
		var category = AudioController.GetCategory( "StoryMusic" );
        foreach (var strAudio in playList)
        {
            AudioClip clip = Resources.Load(ResoucesPathEnum.musicPath + strAudio, typeof(AudioClip)) as AudioClip;
            AudioController.AddToCategory( category, clip, strAudio);
        }
		AudioController.AddPlaylist("test",playList.ToArray());
		AudioController.Instance.loopPlaylist =true;
		AudioController.PlayMusicPlaylist("test");
    }

}
