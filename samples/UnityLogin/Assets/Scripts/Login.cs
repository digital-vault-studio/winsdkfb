using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if ENABLE_WINMD_SUPPORT
using winsdkfb;
using winsdkfb.Graph;
#endif

public delegate void LogResult(string result, string buttonText);

[RequireComponent(typeof(Button))]
public class Login : MonoBehaviour
{
    [SerializeField]
    private Text facebookAccessToken;

    private Button loginButton;
    private Text loginButtonLabel;

    public static LogResult logger;

    private void Awake()
    {
        loginButton = GetComponent<Button>();
        loginButtonLabel = loginButton.GetComponentInChildren<Text>();
        logger = LogLoginResult;
    }

    /// <summary>
    /// This function is called from Unity UI, all is running on main thread, to execute
    /// XAML code, you should invoke it on XAML UI Thread.
    /// </summary>
    public void FacebookLogin()
    {
#if ENABLE_WINMD_SUPPORT
        // Here we invoke XAML code from UI XAML Thread.
        // If you have the following issue: he application called an interface that was marshalled for a different thread
        // means that your running this code from other thread, change 'InvokeOnUIThread' to 'InvokeOnAppThread'.
        UnityEngine.WSA.Application.InvokeOnUIThread(async () =>
        {
            List<string> permissionList = new List<string>
            {
            "email",
            "public_profile"
            };

            FBSession sess = FBSession.ActiveSession;

            if(sess.LoggedIn)
            {
                await sess.LogoutAsync();
                Login.logger.Invoke("", "Login");
            }
            else
            {
                sess.FBAppId = "1025365401567256"; // AppID here
                sess.WinAppId = "s-1-15-2-1638619580-1889396403-1308485934-3551780281-3637021990-998384026-3803739945"; // WinAppId here

                FBPermissions permissions = new FBPermissions(permissionList);
                FBResult result = await sess.LoginAsync(permissions, SessionLoginBehavior.WebAuth);

                if (result.Succeeded)
                {
                    // Login success                
                    Login.logger.Invoke(sess.AccessTokenData.AccessToken, "Logout");
                }
                else
                {
                    // Login failed
                    Login.logger.Invoke(result.ErrorInfo.Code + " " + result.ErrorInfo.ErrorUserMessage, "Login");
                }
            }
        }, false);
#endif
    }

    /// <summary>
    /// A delegate to receive the result
    /// </summary>
    /// <param name="result">Facebook login result text, can be our token or an error string</param>
    /// <param name="buttonText"></param>
    public void LogLoginResult(string result, string buttonText)
    {
        // All Unity calls should be invoked from App Thread, we invoked this delegate
        // from InvokeOnUIThread, so we need execute the text assignation in App Thread.
#if UNITY_WSA
        UnityEngine.WSA.Application.InvokeOnAppThread(() =>
        {
            facebookAccessToken.text = result;
            loginButtonLabel.text = buttonText;
        }, false);
#endif
    }
}
