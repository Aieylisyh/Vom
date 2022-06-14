#import <Foundation/Foundation.h>
#import <AudioToolBox/AudioToolBox.h>
@implementation ShortVibrator
extern "C" void playSystemSound (int n)
{
    AudioServicesPlaySystemSound(n);
}
@end