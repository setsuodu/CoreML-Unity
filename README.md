# CoreML-Unity

[![GitHub](https://img.shields.io/github/license/mashape/apistatus.svg?style=social)](https://github.com/brakmic/OpenCV/blob/master/LICENSE)

## 概览

集成 CoreML & Vision，使Unity具有机器学习图片识别的能力。
旧版本已经归档，可以在Tag中找到。后续版本如果更新，会根据ARKit官方的版本号命名Tag。

## 开发环境

- [Unity 2021.3.1f1c1](https://unity.cn/releases/lts/2021)；
- [Xcode 12.5.1](https://developer.apple.com/download/all/?q=xcode)；
- UnityARKitPlugin 5.0.0-pre.8；//由PackageManger管理版本
- [MobileNetV2.mlmodel](https://developer.apple.com/machine-learning/build-run-models/);

## 运行步骤

1. 打包 Xcode工程；
2. 手动拖拽 ``*.mlmodel`` 文件到 Xcode工程根目录，✅Copy if needed；✅UnityFramework；
3. 在真机上运行;

## 功能特性

- 1. 支持Unity中选择图片，对内容识别。
- 2. 支持在oc层调用CVPixelBufferRef，多线程执行识别，避免丢帧卡顿。
- 3. 支持在运行时下载和加载``*.mlmodel``，减小包体。

## 参考资料

- https://github.com/hanleyweng/CoreML-in-ARKit
- https://github.com/Unity-Technologies/arfoundation-samples/issues/615
