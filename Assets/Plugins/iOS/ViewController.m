#import "ViewController.h"
#import "GoogLeNetPlaces.h"
#import "UIImage+Utils.h"

@implementation ViewController

NSString * strPath = nil;

- (int) returnInstanceInt {
    
    NSLog(@"识别图像: %@", strPath);
    UIImage *image = [UIImage imageWithContentsOfFile:strPath];
    
    //识别
    NSString *sceneLabel = [self predictImageScene:image];
    NSLog(@"Scene label is: %@", sceneLabel);
    
    UnitySendMessage("Main Camera", "GetResult", sceneLabel);
    
    return 0;
}

NSMutableDictionary *_instanceHolder;
+ (NSMutableDictionary*) instanceHolder {
    if(_instanceHolder == nil){
        _instanceHolder = [[NSMutableDictionary alloc] init];
    }
    return _instanceHolder;
}

+ (NSString*) createInstance {
    NSUUID *myUUID = [NSUUID UUID];
    ViewController *_osHook = [[ViewController alloc] init];
    [[ViewController instanceHolder] setObject:_osHook forKey:[myUUID UUIDString]];
    return [myUUID UUIDString];
}

+ (ViewController*) getInstanceForKey:(NSString*) key{
    return [[ViewController instanceHolder] valueForKey:key];
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

- (void)viewDidLoad {
    //[super viewDidLoad];
    // Do any additional setup after loading the view, typically from a nib.
    //UIImage *image = [UIImage imageNamed:@"new"];
    //NSString *sceneLabel = [self predictImageScene:image];
    //NSLog(@"Scene label is: %@", sceneLabel);
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
        
        //NSInteger size = [image length];
        //NSLog(@"图片大小: %ld", size);
        
        //UIImage *image = [UIImage imageNamed:@"new"];
        //NSString *sceneLabel = [self predictImageScene:image];
        //NSLog(@"Scene label is: %@", sceneLabel);
    }
    
#if defined (__cplusplus)
}
#endif
