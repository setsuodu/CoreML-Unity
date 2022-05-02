# CoreML-Unity

[![GitHub](https://img.shields.io/github/license/mashape/apistatus.svg?style=social)](https://github.com/brakmic/OpenCV/blob/master/LICENSE)

## [Obstacle]
由于Xcode和ARKit升级，本代码已经不再适应最新的平台，有需要可以使用老版本工具编译，参考版本已经在下面列出。

## 概览

集成 CoreML & Vision，使Unity具有机器学习图片识别的能力。

## 开发环境

- [Unity 2018.x.x](https://unity.cn/releases/lts/2018)；
- [Xcode 11.x.x](https://developer.apple.com/download/all/?q=xcode)；
- [UnityARKitPlugin 2.0](https://bitbucket.org/Unity-Technologies/unity-arkit-plugin)；
- [GoogLeNetPlaces.mlmodel](https://developer.apple.com/machine-learning/build-run-models/); //用于静态图片识别程序（链接已失效，新版的CoreML也不支持了）
- (*)MobileNet.mlmodel; //可替代GoogLeNetPlaces，用于结合Vision视频流识别；

## 运行步骤

1. 打包 Xcode工程；
2. 手动拖拽 ``*.mlmodel`` 文件到 Xcode工程根目录，Copy if needed；
3. ``Build Settings/Enable Objective-C Exceptions``改为YES; //用于支持OC中的``try..catch...``
4. 在真机上运行;

## 开发规划

- [x] Unity中拍照，并对内容识别。
- [x] 优化oc回调。关于UnitySendMessage方法，NSString, const char*数据类型，等。
- [x] 获取ARKit的pixelbuffer，实时传给本插件的oc层 CVPixelBufferRef。
- [x] 作为Pack包，依赖ARKit插件，避免session、buffer冲突。
- [x] 识别成功，将结果用TextMesh显示在世界坐标下。
- [x] 运行时，下载后，加载``*.mlmodel``，减小包体。

## 参考资料

- https://github.com/hanleyweng/CoreML-in-ARKit
