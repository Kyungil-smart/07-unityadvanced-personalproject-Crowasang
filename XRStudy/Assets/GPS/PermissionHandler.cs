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
