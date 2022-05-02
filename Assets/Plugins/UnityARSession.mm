#import <ARKit/ARKit.h>
#import <Vision/Vision.h>
#include "UnityAppController.h"

typedef struct UnityXRNativeSessionPtr
{
    int version;
    void* session;
} UnityXRNativeSessionPtr;

VNRequest *visionCoreMLRequest; //图像分析请求的抽象类
VNSequenceRequestHandler *sequenceRequestHandler; //处理多个图像

static CVPixelBufferRef s_UnityPixelBuffers;
static NSString* filePath;

@interface UnityARSession : NSObject <ARSessionDelegate>
@end
static UnityARSession * SharedInstance;
@implementation UnityARSession
static ARSession* _session;

- (NSURL*)setCoreMLModel:(NSString*)modelPath {
    NSError *error = nil;
    if(!modelPath){
        NSLog(@"modelPath is nil");
    }
    
    // NSString转NSURL
    NSURL *modelURL = [NSURL fileURLWithPath:modelPath];
    NSURL *coremlc = nil;
    /*
    @try {
        coremlc = [MLModel compileModelAtURL:modelURL error:&error];
        if(!coremlc){
            NSLog(@"error=%@", [error localizedDescription]);
        }
    } @catch (NSException *exception) {
        NSLog(@"%@", exception.reason);
    } @finally {
        
    }*/
    coremlc = [MLModel compileModelAtURL:modelURL error:&error];
    if(!coremlc){
        NSLog(@"error=%@", [error localizedDescription]);
    }
    return coremlc;
}

BOOL isCancelled = NO;
NSURL *compileUrl = nil;
MLModel *model = nil;
- (void)startAction
{
    isCancelled = NO;
    
    if(filePath == nil) {
        NSLog(@"下载失败，无法setCoreMLModel");
        return;
    }
    
    if(compileUrl == nil) {
        compileUrl = [self setCoreMLModel:filePath];
    }
    if(model == nil) {
        model = [MLModel modelWithContentsOfURL:compileUrl error:nil];
    }
    
    [self setupVisionRequests:model];
    [self loopCoreMLUpdate];
}
- (void)stopAction
{
    isCancelled = YES;
}
// 异步处理
- (void)loopCoreMLUpdate
{
    if(isCancelled) {
        return;
    }
    
    //NSLog(@"循环执行");
    dispatch_queue_t queue = dispatch_get_main_queue();
    dispatch_block_t block = ^(){
        // 1. Run Update.
        [self updateCoreML];
        // 2. Loop this function.
        [self loopCoreMLUpdate];
    };
    dispatch_async(queue, block);
}

- (void)updateCoreML
{
    ARFrame *frame = _session.currentFrame;
    CVPixelBufferRef pixelBuffer = frame.capturedImage;
    if(pixelBuffer == nil){
        return;
    }
    if (!sequenceRequestHandler) {
        sequenceRequestHandler = [[VNSequenceRequestHandler alloc]init];
    }
    [sequenceRequestHandler performRequests:@[visionCoreMLRequest] onCVPixelBuffer:pixelBuffer error:NULL];
}
// 初始化
- (void)setupVisionRequests:(MLModel*)model {
    
    //MobileNet *mobilenetModel = [[MobileNet alloc] init];
    //VNCoreMLModel *visionModel = [VNCoreMLModel modelForMLModel:mobilenetModel.model error:nil];
    
    // 从外部读取
    VNCoreMLModel *visionModel = [VNCoreMLModel modelForMLModel:model error:nil];
    
    VNCoreMLRequest *classificationRequest = [[VNCoreMLRequest alloc] initWithModel:visionModel completionHandler:^(VNRequest * _Nonnull request, NSError * _Nullable error) {
        if (error) {
            NSLog(@"Failed:%@",error);
        }
        
        NSArray *observations = request.results;
        if (!observations.count) {
            return NSLog(@"无数据");
        }
        
        VNClassificationObservation *observation = nil;
        for (VNClassificationObservation *ob in observations) {
            if (![ob isKindOfClass:[VNClassificationObservation class]]) {
                continue;
            }
            if (!observations) {
                observation = ob;
                continue;
            }
            if (observation.confidence < ob.confidence) {
                observation = ob;
            }
        }
        
        dispatch_async(dispatch_get_main_queue(), ^{
            NSString * text = [NSString stringWithFormat:@"%@ (%.0f%%)", [[observation.identifier componentsSeparatedByString:@", "] firstObject], observation.confidence * 100];
            //NSLog(@"识别结果：%@", text);
            //UnitySendMessage("Object","ClassName","param");
            const char * log = [text UTF8String];
            UnitySendMessage("Callback", "RecogniseCallback", log);
        });
    }];
    visionCoreMLRequest = classificationRequest;
}
@end

extern "C" struct UnityDepthTextureHandles
{
    void* textureDepth;
    double depthTimestamp;
    int width;
    int height;
};

static double s_DepthTimestamp = 0.0;
static id <MTLTexture> s_CapturedDepthImageTexture = NULL;
static int s_width = 0;
static int s_height = 0;

static id <MTLDevice> _device = NULL;

static CVMetalTextureCacheRef _textureCache;

extern "C"
{
    void CreateSession(UnityXRNativeSessionPtr* nativeSession)
    {
        if(SharedInstance == nil)
            SharedInstance = [[UnityARSession alloc] init];
        _session = (__bridge ARSession*)nativeSession->session; //初始化时取值
    }

    void StartVision()
    {
        if (SharedInstance == nil) {
            NSLog(@"==> session is nil");
            return;
        }
        NSLog(@"==> session is exist");
        [SharedInstance startAction];
    }

    void StopVision()
    {
        NSLog(@"stop vision thread");
        [SharedInstance stopAction];
    }

    void LoadMLModel(char * path)
    {
        NSString * strPath = [NSString stringWithUTF8String:path];
        NSLog(@"OC==>>%@", strPath);
        filePath = strPath;
    }


    ///*
    UnityDepthTextureHandles UnityGetDepthMap(UnityXRNativeSessionPtr* nativeSession)
    {
        if(_device == NULL)
        {
            _device = MTLCreateSystemDefaultDevice();
            CVMetalTextureCacheCreate(NULL, NULL, _device, NULL, &_textureCache);
        }

        ARSession* session = (__bridge ARSession*)nativeSession->session; //没变过，取一次就行了
//        printf("[oc]ARSession: 是否变了？%d, %d\n, %d\n", session == lastSession, 1==1, 1==2);
//        lastSession = session;
        
        ARFrame* frame = session.currentFrame; //每帧在改变
        printf("[oc]ARFrame: 是否变了？%d\n", frame == NULL);
        
        UnityDepthTextureHandles handles;
        return handles;
    }

    void ReleaseDepthTextureHandles(UnityDepthTextureHandles handles)
    {
        if (handles.textureDepth != NULL)
        {
            CFRelease(handles.textureDepth);
        }
    }

    void UnityUnloadMetalCache()
    {
        if (_textureCache != NULL) {
            CFRelease(_textureCache);
            _textureCache = NULL;
        }
        _device = NULL;
    }
    //*/
}
