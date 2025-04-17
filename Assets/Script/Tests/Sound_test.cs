using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Sound_test
{
    private GameObject soundObject;
    private AudioSource audioSource;
    private AudioClip testClip;

    [SetUp]
    public void SetUp()
    {
        // Tạo một GameObject mới và thêm AudioSource
        soundObject = new GameObject();
        audioSource = soundObject.AddComponent<AudioSource>();

        // Tải một clip âm thanh để kiểm tra
        testClip = Resources.Load<AudioClip>("YourSoundClip");
        audioSource.clip = testClip;
    }

    [TearDown]
    public void TearDown()
    {
        // Xóa GameObject sau mỗi test để không gây ảnh hưởng đến test khác
        GameObject.Destroy(soundObject);
    }

    [Test]
    public void PlaySoundTest()
    {
        // Kiểm tra xem âm thanh có được phát khi gọi Play
        audioSource.Play();

        // Kiểm tra xem AudioSource có đang phát không
        Assert.IsTrue(audioSource.isPlaying);
    }

    [Test]
    public void SoundEffectTriggerTest()
    {
        // Giả sử khi nhấn nút, âm thanh sẽ được phát
        audioSource.PlayOneShot(testClip);

        // Kiểm tra xem âm thanh có đang phát không
        Assert.AreEqual(audioSource.isPlaying, true);
    }
}
