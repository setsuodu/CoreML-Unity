//
//  ViewController.m
//  CoreMLDemo
//
//  Created by chenyi on 08/06/2017.
//  Copyright © 2017 chenyi. All rights reserved.
//

#import "ViewController.h"
#import "GoogLeNetPlaces.h"
#import "UIImage+Utils.h"

@interface ViewController ()

@end

@implementation ViewController

- (NSString *)predictImageScene:(UIImage *)image {
    GoogLeNetPlaces *model = [[GoogLeNetPlaces alloc] init];
    NSError *error;
    //必须是224*224的
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

//通过实例给.mm中调用
- (int) GetInstanceInt {
    return 94632165;
}

NSMutableDictionary *_instanceHolder;
+ (NSMutableDictionary*) instanceHolder {
    if(_instanceHolder == nil) {
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

+ (ViewController*) getInstanceForKey:(NSString*) key {
    return [[ViewController instanceHolder] valueForKey:key];
}

@end

#if defined (__cplusplus)
extern "C"
{
#endif
    
    void ImagePathToIOS(char* str);
    
#if defined (__cplusplus)
}
#endif
