using System.IO;
using System.Xml;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using UnityEditor.Callbacks;

public class XCodePostProcess
{
	[PostProcessBuild]
	static void OnPostprocessBuild(BuildTarget buildTarget, string path)
	{
		if (buildTarget == BuildTarget.iOS)
		{
			ModifyProj(path);
			SetPlist(path);
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

        // 添加.tbd
        pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile("usr/lib/libz.tbd", "Frameworks/libz.tbd", PBXSourceTree.Sdk));
        //pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile("usr/lib/libc++.tbd", "Frameworks/libc++.tbd", PBXSourceTree.Sdk));

		// 添加系统框架.framework
		pbxProj.AddFrameworkToProject(targetGuid, "Vision.framework", false);
		pbxProj.AddFrameworkToProject(targetGuid, "CoreML.framework", false);
		pbxProj.AddFrameworksBuildPhase(targetGuid);
      
        // 添加一般文件
		string fileName = "MobileNet.mlmodel";
		string srcPath = Path.Combine("Assets/Plugins", fileName);
		string dstPath = "Libraries/" + fileName;
        /*
		Directory.CreateDirectory(Path.Combine(path, "Libraries"));
		File.Copy(srcPath, Path.Combine(path, dstPath));
		Debug.Log("Copy To ==>> " + Path.Combine(path, dstPath));
		pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile(dstPath, dstPath, PBXSourceTree.Source));
        */
		File.Copy(srcPath, Path.Combine(path, fileName));
		pbxProj.AddFileToBuild(targetGuid, pbxProj.AddFile(fileName, dstPath, PBXSourceTree.Source));

		pbxProj.AddFrameworkToProject(targetGuid, fileName, false); //系统内置框架，optional/required
        pbxProj.AddFrameworksBuildPhase(targetGuid);

		// 增加框架搜索路径
		//pbxProj.SetBuildProperty(targetGuid, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
		//pbxProj.AddBuildProperty(targetGuid, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)/Frameworks");

		// 设置teamID
		//pbxProj.SetBuildProperty(targetGuid, "DEVELOPMENT_TEAM", "AUF3355GWB"); //填的是组织单位，而不是用户ID

		File.WriteAllText(projPath, pbxProj.WriteToString());
	}

	static void SetPlist(string path)
	{
		string plistPath = path + "/Info.plist";
		PlistDocument plist = new PlistDocument ();
		plist.ReadFromString (File.ReadAllText (plistPath));

		//Information Property List
		PlistElementDict iplist = plist.root;

		File.WriteAllText(plistPath, plist.WriteToString());
	}

	// ちょっとしたユーティリティ関数（http://goo.gl/fzYig8を参考）
    internal static void CopyAndReplaceDirectory(string srcPath, string dstPath)
    {
        if (Directory.Exists(dstPath))
            Directory.Delete(dstPath);
		
        if (File.Exists(dstPath))
            File.Delete(dstPath);

        Directory.CreateDirectory(dstPath);

		foreach (string file in Directory.GetFiles(srcPath))
            File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)));

		foreach (string dir in Directory.GetDirectories(srcPath))
            CopyAndReplaceDirectory(dir, Path.Combine(dstPath, Path.GetFileName(dir)));
    }
}
