using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Effect_portal_test
{
    private GameObject effectObject;
    private ParticleSystem particleSystem;

    [SetUp]
    public void SetUp()
    {
        // Tạo một GameObject mới và thêm ParticleSystem
        effectObject = new GameObject();
        particleSystem = effectObject.AddComponent<ParticleSystem>();
    }

    [TearDown]
    public void TearDown()
    {
        // Xóa GameObject sau mỗi test để không gây ảnh hưởng đến test khác
        GameObject.Destroy(effectObject);
    }

    [Test]
    public void ParticleSystemPlayTest()
    {
        // Kiểm tra nếu particle system có thể bắt đầu
        particleSystem.Play();

        // Đảm bảo particle system đang chạy
        Assert.IsTrue(particleSystem.isPlaying);
    }

    [Test]
    public void ParticleEffectTriggerTest()
    {
        // Giả sử khi có va chạm với đối tượng, particle system sẽ kích hoạt
        particleSystem.Play();

        // Kiểm tra xem particle system có đang phát không
        Assert.AreEqual(particleSystem.isPlaying, true);
    }
}
