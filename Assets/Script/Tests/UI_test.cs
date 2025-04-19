using UnityEngine;
using UnityEngine.SceneManagement;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Collections;

public class UI_test
{
    private GameObject uiObject;
    private UI_Sting ui;

    [SetUp]
    public void Setup()
    {
        uiObject = new GameObject();
        ui = uiObject.AddComponent<UI_Sting>();
    }

    [UnityTest]
    public IEnumerator Pause_Should_Stop_Time()
    {
        ui.pause();
        yield return null;
        Assert.AreEqual(0f, Time.timeScale);
        Assert.IsTrue(UI_Sting.GameIsPaused);
    }

    [UnityTest]
    public IEnumerator Resume_Should_Resume_Time()
    {
        ui.resume();
        yield return null;
        Assert.AreEqual(1f, Time.timeScale);
        Assert.IsFalse(UI_Sting.GameIsPaused);
    }

    [UnityTest]
    public IEnumerator ShowOption_Should_Activate_Panel_And_Pause()
    {
        ui.panel = new GameObject();
        ui.panel.SetActive(false);

        ui.htoption();
        yield return null;

        Assert.IsTrue(ui.panel.activeSelf);
        Assert.AreEqual(0f, Time.timeScale);
        Assert.IsTrue(UI_Sting.GameIsPaused);
    }

    [UnityTest]
    public IEnumerator HideOption_Should_Deactivate_Panel_And_Resume()
    {
        ui.panel = new GameObject();
        ui.panel.SetActive(true);

        ui.anoption();
        yield return null;

        Assert.IsFalse(ui.panel.activeSelf);
        Assert.AreEqual(1f, Time.timeScale);
        Assert.IsFalse(UI_Sting.GameIsPaused);
    }

    [UnityTest]
    public IEnumerator Restart_Should_Reset_Time_And_Reload_Scene()
    {
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Đảm bảo đang ở 1 scene cụ thể
        Time.timeScale = 0f;
        UI_Sting.GameIsPaused = true;

        ui.Restart();

        yield return null;

        Assert.AreEqual(1f, Time.timeScale);
        Assert.IsFalse(UI_Sting.GameIsPaused);
        Assert.AreEqual(currentSceneIndex, SceneManager.GetActiveScene().buildIndex);
    }
}
