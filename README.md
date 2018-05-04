# coreml-unity

集成CoreML.framework，使unity工程具有机器学习图片识别的能力。目前正在尝试阶段，已跑通基础的功能，遇到未知的坑可能影响到项目进展。

## Features

- 静态图片识别。

## Roadmap

- unity中拍照，并对内容识别。
- 优化oc回调。关于UnitySendMessage方法，NSString, const char*数据类型，等。
- 获取arkit的pixelbuffer，实时传给本插件的oc层 CVPixelBufferRef。
- 作为Pack包，依赖ARKit插件，避免session、buffer冲突。
- （热更新？）获取更多的model，使用机器学习训练。
- more...

## 参考
https://www.jianshu.com/p/ed8e76081cad
https://github.com/hanleyweng/CoreML-in-ARKit
