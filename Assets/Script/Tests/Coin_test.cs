using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CoinTest_Đơn_Giản
{
    private GameObject player;
    private GameObject coin;
    private GameObject audio;
    private FakeSoundManager soundManager;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Tạo Player
        player = new GameObject("Player");
        player.tag = "Player";
        player.AddComponent<Rigidbody2D>();

        // Tạo Audio & gán SoundManager giả
        audio = new GameObject("Audio");
        audio.tag = "Audio";
        soundManager = audio.AddComponent<FakeSoundManager>();

        // Tạo Coin và gán collider
        coin = new GameObject("Coin");
        var collider = coin.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        coin.AddComponent<Coin>();

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        GameObject.Destroy(player);
        GameObject.Destroy(coin);
        GameObject.Destroy(audio);
        yield return null;
    }

    [UnityTest]
    public IEnumerator Coin_Goi_PlaySFX_Khi_Player_Va_Cham()
    {
        player.transform.position = Vector3.zero;
        coin.transform.position = Vector3.zero;

        // Chờ để trigger xử lý
        yield return new WaitForSeconds(0.1f);

        Assert.IsTrue(soundManager.wasPlayed, "Âm thanh không được phát khi Player va chạm với Coin.");
    }

    // Giả lập SoundManager
    public class FakeSoundManager : MonoBehaviour
    {
        public bool wasPlayed = false;
        public AudioClip coin;

        public void PlaySFX(AudioClip clip)
        {
            wasPlayed = true;
            Debug.Log("PlaySFX() đã được gọi!");
        }
    }
}
