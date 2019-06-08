using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/* Phyllotaxis 公式  --极坐标公式
 *  angle:   φ = n * degree
 *  radius:   r = c * √n
 *  x = r * cos(angle)
 *  y = r * sin(angle)
 *  degree : 0-360
 *  n: 0 - ∞
 *  c: 0.5
 */

/*TrailRenderer 组建实现拖尾
 * Lerping 控制拖尾运功轨迹与速度
 *
 *         Vector3.Lerp (from: Vector3, to: Vector3, t: float)
 * t 在 [0 - 1], t = 0时 返回from, t =1 时返回 to, t = 0.5 时返回 from 与 to 的平均值
 */
public class Phyllotaxis : MonoBehaviour {

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
        _phyllotaxisPosition = CalculatePhyllotaxis(_degree, _scale, _number);
        _startPosition = this.transform.localPosition;
        _endPosition = new Vector3(_phyllotaxisPosition.x, _phyllotaxisPosition.y, 0);
    }

    // Use this for initialization
    void Awake() {
        _trailRenderer = this.GetComponent<TrailRenderer>();
        _number = _numberStart;
        this.transform.localPosition = CalculatePhyllotaxis(_degree, _scale, _number);
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
        if (_userlerping)
        {
            if (_isLerping)
            {
                _lerpPosSpeed = Mathf.Lerp(_lerpPosSpeedMinMax.x, _lerpPosSpeedMinMax.y, _lerpPosAnimCurve.Evaluate(_audioPeer._audioBand[_lerpPosBand]));
                _lerpPosTimer = Time.deltaTime * _lerpPosSpeed;
                //Clamp01 将变换的位置设为时间的位置，钳制在0 - 1
                this.transform.localPosition = Vector3.Lerp(_startPosition, _endPosition, Mathf.Clamp01(_lerpPosTimer));
                if (_lerpPosTimer >= 1)
                {
                    _lerpPosTimer -= 1;
                    _number += _stepSize;
                    SetLerpPosition();
                }

            }
        }
        if(!_userlerping)
        {
            //坐标随_number的增加而变化
            _phyllotaxisPosition = CalculatePhyllotaxis(_degree, _scale, _number);
            this.transform.localPosition = new Vector3(_phyllotaxisPosition.x, _phyllotaxisPosition.y, _lerpPosAnimCurve.Evaluate(_audioPeer._audioBand[_lerpPosBand]));


            _number += _stepSize;
            _currentIteration++;
        }
    }
    /*private void FixedUpdate()
    {
        if (_userlerping)
        {
            if (_isLerping)
            {
                //计算轨迹运行时间
                float timeSinceStarted = Time.time - _timeStartedLerping;
                float percentageComplete = timeSinceStarted / _intervalLerp;
                //在percentageComplete时间内从_startPosition运动到_endPosition 
                this.transform.localPosition = Vector3.Lerp(_startPosition, _endPosition, percentageComplete);
                //当时间大于1s时开始下一个循环
                if (percentageComplete >= 0.97f)
                {
                    transform.localPosition = _endPosition;
                    _number += _stepSize;
                    _currentIteration++;
                    if (_currentIteration <= _maxIteration)
                    {
                        StartLerping();
                    }
                    else
                    {
                        _isLerping = false;
                    }
                }
            }
        }
        else
        {
            //坐标随_number的增加而变化
            _phyllotaxisPosition = CalculatePhyllotaxis(_degree, _scale, _number);
            this.transform.localPosition = new Vector3(_phyllotaxisPosition.x, _phyllotaxisPosition.y, 0);
            _number += _stepSize;
            _currentIteration++;
        }
    }*/

}
