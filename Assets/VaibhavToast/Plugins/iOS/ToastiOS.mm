#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

extern "C" {
    void show_message(const char *message)
    {
        if (!message) {
            return;
        }

        NSString *msg = [NSString stringWithUTF8String:message];

        dispatch_async(dispatch_get_main_queue(), ^
		{
            UIWindow *keyWindow = [UIApplication sharedApplication].keyWindow;
            if (!keyWindow) 
			{
                keyWindow = [UIApplication sharedApplication].windows.firstObject;
            }
            UIViewController *rootVC = keyWindow.rootViewController;
            if (!rootVC) 
			{
                return;
            }
            UIAlertController *alert = [UIAlertController
                alertControllerWithTitle:nil
                                 message:msg
                          preferredStyle:UIAlertControllerStyleAlert];

            [rootVC presentViewController:alert animated:YES completion:nil];

            dispatch_after(dispatch_time(DISPATCH_TIME_NOW, (int64_t)(1.5 * NSEC_PER_SEC)), dispatch_get_main_queue(), ^
			{
                [alert dismissViewControllerAnimated:YES completion:nil];
            });
        });
    }
}
