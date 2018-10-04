using System.IO;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;
using UnityEditor.XCodeEditor;

public class XCodePostProcess
{
	[PostProcessBuild]
	static void OnPostprocessBuild(BuildTarget buildTarget, string path)
	{
		if (buildTarget == BuildTarget.iOS)
		{
			ModifyProj(path);
            //SetPlist(path); //不需要配置
            //ModifyOC(path);
        }
    }

    /// <summary>
    /// 修改Xcode工程配置
    /// </summary>
	/// <param name="path">Xcode工程根目录</param>
	public static void ModifyProj(string path)
	{
		string projPath = PBXProject.GetPBXProjectPath(path);
		PBXProject pbxProj = new PBXProject();
        pbxProj.ReadFromString(File.ReadAllText(projPath));

		// 配置目标TARGETS
		string targetGuid = pbxProj.TargetGuidByName("Unity-iPhone");

		// 添加系统框架.framework
		pbxProj.AddFrameworkToProject(targetGuid, "Vision.framework", false);
		pbxProj.AddFrameworkToProject(targetGuid, "CoreML.framework", false);
		pbxProj.AddFrameworksBuildPhase(targetGuid);

        /* 还是需要手动配置
        // 添加一般文件
		string fileName = "MobileNet.mlmodel"; //必须输出到 Build/Library 文件夹中
		string srcPath = Path.Combine("Assets/Plugins", fileName);
		string dstPath = "Libraries/" + fileName;
		File.Copy(srcPath, Path.Combine(path, dstPath));
		//pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile(fileName, dstPath, PBXSourceTree.Source)); //xcode报红
        //pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile(dstPath, fileName, PBXSourceTree.Source)); //加到了外面
		pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile(dstPath, dstPath, PBXSourceTree.Source)); //xcode报红

		// GoogLeNetPlaces.mlmodel
		string file1 = "GoogLeNetPlaces.mlmodel";
		string srcPath1 = Path.Combine("Assets/Plugins", file1);
		string dstPath1 = "Libraries/" + file1;
        File.Copy(srcPath, Path.Combine(path, dstPath1));
        pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile(dstPath1, dstPath1, PBXSourceTree.Source));
        */

		File.WriteAllText(projPath, pbxProj.WriteToString());
	}

	static void SetPlist(string path)
	{
		string plistPath = path + "/Info.plist";
		PlistDocument plist = new PlistDocument ();
		plist.ReadFromString (File.ReadAllText (plistPath));

		// Information Property List
		PlistElementDict plistDict = plist.root;

		File.WriteAllText(plistPath, plist.WriteToString());
	}

    // 修改OC代码
	static void ModifyOC(string path)
	{
		// .mm文件
		XClass mm = new XClass(path + "/Libraries/UnityARKitPlugin/Plugins/iOS/UnityARKit/NativeInterface/ARSessionNative.mm");

		// 在指定代码后面增加一行代码
        mm.WriteBelow("#include \"UnityAppController.h\"", "VNSequenceRequestHandler *sequenceRequestHandler;");
		mm.WriteBelow("#include \"UnityAppController.h\"", "VNRequest *visionCoreMLRequest;");
		mm.WriteBelow("#include \"UnityAppController.h\"", "#import \"MobileNet.h\"");
		mm.WriteBelow("#include \"UnityAppController.h\"", "#import <Vision/Vision.h>");
      
		mm.WriteBelow("_classToCallbackMap = [[NSMutableDictionary alloc] init];",
		              "[self setupVisionRequests];\n[self loopCoreMLUpdate];");
    }
}
