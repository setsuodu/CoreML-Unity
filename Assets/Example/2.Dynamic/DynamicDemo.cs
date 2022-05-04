using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class DynamicDemo : MonoBehaviour
{
    static string modelName = "MobileNetV2.mlmodel";

    public ARSession arSession;
    public ARCameraManager cameraManager;
    public XRCameraParams cameraParams;
    public XRCameraFrame cameraFrame;

    public Button m_LoadBtn;
    public Button m_StartBtn;
    public Button m_StopBtn;
    public Text m_ResultText;

    void Awake()
    {
        if (cameraManager == null)
            cameraManager = FindObjectOfType<ARCameraManager>();
    }

    void OnEnable()
    {
        if (cameraManager != null)
        {
            OSHookBridge.CreateSession(arSession.subsystem.nativePtr);
            Debug.Log("[Unity]OnEnable: pass nativePtr to OC");
        }
    }

    IEnumerator DownloadMLModel()
    {
        string filePath = $"{Application.persistentDataPath }/{modelName}";
        string url = $"192.168.1.103/download/{modelName}"; //WWW不支持https

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
        m_ResultText.text = log;
        doSetText?.Invoke(log);
    }

    public delegate void SetText(string str);
    public static SetText doSetText;
}