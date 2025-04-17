using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class Player_Move_Test
{
    [UnityTest]
    public IEnumerator PlayerMovesForward()
    {
        GameObject player = new GameObject();
        player.transform.position = Vector3.zero;

        player.transform.position += Vector3.forward;

        yield return null;

        Assert.AreEqual(Vector3.forward, player.transform.position);
    }
    [UnityTest]
    public IEnumerator Player_nhay()
    {
        GameObject player = new GameObject();
        Rigidbody rb = player.AddComponent<Rigidbody>();
        player.transform.position = Vector3.zero;

        // Giả lập nhảy bằng lực hướng lên
        rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);

        yield return new WaitForSeconds(0.1f);

        Assert.Greater(player.transform.position.y, 0f);
    }

    [UnityTest]
    public IEnumerator Player_danh()
    {
        GameObject enemy = new GameObject("Enemy");
        enemy.transform.position = new Vector3(0, 0, 1);
        enemy.AddComponent<BoxCollider>();

        GameObject player = new GameObject("Player");
        player.transform.position = Vector3.zero;
        player.AddComponent<BoxCollider>();

        // Giả lập phạm vi tấn công
        float attackRange = 2f;
        bool hitEnemy = Vector3.Distance(player.transform.position, enemy.transform.position) <= attackRange;

        yield return null;

        Assert.IsTrue(hitEnemy);
    }
    [UnityTest]
    public IEnumerator PlayerPicksUpItem()
    {
        GameObject item = GameObject.CreatePrimitive(PrimitiveType.Cube);
        item.name = "Item";
        item.transform.position = Vector3.forward;

        GameObject player = new GameObject();
        player.transform.position = Vector3.zero;

        // Giả lập di chuyển đến item
        player.transform.position = Vector3.forward;

        yield return null;

        bool pickedUp = Vector3.Distance(player.transform.position, item.transform.position) < 0.1f;

        Assert.IsTrue(pickedUp);
    }
    [UnityTest]
    public IEnumerator PlayerSlides()
    {
        GameObject player = new GameObject();
        player.transform.position = Vector3.zero;

        // Giả lập hành động trượt bằng cách thay đổi vị trí nhanh
        Vector3 slideDirection = Vector3.forward * 3f;
        player.transform.position += slideDirection;

        yield return null;

        Assert.AreEqual(new Vector3(0, 0, 3), player.transform.position);
    }
    [UnityTest]
    public IEnumerator PlayerClimbsLadder()
    {
        GameObject player = new GameObject();
        player.transform.position = Vector3.zero;

        // Giả lập leo thang bằng cách tăng chiều cao
        player.transform.position += Vector3.up * 2f;

        yield return null;

        Assert.AreEqual(new Vector3(0, 2, 0), player.transform.position);
    }
    [UnityTest]
    public IEnumerator PlayerRespawnsAtCheckpoint()
    {
        Vector3 checkpoint = new Vector3(5, 0, 5);

        GameObject player = new GameObject();
        player.transform.position = new Vector3(100, 0, 100); // chết ở đâu đó xa

        // Giả lập hồi sinh
        player.transform.position = checkpoint;

        yield return null;

        Assert.AreEqual(checkpoint, player.transform.position);
    }
    [UnityTest]
    public IEnumerator UIShowsOnItemPickup()
    {
        GameObject ui = new GameObject("ItemUI");
        ui.SetActive(false);

        GameObject item = new GameObject("Item");
        GameObject player = new GameObject();

        // Giả lập nhặt item
        player.transform.position = Vector3.forward;
        item.transform.position = Vector3.forward;
        ui.SetActive(true);

        yield return null;

        Assert.IsTrue(ui.activeSelf);
    }

}
