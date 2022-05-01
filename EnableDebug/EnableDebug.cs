using MelonLoader;
using UnityEngine;

namespace EnableDebug
{
    public class EnableDebug : MelonMod
    {
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            GameObject obj = GameObject.Find("GameMaster/_TSPUD_MENU(Clone)/Root/Settings/PageTitles (Toggles)/Debug (PageButton)");
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }
    }
}
