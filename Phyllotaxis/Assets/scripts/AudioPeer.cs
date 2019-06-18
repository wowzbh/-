using UnityEngine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class AudioPeer : MonoBehaviour
{

    private AudioSource _audioSource;
    private float[] _freqBand = new float[8];
    private float[] _bandBuffer = new float[8];
    private float[] _bufferDecrease = new float[8];

    public float[] _freqBandHighest = new float[8];
    [HideInInspector]
    public float[] _audioBand, _audioBandBuffer;
    //存储频谱信息的数组 左右声道
    private float[] _samplesLeft = new float[512];
    private float[] _samplesRight = new float[512];
    //判断左右声道
    public enum _channel { Stereo, Left, Right};
    public _channel channel = new _channel();

    //平滑变化
    public float _audioProfile;

    //进度条控制
    SliderUI _slider;        //进度控制
    int _currentIndex = 0; //当前音频索引

    void Start()
    {
        //实例化音乐文件
        _audioBand = new float[8];
        _audioBandBuffer = new float[8];
        _audioSource = this.GetComponent<AudioSource>();
        _slider = GameObject.Find("Slider").GetComponent<SliderUI>();      //获取重写的Slider组件
        AudioProfile(_audioProfile);
    }

    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
        AudioSlider();
    }

    void AudioSlider()      //进度条实时更新
    {
        if (_audioSource == null || _audioSource.clip == null)
        {
            return;
        }
        if (_audioSource.isPlaying && SliderUI.State == SliderState.Normal)
        {
            _slider.value = _audioSource.time / _audioSource.clip.length;     //进度条实时更新
        }
        if (SliderUI.State == SliderState.Up)      //拖拽进度条
        {
            _audioSource.time = _slider.value * _audioSource.clip.length;
            _slider.Reset();
        }
    }

    void AudioProfile(float audioProfile)
    {
        for (int i = 0; i < 8; i++)
        {
            _freqBandHighest[i] = audioProfile;
        }
    }

    void CreateAudioBands()           //将 freqBand 与 _bandBuffer 数组转化到 _audioBand中
    {
        for (int i = 0; i < 8; i++)
        {
            if (_freqBand[i] > _freqBandHighest[i])
            {
                _freqBandHighest[i] = _freqBand[i];
            }
            _audioBand[i] = (_freqBand[i] / _freqBandHighest[i]);
            _audioBandBuffer[i] = (_bandBuffer[i] / _freqBandHighest[i]);
        }
    }

       
    void GetSpectrumAudioSource()     //获取音乐的频谱信息，并将频谱储存在_samples数组中
    {
        _audioSource.GetSpectrumData(_samplesLeft, 0, FFTWindow.Blackman);
        _audioSource.GetSpectrumData(_samplesRight, 1, FFTWindow.Blackman);

    }

    void BandBuffer()          //band 变化的平滑设置
    {
        for (int i = 0; i < 8; ++i)
        {
            if (_freqBand[i] > _bandBuffer[i])
            {
                _bandBuffer[i] = _freqBand[i];
                _bufferDecrease[i] = 0.005f;
            }
            if (_freqBand[i] < _bandBuffer[i])
            {
                _bandBuffer[i] -= _bufferDecrease[i];
                _bufferDecrease[i] *= 1.2f;
            }
        }
    }

    void MakeFrequencyBands()        //制作频率数组  将512个频谱数组转换到频率数组中
    {
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                if (channel == _channel.Stereo)
                {
                    average += (_samplesLeft[count] + _samplesRight[count]) * (count + 1);
                }
                if (channel == _channel.Left)
                {
                    average += _samplesLeft[count] * (count + 1);
                }
                if (channel == _channel.Right)
                {
                    average += _samplesRight[count] * (count + 1);

                }
                count++;
            }
            average /= count;
            _freqBand[i] = average * 10;
        }
    }
}
