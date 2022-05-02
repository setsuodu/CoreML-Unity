using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DynamicDemo : MonoBehaviour
{
    public ARSession arSession;
    public ARCameraManager cameraManager;
    public XRCameraParams cameraParams;
    public XRCameraFrame cameraFrame;

    //public System.IntPtr native;

    public Button m_LoadBtn;
    public Button m_StartBtn;
    public Button m_StopBtn;

    void Awake()
    {
        if (cameraManager == null)
            cameraManager = FindObjectOfType<ARCameraManager>();

        //cameraParams = new XRCameraParams
        //{
        //    screenWidth = Screen.width,
        //    screenHeight = Screen.height,
        //    screenOrientation = ScreenOrientation.Portrait,
        //    zFar = Camera.main.farClipPlane,
        //    zNear = Camera.main.nearClipPlane,
        //};
        //cameraFrame = new XRCameraFrame();
    }

    void OnEnable()
    {
        if (cameraManager != null)
        {
            OSHookBridge.CreateSession(arSession.subsystem.nativePtr);
            Debug.Log("[Unity]OnEnable: pass nativePtr to OC");
        }
    }

    string url = "http://192.168.1.103/MobileNetV2.mlmodel"; //WWW不支持https

    IEnumerator DownloadMLModel()
    {
        string filePath = Application.persistentDataPath + "/MobileNet.mlmodel";

        if (!File.Exists(filePath))
        {
            Debug.Log("开始下载MLModel...");

            WWW www = new WWW(url);
            while (!www.isDone)
            {
                Debug.Log(www.progress * 100 + "%");
                yield return null;
            }
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.Log(www.error);
                yield break;
            }

            if (www.isDone)
            {
                byte[] bytes = www.bytes;
                FileStream fileStream = File.Create(filePath);
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
            }
        }

        Debug.Log("下载完成:" + filePath);

        OSHookBridge.LoadMLModel(filePath);
    }

    public void OnLoadButtonClick()
    {
        StartCoroutine(DownloadMLModel());
    }

    public void OnStartButtonClick()
    {
        OSHookBridge.StartVision();
    }

    public void OnStopButtonClick()
    {
        OSHookBridge.StopVision();
    }

    // coreml识别到的结果回调，oc层异步每帧调用
    public void RecogniseCallback(string log)
    {
        Debug.Log("识别结果 ==>> " + log);
    }
}