using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 
 * 通过.mm去调用.m中的方法
 */
public class Demo : MonoBehaviour
{
    public Image m_textImage;
    public Text m_resultText;

    void Start()
    {
        OSHookBridge.CallMethod();
    }

    public void StartCoreML()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IntPtr handle = OSHookBridge.CreateInstance();
            Debug.Log(OSHookBridge.GetInstanceInt(handle));
        }
    }

    public void ToIOS()
    {
        Texture2D t2d = m_textImage.sprite.texture;

        byte[] bytes = t2d.EncodeToJPG();
        string savePath = Application.persistentDataPath + "/testImage.jpg";
        //[iOS]/var/mobile/Containers/Data/Application/1B59CF4F-855D-4712-9375-C7131B9FC560/Documents/testImage.jpg

        File.WriteAllBytes(savePath, bytes);
        Debug.Log("已保存到：" + savePath);

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            OSHookBridge.ImagePathToIOS(savePath);
        }
    }

    public void GetResult(string res)
    {
        m_resultText.text = res;
    }
}
