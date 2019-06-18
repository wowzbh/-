using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour {

    // Use this for initialization
    public Camera background;
    public Camera sceneLd;
    public GameObject BtnEsc;
    public GameObject BtnLd;
    public GameObject Phyllotaxis;
    public GameObject slider;
    public GameObject BtnPause;
    public GameObject BtnPrev;
    public GameObject BtnNext;
    public GameObject _Text;
    private AudioSource _audioSource;

	void Start () {
        _audioSource = GameObject.Find("_audioPeer").GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void btn_esc()
    {
        background.enabled = true;
        sceneLd.enabled = false;
        BtnEsc.SetActive(false);
        BtnLd.SetActive(true);
        BtnPause.SetActive(true);
        BtnPrev.SetActive(true);
        BtnNext.SetActive(true);
        Phyllotaxis.SetActive(false);
        slider.SetActive(true);
        _Text.SetActive(true);
    }
    public void btn_ld()
    {
        background.enabled = false;
        sceneLd.enabled = true;
        BtnEsc.SetActive(true);
        BtnLd.SetActive(false);
        BtnPause.SetActive(false);
        BtnPrev.SetActive(false);
        BtnNext.SetActive(false);
        Phyllotaxis.SetActive(true);
        slider.SetActive(false);
        _Text.SetActive(false);
    }
    public void btn_next()
    {
        AudioPeer._currentIndex++;
        if (AudioPeer._currentIndex >= Date_demo.Instance.clips.Length)
        {
            AudioPeer._currentIndex = 0;
        }
        _audioSource.clip = Date_demo.Instance.clips[AudioPeer._currentIndex];
        _audioSource.time = 0;
        _audioSource.Play();
    }

    public void btn_prev()
    {
        AudioPeer._currentIndex--;
        if (AudioPeer._currentIndex < 0)
        {
            AudioPeer._currentIndex = Date_demo.Instance.clips.Length - 1;
        }
        _audioSource.clip = Date_demo.Instance.clips[AudioPeer._currentIndex];
        _audioSource.time = 0;
        _audioSource.Play();
    }

    public void btn_pause()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Pause();
            return;
        }
        else
        {
            _audioSource.Play();
            return;
        }
    }
}
