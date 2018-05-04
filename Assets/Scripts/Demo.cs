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
    public Image m_image;

    void Start()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IntPtr handle = Bridge.CreateInstance();
            Debug.Log(Bridge.GetInstanceInt(handle));
        }
    }

    public void ToIOS()
    {
        Texture2D t2d = m_image.sprite.texture;

        byte[] bytes = t2d.EncodeToJPG();
        string savePath = Application.persistentDataPath + "/testImage.jpg";
        //[iOS]/var/mobile/Containers/Data/Application/1B59CF4F-855D-4712-9375-C7131B9FC560/Documents/testImage.jpg

        File.WriteAllBytes(savePath, bytes);
        Debug.Log("已保存到：" + savePath);

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Bridge.ImagePathToIOS(savePath);
        }
    }
}
