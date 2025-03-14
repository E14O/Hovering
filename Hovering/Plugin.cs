using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hovering
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;
        public GameObject HBV, EF;
        public bool Enabled = true;
        public bool VREnabled = true;
        public bool button;
        private ConfigEntry<bool> HoveringOnStartUp, WorkingOnVr;

        void Start()
        {
            GorillaTagger.OnPlayerSpawned(OnGameInitialized);
        }
        void OnGameInitialized()
        {
            HBV = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/RigAnchor/rig/body/HoverboardVisual");
            EF = GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToHoverboard/TurnOnForHoverboard");

            HoveringOnStartUp = Config.Bind("EOJ Settings", "EnableOnJoin", true, "Toggles If The Hoverboard Is Enabled On StartUp");
            WorkingOnVr = Config.Bind("WOVR Settings", "WorkOnVr", true, "Checks If The Hoverboard Can Be Toggled In VR");

            if (HoveringOnStartUp.Value == true)
            {
                EF.SetActive(true);
                HBV.SetActive(true);
            }

            if (HoveringOnStartUp.Value == false)
            {
                HBV.SetActive(false);
                EF.SetActive(false);
            } 
        }

        void Update()
        {
            if (ControllerInputPoller.instance.rightControllerSecondaryButton && !button)
            {
                    if (VREnabled == true)
                    {
                        if (Enabled == true)
                        {
                            HBV.SetActive(false);
                            EF.SetActive(false);
                            Enabled = false;
                            HoveringOnStartUp.Value = false;
                    }
                    else
                        {
                            EF.SetActive(true);
                            HBV.SetActive(true);
                            Enabled = true;
                            HoveringOnStartUp.Value = true;
                        }
                    }
            }
            if (Keyboard.current.f3Key.wasPressedThisFrame)
            {
                    if (Enabled == true)
                    {
                        HBV.SetActive(false);
                        EF.SetActive(false);
                        Enabled = false;
                        HoveringOnStartUp.Value = false;
                    }
                    else
                    {
                        EF.SetActive(true);
                        HBV.SetActive(true);
                        Enabled = true;
                        HoveringOnStartUp.Value = true;
                    }
            }
            if (Keyboard.current.f4Key.wasPressedThisFrame)
            {
                if (VREnabled == true && WorkingOnVr.Value == true)
                {
                    VREnabled = false;
                    Debug.Log("H: VRENABLED = FALSE");
                    WorkingOnVr.Value = false;
                }
                else
                {
                    VREnabled = true;
                    Debug.Log("H: VRENABLED = TRUE");
                    WorkingOnVr.Value = true;
                }
            }
        }
    }
}