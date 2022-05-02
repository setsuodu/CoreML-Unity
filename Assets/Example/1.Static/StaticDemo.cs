using System.IO;
using UnityEngine;
using UnityEngine.UI;

/* 
 * 调用.m 中的 extern c 方法
 */
public class StaticDemo : MonoBehaviour
{
    public Image m_textImage;
    public Text m_resultText;

    // 通过unity获取外部（摄像头等）图像，传入sdk
    public void SaveToIOS()
    {
        Texture2D t2d = m_textImage.sprite.texture;

        //byte[] bytes = t2d.EncodeToJPG(); //新版Unity在iOS平台不支持了。
        byte[] bytes = DeCompress(t2d).EncodeToPNG();

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

    public Texture2D DeCompress(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}