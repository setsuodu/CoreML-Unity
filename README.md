# coreml-unity

### Description
集成CoreML.framework，使unity工程具有机器学习图片识别的能力。

### Features
- 静态图片识别。
- 视频流识别。
- 通过 PBXProject.AddFileToBuild 自动引入.mlmodel文件。

### Startup
1. 下载最新的ARKit插件，替换UnityARKitPlugin包。
2. 下载最新的TextMesh Pro，导入工程。
3. 下载MobileNet.mlmodel。
4. Build Xcode工程。
5. 手动拖拽 .mlmodel到Xcode工程中。

### Roadmap
- [ ] unity中拍照，并对内容识别。
- [x] 优化oc回调。关于UnitySendMessage方法，NSString, const char*数据类型，等。
- [x] 获取arkit的pixelbuffer，实时传给本插件的oc层 CVPixelBufferRef。
- [x] 作为Pack包，依赖ARKit插件，避免session、buffer冲突。
- [x] 识别成功，将结果用TextMesh显示在世界坐标下。
- [ ] 使用点云，定位屏幕中心射线交点？
- [ ] runtime加载mlmodel。
- [ ] 获取更多的mlmodel，使用机器学习训练。

### Reference
- https://www.jianshu.com/p/ed8e76081cad
- https://github.com/hanleyweng/CoreML-in-ARKit
