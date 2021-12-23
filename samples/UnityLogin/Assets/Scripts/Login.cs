using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if ENABLE_WINMD_SUPPORT
using winsdkfb;
using winsdkfb.Graph;
#endif

public delegate void LogResult(string result);

public class Login : MonoBehaviour
{
    [SerializeField]
    private Text facebookAccessToken;

    public static LogResult logger;

    private void Awake()
    {
        logger = LogLoginResult;
    }

    public void FacebookLogin()
    {
#if ENABLE_WINMD_SUPPORT
        UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
        {
            List<string> permissionList = new List<string>
            {
            "email",
            "public_profile"
            };

            FBSession sess = FBSession.ActiveSession;
            sess.FBAppId = "1025365401567256"; // AppID here
            sess.WinAppId = "s-1-15-2-1638619580-1889396403-1308485934-3551780281-3637021990-998384026-3803739945"; // WinAppId here


            FBPermissions permissions = new FBPermissions(permissionList);
            FBResult result = await sess.LoginAsync(permissions, SessionLoginBehavior.WebAuth);

            if (result.Succeeded)
            {
                // Login success                
                Login.logger.Invoke(sess.AccessTokenData.AccessToken);
            }
            else
            {
                // Login failed
                Login.logger.Invoke(result.ErrorInfo.Code + " " + result.ErrorInfo.ErrorUserMessage);
            }
        }, false);
#endif
    }

    public void LogLoginResult(string result)
    {
#if ENABLE_WINMD_SUPPORT
        UnityEngine.WSA.Application.InvokeOnAppThread(() =>
        {
            facebookAccessToken.text = result;
        }, false);
#endif
    }
}
