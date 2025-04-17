using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor.Animations;  // Thêm dòng này để sử dụng AnimatorController

public class Animation_test
{
    private GameObject character;
    private Animator animator;

    [SetUp]
    public void SetUp()
    {
        // Tạo một GameObject mới và gắn Animator vào
        character = new GameObject();
        animator = character.AddComponent<Animator>();

        // Giả sử bạn có một controller với các state đã thiết lập sẵn
        AnimatorController controller = Resources.Load<AnimatorController>("YourAnimatorController");
        animator.runtimeAnimatorController = controller;
    }

    [TearDown]
    public void TearDown()
    {
        // Xóa GameObject sau mỗi test để không gây ảnh hưởng đến test khác
        GameObject.Destroy(character);
    }

    [Test]
    public void AnimationTransitionTest()
    {
        // Kiểm tra xem animator có thể chuyển qua state 'Walk' khi trigger được gọi
        animator.SetTrigger("Walk");

        // Đảm bảo animation đang chạy
        Assert.IsTrue(animator.GetCurrentAnimatorStateInfo(0).IsName("Walk"));
    }

    [Test]
    public void AnimationPlayTest()
    {
        // Giả sử bạn muốn kiểm tra một animation bắt đầu khi game bắt đầu
        animator.Play("Idle");

        // Kiểm tra trạng thái animation
        Assert.IsTrue(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
    }
}
