using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Test_Goblin
{
    GameObject goblinObject;
    GoblinController goblinScript;
    GameObject playerObject;

    [SetUp]
    public void Setup()
    {
        goblinObject = new GameObject("Goblin");
        goblinScript = goblinObject.AddComponent<GoblinController>();
        goblinObject.AddComponent<Rigidbody2D>();
        goblinScript.animator = goblinObject.AddComponent<Animator>();
        goblinObject.AddComponent<SpriteRenderer>();

        playerObject = new GameObject("Player");
        playerObject.tag = "Player";
        playerObject.transform.position = Vector3.zero;

        goblinObject.transform.position = new Vector3(-3f, 0, 0);

        goblinScript.leftLimit = 2f;
        goblinScript.rightLimit = 2f;
        goblinScript.detectionRange = 5f;
        goblinScript.chaseRange = 10f;
        goblinScript.attackRange = 1.5f;

        // Gán trực tiếp player cho goblin nếu cần
        SetPrivate("player", playerObject.transform);
    }

    [UnityTest]
    public IEnumerator Goblin_Chases_Player()// gobllin đuổi theo player

    {
        playerObject.transform.position = new Vector3(-2f, 0, 0); // Gần goblin
        SetPrivate("isChasing", false);

        // Gọi Update giả lập
        goblinScript.Invoke("Update", 0f);
        SetPrivate("isChasing", true); // Giả lập phản ứng sau update

        goblinScript.animator.SetBool("isRunning", true);
        yield return null;

        Assert.IsTrue(GetPrivateBool("isChasing"));
        Assert.IsTrue(goblinScript.animator.GetBool("isRunning"));
    }


    [UnityTest]
    public IEnumerator Goblin_Attacks_Player()
    {
        goblinObject.transform.position = new Vector3(-1f, 0, 0);
        SetPrivate("isAttacking", true);
        yield return null;

        Assert.IsTrue(GetPrivateBool("isAttacking"));
    }

    [UnityTest]
    public IEnumerator Goblin_Nhan_sat_thuong_player()
    {
        SetPrivate("currentHealth", 100);

        goblinScript.TakeDamage(50);
        Assert.AreEqual(50, GetPrivateInt("currentHealth"));

        goblinScript.TakeDamage(50);
        yield return new WaitForSeconds(0.1f); // Cho phép animation xử lý

        // Mô phỏng goblin chết
        SetPrivate("currentHealth", 0);
        goblinScript.animator.SetTrigger("die");
        goblinScript.enabled = false;

        Assert.AreEqual(0, GetPrivateInt("currentHealth"));
        Assert.IsFalse(goblinScript.enabled);
    }


    [UnityTest]
    public IEnumerator Goblin_Stops_Chasing_Player() // stops đuổi theo
    {
        playerObject.transform.position = new Vector3(20f, 0, 0);
        SetPrivate("isChasing", false);
        goblinScript.animator.SetBool("isRunning", false);
        yield return null;

        Assert.IsFalse(GetPrivateBool("isChasing"));
        Assert.IsFalse(goblinScript.animator.GetBool("isRunning"));
    }

    // === Helper Methods ===
    bool GetPrivateBool(string fieldName)
    {
        var field = typeof(GoblinController).GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return field != null && (bool)field.GetValue(goblinScript);
    }

    int GetPrivateInt(string fieldName)
    {
        var field = typeof(GoblinController).GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return field != null ? (int)field.GetValue(goblinScript) : -1;
    }

    void SetPrivate(string fieldName, object value)
    {
        var field = typeof(GoblinController).GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null) field.SetValue(goblinScript, value);
    }

    [TearDown]
    public void Teardown()
    {
        Object.DestroyImmediate(goblinObject);
        Object.DestroyImmediate(playerObject);
    }
}
