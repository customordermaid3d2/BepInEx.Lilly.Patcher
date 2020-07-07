using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using BepInEx.Harmony;
using HarmonyLib;
using UnityEngine;
using Mono.Cecil;

namespace BepInEx.Lilly.Patcher
{
    public static class Patcher
    {
        private static HarmonyLib.Harmony instance;

        // List of assemblies to patch
        //public static IEnumerable<string> TargetDLLs => GetDLLs();
        //
        //public static IEnumerable<string> GetDLLs()
        //{
        //    // Do something before patching Assembly-CSharp.dll
        //    
        //    Log("GetDLLs.Assembly-CSharp.dll");
        //    yield return "Assembly-CSharp.dll";
        //
        //    // Do something after Assembly-CSharp has been patched, and before UnityEngine.dll has been patched
        //
        //    Log("GetDLLs.UnityEngine.dll");
        //    yield return "UnityEngine.dll";
        //
        //    // Do something after patching is done
        //}

        // Called before patching occurs
        public static void Initialize()
        {
            Log("Initialize start");
            //HarmonyLib.Harmony.CreateAndPatchAll(typeof(Patcher));
            //var harmony = new HarmonyLib.Harmony("BepInEx.Lilly.Patcher.Patcher");
            instance = HarmonyLib.Harmony.CreateAndPatchAll(typeof(Patcher), "BepInEx.Lilly.Patcher.Patcher");
            Log("Initialize end");
        }

        // Patches the assemblies
        public static void Patch(AssemblyDefinition assembly)
        // public static void Patch(ref AssemblyDefinition assembly);
        /*
        In the latter case, the reference to the AssemblyDefinition is passed. 
        That means it is possible to fully swap an assembly for a different one.
        후자의 경우 AssemblyDefinition에 대한 참조가 전달됩니다.
        즉, 어셈블리를 다른 어셈블리로 완전히 교체 할 수 있습니다.
        */
        {
            // Patcher code here
            Log("Patch");

            if (assembly.Name.Name == "Assembly-CSharp")
            {
                // The assembly is Assembly-CSharp.dll
                Log("Patch.Assembly-CSharp");
            }
            else if (assembly.Name.Name == "UnityEngine")
            {
                // The assembly is UnityEngine.dll
                Log("Patch.UnityEngine");
            }
        }


        //[HarmonyPatch(typeof(AudioSourceMgr), nameof(AudioSourceMgr.LoadPlay), MethodType.Getter)] // Specify target method with HarmonyPatch attribute
        [HarmonyPatch(typeof(AudioSourceMgr), "LoadPlay")] // Specify target method with HarmonyPatch attribute
        [HarmonyPrefix]                              // There are different patch types. Prefix code runs before original code
        public static bool LoadPlayPrefix(AudioSourceMgr __instance, ref string f_strFileName)
        {   // public void LoadPlay(string f_strFileName, float f_fFadeTime, bool f_bStreaming, bool f_bLoop = false)
            Log("LoadPlayPrefix");
            Log("f_strFileName:"+ f_strFileName);
            // - returns a boolean that controls if original is executed (true) or not (false)
            return true;
        }

        [HarmonyPatch(typeof(AudioSourceMgr), "LoadPlay")] // Specify target method with HarmonyPatch attribute
        [HarmonyPostfix]                              // There are different patch types. Prefix code runs before original code
        public static void LoadPlayPostfix()
        {
            Log("LoadPlayPostfix");
        }

        // Called after current patcher is done
        public static void Finish()
        {
            Log("Finish");
        }

        //------------------------ 로그 처리 ---------------------------

        static String name = "BepInEx.Lilly.Patcher";

        public static void Log(System.Object s)
        {
            LogConsole(s, ConsoleColor.White);
            //Debug.Log(s);
        }

        public static void LogWarning(System.Object s)
        {
            LogConsole(s, ConsoleColor.Yellow);
        }

        public static void LogError(System.Object s)
        {
            LogConsole(s, ConsoleColor.Red);
        }

        public static void LogConsole(System.Object s, ConsoleColor c = ConsoleColor.White)
        {           
            Console.ForegroundColor = c;
            Console.WriteLine(name + ":" + s);
            Console.ResetColor();
        }
    }
}
