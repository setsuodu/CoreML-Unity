#import "ViewController.h"
#import "GoogLeNetPlaces.h"
#import "UIImage+Utils.h"

static ViewController * SharedInstance;

@implementation ViewController

+ (ViewController *)sharedInstance {
    if (SharedInstance == nil)
        SharedInstance = [[ViewController alloc] init];
    return SharedInstance;
}

NSString * strPath = nil;

// 执行识别
- (void) startML {
    
    NSLog(@"识别图像: %@", strPath);
    UIImage *image = [UIImage imageWithContentsOfFile:strPath];
    
    //识别
    NSString *sceneLabel = [self predictImageScene:image];
    NSLog(@"Scene label is: %@", sceneLabel);
    
    UnitySendMessage("Main Camera", "GetResult", [sceneLabel UTF8String]);
}

- (NSString *)predictImageScene:(UIImage *)image {
    GoogLeNetPlaces *model = [[GoogLeNetPlaces alloc] init];
    NSError *error;
    //必须是224*224的图
    UIImage *scaledImage = [image scaleToSize:CGSizeMake(224, 224)];
    CVPixelBufferRef buffer = [image pixelBufferFromCGImage:scaledImage];
    GoogLeNetPlacesInput *input = [[GoogLeNetPlacesInput alloc] initWithSceneImage:buffer];
    GoogLeNetPlacesOutput *output = [model predictionFromFeatures:input error:&error];
    return output.sceneLabel;
}

@end

//extern "C" 放在.m 中必须加 #if...#endif
#if defined (__cplusplus)
extern "C"
{
#endif
    
    void ImagePathToIOS(char* str)
    {
        NSLog(@"Unity传递过来的参数: %s", str);
        
        strPath = [NSString stringWithUTF8String:str];
        // 加载Mainbundle中的图片,推荐使用imageWithContentsOfFile:这个方法来加载图片，至于区别，请自行百度
        UIImage *image = [UIImage imageWithContentsOfFile:strPath];
        
        CGFloat fixelW = CGImageGetWidth(image.CGImage);
        CGFloat fixelH = CGImageGetHeight(image.CGImage);
        NSLog(@"图片大小: %f * %f", fixelW, fixelH);
    }
    
    void StartCoreML()
    {
        [ViewController.sharedInstance startML];
    }
    
#if defined (__cplusplus)
}
#endif
