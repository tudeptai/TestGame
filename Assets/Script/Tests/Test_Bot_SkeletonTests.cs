using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Test_Enemy
{
    GameObject botObject;
    BOT_SKELETON botScript;
    GameObject playerObject;
    //TEST CHO BOT_FLYEYE 
    GameObject flyeyeBot;
    BOT_FLYEYE flyeyeScript;
    // TEST CHO BOT_MUSHROOM
    GameObject mushroomBot;
    BOT_MUSHROOM mushroomScript;
    [SetUp]
    public void Setup()
    {
        // Tạo bot chính (BOT_SKELETON)
        botObject = new GameObject("Bot");
        botObject.transform.position = Vector3.zero;
        botScript = botObject.AddComponent<BOT_SKELETON>();
        botScript.animator = botObject.AddComponent<Animator>();
        botScript.attackSound = botObject.AddComponent<AudioSource>();
        botObject.AddComponent<Rigidbody2D>();

        // Tạo player cho bot tương tác
        playerObject = new GameObject("Player");
        playerObject.transform.position = new Vector3(0.5f, 0f, 0f);

        // Gán player cho trường private 'findPlayer' của bot bằng reflection
        botScript.GetType().GetField("findPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(botScript, playerObject);
    }

    // ---------- BOT_MUSHROOM TEST ----------

    [UnityTest]
    public IEnumerator Mushroom_Chases_Playe()
    {
        // Kiểm tra BOT_MUSHROOM có di chuyển khi player ở gần

        mushroomBot = new GameObject("MushroomBot");
        mushroomScript = mushroomBot.AddComponent<BOT_MUSHROOM>();
        mushroomBot.AddComponent<Rigidbody2D>();

        GameObject mushroomPlayer = new GameObject("PlayerForMushroom");
        mushroomPlayer.transform.position = new Vector3(1f, 0f, 0f); // Gần bot

        // Gán player
        mushroomScript.GetType().GetField("findPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
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
    public IEnumerator Mushroom_Stops_Player()
    {
        // Kiểm tra bot đứng yên khi player ở quá xa

        mushroomBot = new GameObject("MushroomBot");
        mushroomScript = mushroomBot.AddComponent<BOT_MUSHROOM>();
        mushroomBot.AddComponent<Rigidbody2D>();

        GameObject mushroomPlayer = new GameObject("PlayerForMushroom");
        mushroomPlayer.transform.position = new Vector3(10f, 0f, 0f); // Rất xa

        mushroomScript.GetType().GetField("findPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
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
    public IEnumerator Mushroom_Huongmat_Player()
    {
        // Kiểm tra Mushroom quay đúng hướng về phía player

        mushroomBot = new GameObject("MushroomBot");
        mushroomScript = mushroomBot.AddComponent<BOT_MUSHROOM>();
        mushroomBot.AddComponent<Rigidbody2D>();

        GameObject mushroomPlayer = new GameObject("PlayerForMushroom");
        mushroomPlayer.transform.position = new Vector3(-2f, 0f, 0f); // Player bên trái bot

        mushroomScript.GetType().GetField("findPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(mushroomScript, mushroomPlayer);

        mushroomScript.attackRange = 10f;

        yield return new WaitForSeconds(0.2f);

        Assert.AreEqual(-1f, mushroomBot.transform.localScale.x, "Mushroom bot phải quay về bên trái khi player ở trái.");

        Object.Destroy(mushroomBot);
        Object.Destroy(mushroomPlayer);
    }

    // ---------- BOT_SKELETON TEST ----------

    [UnityTest]
    public IEnumerator SKELETON_attack_Player()
    {
        // Kiểm tra bot tấn công khi player ở gần

        botScript.attackRange = 1f;
        botScript.raycastDistance = 1f;

        yield return null; // Chờ 1 frame

        Assert.IsTrue(GetPrivateBool(botScript, "isAttacking"));
    }

    [UnityTest]
    public IEnumerator SKELETON_NO_attack_Player()
    {
        // Kiểm tra bot KHÔNG tấn công khi player quá xa

        playerObject.transform.position = new Vector3(10f, 0f, 0f); // Rất xa

        yield return null;

        Assert.IsFalse(GetPrivateBool(botScript, "isAttacking"));
    }

    [UnityTest]
    public IEnumerator SKELETON_Move_player()
    {
        // Kiểm tra bot có di chuyển về phía player

        botScript.attackRange = 5f;

        Vector3 startPos = botObject.transform.position;
        yield return new WaitForSeconds(0.1f);

        Assert.AreNotEqual(startPos, botObject.transform.position);
    }

    // ---------- BOT_FLYEYE TEST ----------

    [UnityTest]
    public IEnumerator FLYEYE_move_Player()
    {
        // Kiểm tra Flyeye di chuyển về phía player

        flyeyeBot = new GameObject("FlyeyeBot");
        flyeyeScript = flyeyeBot.AddComponent<BOT_FLYEYE>();
        flyeyeBot.AddComponent<Rigidbody2D>();

        GameObject flyeyePlayer = new GameObject("PlayerForFlyeye");
        flyeyePlayer.transform.position = new Vector3(1f, 0f, 0f); // Gần bot

        flyeyeScript.GetType().GetField("findPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
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
    public IEnumerator FLYEYE_NO_move_Player()
    {
        // Kiểm tra Flyeye KHÔNG di chuyển khi player quá xa

        flyeyeBot = new GameObject("FlyeyeBot");
        flyeyeScript = flyeyeBot.AddComponent<BOT_FLYEYE>();
        flyeyeBot.AddComponent<Rigidbody2D>();

        GameObject flyeyePlayer = new GameObject("PlayerForFlyeye");
        flyeyePlayer.transform.position = new Vector3(10f, 0f, 0f); // Rất xa

        flyeyeScript.GetType().GetField("findPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
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
    public IEnumerator FLYEYE_HuongMat_player()
    {
        // Kiểm tra Flyeye quay đúng hướng về phía player

        flyeyeBot = new GameObject("FlyeyeBot");
        flyeyeScript = flyeyeBot.AddComponent<BOT_FLYEYE>();
        flyeyeBot.AddComponent<Rigidbody2D>();

        GameObject flyeyePlayer = new GameObject("PlayerForFlyeye");
        flyeyePlayer.transform.position = new Vector3(-2f, 0f, 0f); // Bên trái bot

        flyeyeScript.GetType().GetField("findPlayer", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(flyeyeScript, flyeyePlayer);

        flyeyeScript.attackRange = 10f;

        yield return new WaitForSeconds(0.2f);

        Assert.AreEqual(-1f, flyeyeBot.transform.localScale.x, "Flyeye bot phải quay về bên trái khi player ở trái.");

        Object.Destroy(flyeyeBot);
        Object.Destroy(flyeyePlayer);
    }

    // Hàm hỗ trợ lấy biến private kiểu bool
    private bool GetPrivateBool(BOT_SKELETON bot, string fieldName)
    {
        var field = typeof(BOT_SKELETON).GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (bool)field.GetValue(bot);
    }

    // Hủy các đối tượng sau mỗi bài test
    [TearDown]
    public void Teardown()
    {
        Object.Destroy(botObject);
        Object.Destroy(playerObject);
    }
}
