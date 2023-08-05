using System;
using System.IO;
using UnityEngine;
using MelonLoader;
using LibSM64;
using UnityEngine.SceneManagement;
using System.Reflection;


namespace Mario64
{
    public class Mario64 : MelonMod
    {
        public static AssetBundle marioBundle;

        public static bool hasMario = false;

        public static GameObject mario;

        public static MethodInfo clickingOnThings;

        public override void OnApplicationStart()
        {
            marioBundle = AssetBundle.LoadFromFile(Path.Combine(Path.GetFullPath("."), "Mods\\mario64"));
            if (marioBundle == null)
            {
                Debug.Log("Failed to load mario asset bundle! :(");
                return;
            }

            clickingOnThings = typeof(StanleyController).GetMethod("ClickingOnThings", BindingFlags.NonPublic | BindingFlags.Instance);
            MelonLogger.Msg(clickingOnThings);
        }

        public override void OnUpdate()
        {
            if (!hasMario)
            {
                if (Input.GetKeyUp(KeyCode.M))
                {
                    hasMario = true;

                    var stanley = GameObject.FindObjectOfType<StanleyController>();
                    stanley.FreezeMotionAndView();

                    CreateMario();

                    stanley.cam.gameObject.AddComponent(typeof(ExampleCamera));
                    stanley.cam.gameObject.GetComponent<ExampleCamera>().target = mario;
                }
            }else
            {
                var mario = GameObject.FindObjectOfType<SM64Mario>();
                if (mario == null)
                {
                    hasMario = false;
                }

                if (Input.GetKeyDown(KeyCode.PageUp))
                {
                    var pos = mario.transform.position;
                    mario.transform.position = new Vector3(pos.x, pos.y + 10, pos.z);
                }

                if (Input.GetKeyDown(KeyCode.PageDown))
                {
                    var pos = mario.transform.position;
                    mario.transform.position = new Vector3(pos.x, pos.y - 10, pos.z);
                }
            }

            if (Input.GetKeyUp(KeyCode.R))
            {
                DefineSurfaces();
                SM64Context.RefreshStaticTerrain();
            }

            if (hasMario)
            {
                var stanley = GameObject.FindObjectOfType<StanleyController>();
                stanley.FreezeMotionAndView();

                stanley.transform.position = mario.transform.position;
                stanley.transform.rotation = mario.transform.rotation;
            }
        }

        public static void Interact()
        {
            var stanley = GameObject.FindObjectOfType<StanleyController>();
            clickingOnThings.Invoke(stanley, new object[] { });
        }

        private static void DefineSurfaces()
        {
            Scene scene = SceneManager.GetActiveScene();

            var rootObjects = GameObject.FindObjectsOfType<MeshCollider>();
            
            /*
            List<GameObject> rootObjects = new List<GameObject>();
            scene.GetRootGameObjects(rootObjects);
            */

            foreach (var obj in rootObjects)
            {
                GameObject gameObj = obj.gameObject;

                if (gameObj.GetComponent<SM64StaticTerrain>() == null && gameObj.GetComponent<MeshCollider>() != null)
                {
                    gameObj.AddComponent<SM64StaticTerrain>();
                }
            }
        }

        private static void CreateMario()
        {
            var material = marioBundle.LoadAsset<Material>("DefaultMario");

            MelonLogger.Msg($"{material.shader}");

            var prefab = marioBundle.LoadAsset<GameObject>("Mario");
            var obj = UnityEngine.Object.Instantiate(prefab);
            obj.name = prefab.name;

            obj.transform.position = StanleyController.Instance.gameObject.transform.position;

            obj.AddComponent(typeof(ExampleInputProvider));
            obj.GetComponent<ExampleInputProvider>().cameraObject = GameObject.Find("Stanley(Clone)/CameraParent/Main Camera");

            obj.AddComponent(typeof(SM64Mario));
            obj.GetComponent<SM64Mario>().material = marioBundle.LoadAsset<Material>("DefaultMario");

            mario = obj;
        }
    }
}
