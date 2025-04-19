//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Cainos.PixelArtPlatformer_VillageProps
//{
//    public class Chest : MonoBehaviour
//    {
//       // [FoldoutGroup("Reference")] public Animator animator;
//       // [FoldoutGroup("Reference")] public GameObject itemPrefab;
//      //  [FoldoutGroup("Reference")] public GameObject itemPrefab1;
//       // [FoldoutGroup("Reference")] public GameObject monsterPrefab;
//       // [FoldoutGroup("Reference")] public GameObject monsterPrefab1;

//        private bool isOpened;
//        private SoundManager soundManager;

//       // [FoldoutGroup("Runtime"), ShowInInspector, DisableInEditMode]
//        public bool IsOpened
//        {
//            get => isOpened;
//            set
//            {
//                isOpened = value;
//               // animator.SetBool("IsOpened", isOpened);
//            }
//        }

//       // [FoldoutGroup("Runtime"), Button("Open"), HorizontalGroup("Runtime/Button")]
//        public void Open()
//        {
//            if (IsOpened) return;

//            IsOpened = true;

//            if (soundManager != null)
//            {
//                soundManager.PlaySFX(soundManager.ruong);
//            }

//            GenerateItemOrMonster();
//        }

//       // [FoldoutGroup("Runtime"), Button("Close"), HorizontalGroup("Runtime/Button")]
//        public void Close()
//        {
//            IsOpened = false;
//        }

//        private void Awake()
//        {
//            GameObject audioObject = GameObject.FindGameObjectWithTag("Audio");
//            if (audioObject != null)
//            {
//                soundManager = audioObject.GetComponent<SoundManager>();
//            }
//        }

//        private void Update()
//        {
//            if (IsOpened) return;

//            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);
//            foreach (Collider2D collider in colliders)
//            {
//                if (collider.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
//                {
//                    Open();
//                    break;
//                }
//            }
//        }

//        private void GenerateItemOrMonster()
//        {
//            bool generateMonster = Random.value > 0.5f;

//            GameObject prefabToSpawn;

//            if (generateMonster)
//            {
//               // prefabToSpawn = (Random.value > 0.5f) ? monsterPrefab : monsterPrefab1;
//            }
//            else
//            {
//               // prefabToSpawn = (Random.value > 0.5f) ? itemPrefab : itemPrefab1;
//            }

//            //Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
//        }
//    }
//}
