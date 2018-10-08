using System;
using System.Runtime.InteropServices;

public class OSHookBridge
{
	/// <summary>
	/// 向OC传递图片路径
	/// </summary>
	/// <param name="path">Path.</param>
	[DllImport("__Internal")]
	public static extern void ImagePathToIOS(string path);

	[DllImport("__Internal")]
	public static extern void StartCoreML();

    [DllImport("__Internal")]
    public static extern void LoadMLModel(string path);

    [DllImport("__Internal")]
    public static extern void StartVision();

    [DllImport("__Internal")]
    public static extern void StopVision();
}
