using System;
using System.Runtime.InteropServices;

public class OSHookBridge
{
    [DllImport("__Internal")]
    public static extern void CallMethod();

    /// <summary>
    /// 向OC传递图片路径
    /// </summary>
    /// <param name="path">Path.</param>
    [DllImport("__Internal")]
    public static extern void ImagePathToIOS(string path);

    ///*
    /// <summary>
    /// 创建实例
    /// </summary>
    [DllImport("__Internal")]
    public static extern IntPtr CreateInstance();

    /// <summary>
    /// 调用.m实例中的方法
    /// </summary>
    [DllImport("__Internal")]
    public static extern int GetInstanceString(IntPtr instanceKey);
    //*/
}
