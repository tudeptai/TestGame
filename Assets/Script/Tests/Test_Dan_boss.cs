using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;    
public class Test_Dan_boss
{
    private GameObject bullet;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        bullet = new GameObject("dan_boss");
        bullet.AddComponent<CircleCollider2D>().isTrigger = true;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.AddComponent<dan_boss>();
        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        if (bullet != null)
            Object.Destroy(bullet);
        yield return null;
    }

    [UnityTest]
    public IEnumerator detroy_bullet_time()
    {
        float lifetime = bullet.GetComponent<dan_boss>().lifeTime;
        yield return new WaitForSeconds(lifetime + 0.1f); // Wait a bit more to ensure destroy
        Assert.IsTrue(bullet == null || bullet.Equals(null)); // Check destroyed
    }

    [UnityTest]
    public IEnumerator detroy_bullet_player()
    {
        // Tạo đối tượng "Trap"
        GameObject trap = new GameObject("Trap");
        trap.tag = "Trap";
        trap.AddComponent<BoxCollider2D>().isTrigger = true;

        // Đặt vị trí cho collision
        bullet.transform.position = Vector3.zero;
        trap.transform.position = Vector3.zero;

        yield return null; // Cho physics update

        // Kích hoạt OnTriggerEnter2D bằng cách di chuyển đạn nhẹ
        bullet.GetComponent<Rigidbody2D>().MovePosition(Vector2.zero);

        yield return new WaitForSeconds(0.1f); // Đợi xử lý trigger

        Assert.IsTrue(bullet == null || bullet.Equals(null));

        Object.Destroy(trap);
    }
}
