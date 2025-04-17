using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Trap_Test
{
    private GameObject trapObject;
    private MovingTrap movingTrap;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Tạo GameObject và thêm MovingTrap
        trapObject = new GameObject("Trap");
        movingTrap = trapObject.AddComponent<MovingTrap>();

        // Cấu hình các thông số để test nhanh
        movingTrap.moveSpeed = 5f;
        movingTrap.moveDistance = 2f;

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        GameObject.Destroy(trapObject);
        yield return null;
    }

    [UnityTest]
    public IEnumerator TrapMovesRightInitially()
    {
        Vector3 startPos = trapObject.transform.position;

        // Chờ 0.5 giây để trap bắt đầu di chuyển
        yield return new WaitForSeconds(0.5f);

        // Sau 0.5 giây, trap nên đã di chuyển sang phải
        Vector3 currentPos = trapObject.transform.position;
        Debug.Log($"Start: {startPos.x}, After: {currentPos.x}");

        Assert.Greater(currentPos.x, startPos.x, "Trap không di chuyển sang phải như mong đợi.");
    }

    [UnityTest]
    public IEnumerator TrapReversesDirectionAtEdge()
    {
        // Đặt lại vị trí ban đầu
        trapObject.transform.position = Vector3.zero;

        // Chờ đủ thời gian để nó đi đến rìa phải và bắt đầu quay đầu
        float duration = (movingTrap.moveDistance * 2) / movingTrap.moveSpeed + 0.5f;
        yield return new WaitForSeconds(duration);

        Vector3 currentPos = trapObject.transform.position;
        Debug.Log($"Position after full cycle: {currentPos.x}");

        // Nếu sau khi đi một vòng, trap quay về gần vị trí ban đầu thì test pass
        Assert.LessOrEqual(Mathf.Abs(currentPos.x), movingTrap.moveDistance, "Trap không quay đầu đúng giới hạn.");
    }
}
