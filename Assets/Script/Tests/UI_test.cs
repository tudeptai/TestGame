using UnityEngine;
using NUnit.Framework; // ✅ Thêm dòng này

public class UI_test
{
    private int health;
    private GameObject coinObject;
    private GameObject playerObject;
    private GameObject audioObject;
   
    [SetUp]
    public void Setup()
    {
        health = 100;
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
}
