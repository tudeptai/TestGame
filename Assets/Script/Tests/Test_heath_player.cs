using UnityEngine;
using NUnit.Framework; // ✅ Thêm dòng này

public class Test_heath_player
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
    public void Health_Damage_Player()
    {
        health -= 20;
        Assert.AreEqual(80, health);
    }

    [Test]
    public void Health_NO_AM_player
        ()
    {
        health -= 150;
        if (health < 0) health = 0;
        Assert.GreaterOrEqual(health, 0);
    }

    [Test]
    public void Health_Max_Start_Player()
    {
        Assert.AreEqual(100, health);
    }
}
