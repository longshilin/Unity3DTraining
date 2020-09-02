using System;
using UnityEngine;

public class Test : MonoBehaviour
{
    public enum MoveMode
    {
        cam,
        obj,
    }

    //上次鼠标位置
    Vector2 prevMousePos = Vector3.zero;

    //滑动结束时的瞬时速度
    Vector3 Speed = Vector3.zero;

    //每帧偏差
    Vector3 offSet = Vector3.zero;

    //鼠标开始位置
    Vector3 startMousePosition = Vector3.zero;

    //速度衰減率
    public float decelerationRate = 0.01f;

    //摄像机
    public Camera m_camera;

    //移动模式
    public MoveMode m_moveMode = MoveMode.obj;

    // 滑动范围
    public float m_MinRange = -80.0f;
    public float m_MaxRange = -20.0f;

    void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        //按下时记录位置
        if (Input.GetMouseButtonDown(0))
        {
            prevMousePos = Input.mousePosition;
            startMousePosition = Input.mousePosition;
        }

        //移动时更新位置
        if (Input.GetMouseButton(0))
        {
            Vector3 curMousePosition = Input.mousePosition; //当前鼠标的屏幕坐标系
            // 计算偏差值
            // a.正交相机直接用下面这种方式获取偏差值
            // offSet = m_camera.ScreenToWorldPoint(curMousePosition) - m_camera.ScreenToWorldPoint(prevMousePos);
            // b.透视相机使用下面通过射线方式获取偏差值
            offSet = GetWorldPositionOnPlane(curMousePosition) - GetWorldPositionOnPlane(prevMousePos);
            Debug.Log(offSet);
            prevMousePos = curMousePosition;
            //瞬时速度
            Speed = offSet / Time.deltaTime;
        }
        else //最后递减
        {
            Speed *= Mathf.Pow(decelerationRate, Time.deltaTime);
            if (Mathf.Abs(Vector3.Magnitude(Speed)) < 1)
            {
                Speed = Vector3.zero;
            }
        }

        Move(Speed, m_MinRange, m_MaxRange);
    }

    /// <summary>
    /// 通过射线获取屏幕点映射到水平面上的点坐标
    /// </summary>
    /// <param name="screenPosition"></param>
    /// <returns></returns>
    public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane xy = new Plane(Vector3.down, Vector3.zero);
        float distance;
        xy.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    public void Move(Vector3 speed, float minRange, float maxRange)
    {
        if (Vector3.Magnitude(Speed) == 0)
        {
            return;
        }

        speed.x = 0;

        Debug.Log("Current Speed" + Vector3.Magnitude(speed));
        if (m_moveMode == MoveMode.obj)
        {
            transform.position += speed * Time.deltaTime;
        }
        else
        {
            // m_camera.transform.localPosition -= speed * Time.deltaTime;

            // 滑动范围 - 修正相机位置 z: -20 ~ -80
            var pos = m_camera.transform.position;
            pos -= speed * Time.deltaTime;
            pos.z = Math.Max(pos.z, minRange);
            pos.z = Math.Min(pos.z, maxRange);
            m_camera.transform.position = pos;
        }
    }
}