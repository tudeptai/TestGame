using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class Test_CameraFollow
{
    private GameObject cameraObj;
    private CameraFollow cameraFollow;
    private GameObject playerObj;

    [SetUp]
    public void SetUp()
    {
        // Tạo camera
        cameraObj = new GameObject("Camera");
        cameraFollow = cameraObj.AddComponent<CameraFollow>();

        // Tạo player
        playerObj = new GameObject("Player");
        playerObj.transform.position = Vector3.zero;

        // Gán target cho cameraFollow
        cameraFollow.target = playerObj.transform;
        cameraFollow.smoothSpeed = 5f;
        cameraFollow.xOffset = 2f;

        // Đặt camera tại vị trí ban đầu
        cameraObj.transform.position = new Vector3(-10f, 5f, -10f);
    }

    [UnityTest] // camera di chuyển theo vị trí player
    public IEnumerator Test_CameraMoves_Player()
    {
        // Di chuyển player sang phải
        playerObj.transform.position = new Vector3(5f, 0f, 0f);

        float initialCameraX = cameraObj.transform.position.x;

        // Chờ một frame (LateUpdate sẽ chạy)
        yield return new WaitForEndOfFrame();

        // Camera nên di chuyển về phía player
        float currentCameraX = cameraObj.transform.position.x;

        Assert.Greater(currentCameraX, initialCameraX, "Camera should move toward the player.");
    }

    [UnityTest]
    public IEnumerator Test_CameraOffsetIsCorrect()
    {
        // Đặt player tại x = 10
        playerObj.transform.position = new Vector3(10f, 0f, 0f);
        cameraFollow.xOffset = 3f;

        yield return new WaitForEndOfFrame();

        float expectedX = 13f;
        float actualX = cameraObj.transform.position.x;

        // Camera nên di chuyển về phía target.x + offset (13), nhưng chưa tới ngay
        float distance = Mathf.Abs(expectedX - actualX);

        Assert.Less(distance, 1.5f, "Camera should move close to the target + offset.");
    }

}
