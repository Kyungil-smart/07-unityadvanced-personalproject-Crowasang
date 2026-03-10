# 안드로이드 권한 요청

## 권한 요청을 하는 이유는?

- 카메라, 위치, 마이크, 저장 공간 등 민감한 기능에 접근해야 할 때, 사용자가 명시적으로 기능을 사용할 수 있게 허가하도록 하기 위함. 

## 매니페스트(Manifest)

- 앱에서 사용할 권한을 미리 명시해둠.
- [Android Developers : Manifest.permission](https://developer.android.com/reference/android/Manifest.permission)

## 유니티에서 매니페스트 설정

- Project Settings -> Player -> Publishing Settings -> Build -> Custom Main Manifest 체크
  - Assets/Plugins/Android/AndroidManifest.xml 경로로 생성.

```xml
<?xml version="1.0" encoding="utf-8"?>
<manifest
    xmlns:android="http://schemas.android.com/apk/res/android"
    package="com.unity3d.player"
    xmlns:tools="http://schemas.android.com/tools">
    <!-- 이 곳에 요청할 권한 추가 -->
    <uses-permission android:name="android.permission.ACCESS_FINE_LOCATION"/>
    <application>
        <activity android:name="com.unity3d.player.UnityPlayerActivity"
                  android:theme="@style/UnityThemeSelector">
            <intent-filter>
                <action android:name="android.intent.action.MAIN" />
                <category android:name="android.intent.category.LAUNCHER" />
            </intent-filter>
            <meta-data android:name="unityplayer.UnityActivity" android:value="true" />
        </activity>
    </application>
</manifest>
```

## 권한 요청

```csharp
using UnityEngine.Android;
using System;

public static class PermissionHandler
{
    public static void Request(string targetPermission, Action<string> grantedAction)
    {
        // 이미 승인되어 있는 상황
        if (Permission.HasUserAuthorizedPermission(targetPermission))
        {
            grantedAction?.Invoke(targetPermission);
        }
        // 승인되어 있지 않은 상황
        else
        {
            PermissionCallbacks callback = new();

            callback.PermissionGranted += grantedAction;
            
            Permission.RequestUserPermission(targetPermission, callback);
        }
    }
}
```