using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Microsoft.Win32;

public class test : MonoBehaviour {

    public AudioSource _audioSource;
    // Use this for initialization
    void Start() {

    }
    IEnumerator GetAudioClip()
    {
        using (var www = UnityWebRequestMultimedia.GetAudioClip("flie://F:/CloudMusic/赵紫骅 - 可乐.mp3", AudioType.MPEG))
        {
            yield return www.Send();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                _audioSource.clip = myClip;
            }
        }
    }

}
