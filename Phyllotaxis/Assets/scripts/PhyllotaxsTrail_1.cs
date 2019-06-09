using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/*    通过_maxIteration来实现动画
 *    当 CurrentIteration >= _maxIteration 时分三种情况
 *     section 1 : stop everthiing 
 *     section 2 : repeat process and start at the start position
 *     section 3 : choose to go backwards 当到1000时 回到999 再回到998 一直回溯直到到0
 *     
 *     通过 频谱变化控制 scale 变化 实现动画
 */
public class PhyllotaxsTrail_1 : MonoBehaviour {

    public AudioPeer _audioPeer;       //AudioPeer获得音乐文件的频谱
    private Material _trailMat;             //拖尾材质
    public Color _trailColor;                //拖尾颜色

    public GameObject _dot;
    //定义公式参数
    public float _degree, _scale;
    public int _numberStart;
    private int _number;
    //拖尾渲染
    private TrailRenderer _trailRenderer;
    //控制速度大小
    public float TimerControl;

    //lerping 控制循环
    public bool _userlerping;  //用户控制是否使用Lerping
    public int _stepSize;   //number 增加步数
    public int _maxIteration;  //执行lerp的最大次数
    private bool _isLerping;    //判断lerp的开始与结束
    private Vector3 _startPosition, _endPosition;
    private int _currentIteration;   //当前Lerp的次数
    private float _lerpPosTimer, _lerpPosSpeed;
    public Vector2 _lerpPosSpeedMinMax;
    public AnimationCurve _lerpPosAnimCurve;
    public int _lerpPosBand;

    private Vector2 _phyllotaxisPosition;


    //
    private bool _forward;    //判断向前是回溯
    public bool _repeat, _invert;  //是否重复或倒置

    //Scaleing 
    //与lerping中控制变化方法一致
    public bool _useScaleAnimation, _useScaleCurve;
    public Vector2 _scaleAnimMinMax;
    public AnimationCurve _scaleAnimCurve;
    public float _scaleAnimSpeed;
    public int _scaleBand;
    private float _scaleTimer, _currentScale;

    //Phyllotaxis计算坐标
    private Vector2 CalculatePhyllotaxis(float degree, float scale, int number)
    {
        double angle = number * (degree * Mathf.Deg2Rad);
        float r = scale * Mathf.Sqrt(number);
        float x = r * (float)System.Math.Cos(angle);
        float y = r * (float)System.Math.Sin(angle);
        Vector2 vec = new Vector2(x, y);
        return vec;
    }

    //lerping 初始化
    void SetLerpPosition()
    {
        _phyllotaxisPosition = CalculatePhyllotaxis(_degree, _currentScale, _number);
        _startPosition = this.transform.localPosition;
        _endPosition = new Vector3(_phyllotaxisPosition.x, _phyllotaxisPosition.y, 0);
    }

    // Use this for initialization
    void Awake()
    {
        _currentScale = _scale;
        _forward = true;
        _trailRenderer = this.GetComponent<TrailRenderer>();
        _number = _numberStart;
        this.transform.localPosition = CalculatePhyllotaxis(_degree, _currentScale, _number);
        if (_userlerping)
        {
            _isLerping = true;
            SetLerpPosition();
        }

        //设置_trailRenderer 的材质
        _trailMat = new Material(_trailRenderer.material);
        _trailMat.SetColor("_TintColor", _trailColor);  //TintColor
        _trailRenderer.material = _trailMat;

    }

    private void Update()
    {
        //当使用scaleing 时
        if (_useScaleAnimation)
        {
            if (_useScaleCurve)
            {
                _scaleTimer += (_scaleAnimSpeed * _audioPeer._audioBand[_scaleBand]) * Time.deltaTime;
                if (_scaleTimer >= 1)
                {
                    _scaleTimer -= 1;
                }
                //将时间变化转化为Scale变化
                _currentScale = Mathf.Lerp(_scaleAnimMinMax.x, _scaleAnimMinMax.y, _scaleAnimCurve.Evaluate(_scaleTimer));
            }
            else
            {
                _currentScale = Mathf.Lerp(_scaleAnimMinMax.x, _scaleAnimMinMax.y, _audioPeer._audioBand[_scaleBand]);
            }
        }

        //当使用Lerping时
        if (_userlerping)
        {
            if (_isLerping)
            {
                _lerpPosSpeed = Mathf.Lerp(_lerpPosSpeedMinMax.x, _lerpPosSpeedMinMax.y, _lerpPosAnimCurve.Evaluate(_audioPeer._audioBand[_lerpPosBand]));
                _lerpPosTimer = Time.deltaTime * _lerpPosSpeed * TimerControl;
                //Clamp01 将变换的位置设为时间的位置，钳制在0 - 1
                this.transform.localPosition = Vector3.Lerp(_startPosition, _endPosition, Mathf.Clamp01(_lerpPosTimer));
                if (_lerpPosTimer >= 1)
                {
                    _lerpPosTimer -= 1;
                    if (_forward)      //向前
                    {
                        _number += _stepSize;
                        _currentIteration++;
                    }
                    else                   //回溯
                    {
                        _number -= _stepSize;
                        _currentIteration--;
                    }
                    if ((_currentIteration > 0) && (_currentIteration < _maxIteration))
                    {
                        SetLerpPosition();
                    }
                    else           //_currentIteration 小于0或大于_maxIteration 时
                    {
                        if (_repeat)
                        {
                            //section 3
                            if (_invert)
                            {
                                _forward = !_forward;
                                SetLerpPosition();
                            }
                            //section 2
                            else   
                            {
                                _number = _numberStart;
                                _currentIteration = 0;
                                SetLerpPosition();
                            }
                        }
                        //section 1
                        else   
                        {
                            _isLerping = false;
                        }
                    }

                }

            }
        }
        if (!_userlerping)
        {
            //坐标随_number的增加而变化
            _phyllotaxisPosition = CalculatePhyllotaxis(_degree, _currentScale, _number);
            this.transform.localPosition = new Vector3(_phyllotaxisPosition.x, _phyllotaxisPosition.y, 0);
            _number += _stepSize;
            _currentIteration++;
        }
    }
}
