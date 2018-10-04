# coreml-unity

## Overview

集成CoreML & Vision，使unity具有机器学习图片识别的能力。

## Require

- UnityARKitPlugin 2.0；
- TextMesh Pro；
- [GoogLeNetPlaces.mlmodel](https://developer.apple.com/documentation/coreml/mlmodel) 用于静态图片识别程序；
- MobileNet.mlmodel 用于结合Vision视频流识别；

## Startup

1. 打包 Xcode工程；
2. 通过 PBXProject.AddFileToBuild() 自动引入 ``Plugins`` 目录中的 ``mlmodel`` 文件，或 手动拖拽 ``mlmodel`` 到 Xcode工程根目录；
3. 在真机上运行;

## Roadmap

- [ ] unity中拍照，并对内容识别。
- [x] 优化oc回调。关于UnitySendMessage方法，NSString, const char*数据类型，等。
- [x] 获取arkit的pixelbuffer，实时传给本插件的oc层 CVPixelBufferRef。
- [x] 作为Pack包，依赖ARKit插件，避免session、buffer冲突。
- [x] 识别成功，将结果用TextMesh显示在世界坐标下。
- [ ] 使用点云，定位屏幕中心射线交点？
- [ ] runtime加载mlmodel。
- [ ] 获取更多的mlmodel，使用机器学习训练。

## Reference

- https://github.com/hanleyweng/CoreML-in-ARKit

## License

[MIT](https://github.com/brakmic/OpenCV/blob/master/LICENSE)
