using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UI_test
{
    private int health;
    private GameObject coinObject;
    private GameObject playerObject;
    private GameObject audioObject;
    private GameObject gameManagerObject;
    private UI_Sting uiSting;


    [SetUp]
    public void Setup()
    {
        health = 100;

        // Khởi tạo đối tượng GameManager và UI_Sting
        gameManagerObject = new GameObject();
        uiSting = gameManagerObject.AddComponent<UI_Sting>();
    }

    [Test]
    public void Health_Should_Decrease_When_Taking_Damage()
    {
        health -= 20;
        Assert.AreEqual(80, health);
    }

    [Test]
    public void Health_Should_Not_Be_Negative()
    {
        health -= 150;
        if (health < 0) health = 0;
        Assert.GreaterOrEqual(health, 0);
    }

    [Test]
    public void Health_Should_Be_Max_At_Start()
    {
        Assert.AreEqual(100, health);
    }
    [Test]
    public void Scene_Should_Load_Correctly_When_Start_Is_Pressed()
    {
        // Lưu lại tên của cảnh hiện tại
        string currentScene = SceneManager.GetActiveScene().name;

        // Gọi phương thức NewGame() (tương tự như nhấn nút Start)
        uiSting.NewGame();

        // Kiểm tra xem cảnh đã thay đổi hay chưa
        Assert.AreNotEqual(currentScene, SceneManager.GetActiveScene().name);  // Cảnh phải thay đổi
    }

}
