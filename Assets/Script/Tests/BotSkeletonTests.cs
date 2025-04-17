using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BotSkeletonTests
{
    GameObject botObject;
    BOT_SKELETON botScript;
    GameObject playerObject;
    // ---------- TEST CHO BOT_FLYEYE ----------
    GameObject flyeyeBot;
    BOT_FLYEYE flyeyeScript;
    // ---------- TEST CHO BOT_MUSHROOM ----------
    GameObject mushroomBot;
    BOT_MUSHROOM mushroomScript;
    [SetUp]
    public void Setup()
    {
        // Tạo bot
        botObject = new GameObject("Bot");
        botObject.transform.position = Vector3.zero;
        botScript = botObject.AddComponent<BOT_SKELETON>();
        botScript.animator = botObject.AddComponent<Animator>();
        botScript.attackSound = botObject.AddComponent<AudioSource>();
        botObject.AddComponent<Rigidbody2D>();

        // Tạo player
        playerObject = new GameObject("Player");
        playerObject.transform.position = new Vector3(0.5f, 0f, 0f);
        botScript.GetType().GetField("findPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(botScript, playerObject);
    }
    [UnityTest]
    public IEnumerator Mushroom_Chases_Player_When_Close()
    {
        mushroomBot = new GameObject("MushroomBot");
        mushroomScript = mushroomBot.AddComponent<BOT_MUSHROOM>();
        mushroomBot.AddComponent<Rigidbody2D>();

        GameObject mushroomPlayer = new GameObject("PlayerForMushroom");
        mushroomPlayer.transform.position = new Vector3(1f, 0f, 0f);

        mushroomScript.GetType()
            .GetField("findPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(mushroomScript, mushroomPlayer);

        mushroomScript.attackRange = 5f;

        Vector3 oldPos = mushroomBot.transform.position;
        yield return new WaitForSeconds(0.2f);
        Vector3 newPos = mushroomBot.transform.position;

        Assert.AreNotEqual(oldPos.x, newPos.x, "Mushroom bot phải di chuyển về phía player.");

        Object.Destroy(mushroomBot);
        Object.Destroy(mushroomPlayer);
    }

    [UnityTest]
    public IEnumerator Mushroom_Stops_When_Player_Too_Far()
    {
        mushroomBot = new GameObject("MushroomBot");
        mushroomScript = mushroomBot.AddComponent<BOT_MUSHROOM>();
        mushroomBot.AddComponent<Rigidbody2D>();

        GameObject mushroomPlayer = new GameObject("PlayerForMushroom");
        mushroomPlayer.transform.position = new Vector3(10f, 0f, 0f);

        mushroomScript.GetType()
            .GetField("findPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(mushroomScript, mushroomPlayer);

        mushroomScript.attackRange = 1f;

        Vector3 oldPos = mushroomBot.transform.position;
        yield return new WaitForSeconds(0.2f);
        Vector3 newPos = mushroomBot.transform.position;

        Assert.AreEqual(oldPos.x, newPos.x, 0.01f, "Mushroom bot không được di chuyển khi player ở xa.");

        Object.Destroy(mushroomBot);
        Object.Destroy(mushroomPlayer);
    }

    [UnityTest]
    public IEnumerator Mushroom_Faces_Correct_Direction_When_Chasing()
    {
        mushroomBot = new GameObject("MushroomBot");
        mushroomScript = mushroomBot.AddComponent<BOT_MUSHROOM>();
        mushroomBot.AddComponent<Rigidbody2D>();

        GameObject mushroomPlayer = new GameObject("PlayerForMushroom");
        mushroomPlayer.transform.position = new Vector3(-2f, 0f, 0f); // player bên trái bot

        mushroomScript.GetType()
            .GetField("findPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(mushroomScript, mushroomPlayer);

        mushroomScript.attackRange = 10f;

        yield return new WaitForSeconds(0.2f);

        Assert.AreEqual(-1f, mushroomBot.transform.localScale.x, "Mushroom bot phải quay về bên trái khi player ở trái.");

        Object.Destroy(mushroomBot);
        Object.Destroy(mushroomPlayer);
    }
    [UnityTest]
    public IEnumerator Bot_Attacks_When_Player_Is_Close()
    {
        // Set phạm vi để đảm bảo bot tấn công
        botScript.attackRange = 1f;
        botScript.raycastDistance = 1f;

        yield return null; // Chờ 1 frame cho Update chạy

        // Kiểm tra bot có đang tấn công không
        Assert.IsTrue(GetPrivateBool(botScript, "isAttacking"));
    }

    [UnityTest]
    public IEnumerator Bot_DoesNotAttack_When_Player_Is_TooFar()
    {
        // Di chuyển player ra xa
        playerObject.transform.position = new Vector3(10f, 0f, 0f);

        yield return null;

        // Kiểm tra bot không tấn công
        Assert.IsFalse(GetPrivateBool(botScript, "isAttacking"));
    }

    [UnityTest]
    public IEnumerator Bot_Move_Towards_Player()
    {
        botScript.attackRange = 5f;

        Vector3 startPos = botObject.transform.position;
        yield return new WaitForSeconds(0.1f); // Cho bot có thời gian chạy

        // Bot phải bắt đầu di chuyển
        Assert.AreNotEqual(startPos, botObject.transform.position);
    }

    [UnityTest]
    public IEnumerator Flyeye_Chases_Player_When_Close()
    {
        flyeyeBot = new GameObject("FlyeyeBot");
        flyeyeScript = flyeyeBot.AddComponent<BOT_FLYEYE>();
        flyeyeBot.AddComponent<Rigidbody2D>();

        // Tạo player
        GameObject flyeyePlayer = new GameObject("PlayerForFlyeye");
        flyeyePlayer.transform.position = new Vector3(1f, 0f, 0f);

        // Gán player cho BOT_FLYEYE (dùng reflection vì findPlayer là private)
        flyeyeScript.GetType()
            .GetField("findPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(flyeyeScript, flyeyePlayer);

        flyeyeScript.attackRange = 5f;

        Vector3 oldPos = flyeyeBot.transform.position;
        yield return new WaitForSeconds(0.2f);
        Vector3 newPos = flyeyeBot.transform.position;

        Assert.AreNotEqual(oldPos.x, newPos.x, "Flyeye bot phải di chuyển về phía player.");

        Object.Destroy(flyeyeBot);
        Object.Destroy(flyeyePlayer);
    }

    [UnityTest]
    public IEnumerator Flyeye_Stops_When_Player_Too_Far()
    {
        flyeyeBot = new GameObject("FlyeyeBot");
        flyeyeScript = flyeyeBot.AddComponent<BOT_FLYEYE>();
        flyeyeBot.AddComponent<Rigidbody2D>();

        GameObject flyeyePlayer = new GameObject("PlayerForFlyeye");
        flyeyePlayer.transform.position = new Vector3(10f, 0f, 0f); // Rất xa

        flyeyeScript.GetType()
            .GetField("findPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(flyeyeScript, flyeyePlayer);

        flyeyeScript.attackRange = 1f;

        Vector3 oldPos = flyeyeBot.transform.position;
        yield return new WaitForSeconds(0.2f);
        Vector3 newPos = flyeyeBot.transform.position;

        Assert.AreEqual(oldPos.x, newPos.x, 0.01f, "Flyeye bot không được di chuyển khi player ở xa.");

        Object.Destroy(flyeyeBot);
        Object.Destroy(flyeyePlayer);
    }

    [UnityTest]
    public IEnumerator Flyeye_Faces_Correct_Direction_When_Chasing()
    {
        flyeyeBot = new GameObject("FlyeyeBot");
        flyeyeScript = flyeyeBot.AddComponent<BOT_FLYEYE>();
        flyeyeBot.AddComponent<Rigidbody2D>();

        GameObject flyeyePlayer = new GameObject("PlayerForFlyeye");
        flyeyePlayer.transform.position = new Vector3(-2f, 0f, 0f); // Bên trái bot

        flyeyeScript.GetType()
            .GetField("findPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(flyeyeScript, flyeyePlayer);

        flyeyeScript.attackRange = 10f;

        yield return new WaitForSeconds(0.2f);

        Assert.AreEqual(-1f, flyeyeBot.transform.localScale.x, "Flyeye bot phải quay về bên trái khi player ở trái.");

        Object.Destroy(flyeyeBot);
        Object.Destroy(flyeyePlayer);
    }
    private bool GetPrivateBool(BOT_SKELETON bot, string fieldName)
    {
        var field = typeof(BOT_SKELETON).GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (bool)field.GetValue(bot);
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(botObject);
        Object.Destroy(playerObject);
    }
}
