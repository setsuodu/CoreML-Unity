using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* 
 * 调用.m 中的 extern c 方法
 */
public class Demo : MonoBehaviour
{
    public Image m_textImage;
    public Text m_resultText;

    void Start()
    {
		
    }
   
    // 通过unity获取外部（摄像头等）图像，传入sdk
    public void SaveToIOS()
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

    public void StartCoreML()
    {
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			OSHookBridge.StartCoreML();
		}
    }

    // 插件回调
    public void GetResult(string res)
    {
        m_resultText.text = res;
    }
}
