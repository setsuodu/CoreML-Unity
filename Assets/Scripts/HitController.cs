using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class HitController : MonoBehaviour
{
    public GameObject prefab;
    GameObject avatar;
    UnityARSessionNativeInterface m_session;
	string result;

    void Awake()
    {
        m_session = UnityARSessionNativeInterface.GetARSessionNativeInterface();
    }

    void Update()
    {
        if(Input.touchCount > 0)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Spawn();
            }
        }
    }

    void Spawn()
    {
        //屏幕坐标转视口坐标
        //手指点击位置
        //Vector3 screenPos = Camera.main.ScreenToViewportPoint(Input.GetTouch(0).position);
        //屏幕中心位置
        Vector3 screenPos = Camera.main.ScreenToViewportPoint(new Vector2(Screen.width / 2, Screen.height / 2));

        ARPoint point = new ARPoint
        {
            x = screenPos.x,
            y = screenPos.y
        };

        // 处理捕捉类型，点还是面，控制虚拟物体可以在哪里创建
        ARHitTestResultType[] resultTypes = {
            //ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingGeometry, //考虑尺寸和形状（没三角面），必须在Plane内创建。Plane带形状的？？
            //ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, //尊重平面有限尺寸，只能创建在Plane中，同一高度会自动合并
            // if you want to use infinite planes use this:
            ARHitTestResultType.ARHitTestResultTypeExistingPlane, //不考虑屏幕大小，必须先扫出至少一个Plane，可以创建在Plane外面，同一高度临近平面自动合并
            //ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane, //搜索过程中？？
            //ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane, //搜索过程中？？
            ARHitTestResultType.ARHitTestResultTypeFeaturePoint //Plane会出现，没有意义，可以在任意空中创建
        };

        foreach(var type in resultTypes)
        {
            List<ARHitTestResult> hitResults = m_session.HitTest(point, type);
            Debug.Log(type.ToString() + ": " + hitResults.Count);
            if(hitResults.Count > 0)
            {
                if(avatar == null)
                {
                    avatar = Instantiate(prefab);
                }
                avatar.transform.position = UnityARMatrixOps.GetPosition(hitResults[0].worldTransform);
				avatar.transform.rotation = UnityARMatrixOps.GetRotation(hitResults[0].worldTransform);
				avatar.GetComponent<TextMesh>().text = result;
            }
        }
    }

    // coreml识别到的结果回调，oc层异步每帧调用
    public void RecogniseCallback(string log)
    {
        Debug.Log("识别结果 ==>> " + log);
        result = log;
    }
}
