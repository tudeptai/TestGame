//using NUnit.Framework;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using UnityEngine.TestTools;
//using System.Collections;

//public class Test_LoginPlayFab
//{
//    private GameObject loginObj;
//    private LoginPlayfabs loginScript;

//    [SetUp]
//    public void SetUp()
//    {
//        loginObj = new GameObject();
//        loginScript = loginObj.AddComponent<LoginPlayfabs>();

//        // Gán các UI c?n thi?t cho test
//        loginScript.TopText = new GameObject().AddComponent<TextMeshProUGUI>();
//        loginScript.MessageText = new GameObject().AddComponent<TextMeshProUGUI>();
//        loginScript.EmailLoginInput = new GameObject().AddComponent<TMP_InputField>();
//        loginScript.PasswordLoginInput = new GameObject().AddComponent<TMP_InputField>();
//        loginScript.EmailRecoveryInput = new GameObject().AddComponent<TMP_InputField>();
//        loginScript.EmailRegisterInput = new GameObject().AddComponent<TMP_InputField>();
//        loginScript.UserNameRegisterInput = new GameObject().AddComponent<TMP_InputField>();
//        loginScript.PassworRegisterInput = new GameObject().AddComponent<TMP_InputField>();

//        loginScript.LoginPage = new GameObject();
//        loginScript.ResgisterPage = new GameObject();
//        loginScript.RecoveryPage = new GameObject();
//    }

//    [Test]
//    public void Test_PasswordTooShort()
//    {
//        loginScript.PassworRegisterInput.text = "123"; // quá ng?n
//        loginScript.RegisterUser();

//        Assert.AreEqual("Password is too short", loginScript.MessageText.text);
//    }

//    [UnityTest]
//    public IEnumerator Test_OpenLoginPage()
//    {
//        loginScript.OpenLoginPage();

//        yield return null;

//        Assert.IsTrue(loginScript.LoginPage.activeSelf);
//        Assert.IsFalse(loginScript.ResgisterPage.activeSelf);
//        Assert.IsFalse(loginScript.RecoveryPage.activeSelf);
//        Assert.AreEqual("Login", loginScript.TopText.text);
//    }

//    [UnityTest]
//    public IEnumerator Test_OpenRecoveryPage()
//    {
//        loginScript.OpenRecoveryPage();

//        yield return null;

//        Assert.IsTrue(loginScript.RecoveryPage.activeSelf);
//        Assert.AreEqual("Recovery", loginScript.TopText.text);
//    }
//}
