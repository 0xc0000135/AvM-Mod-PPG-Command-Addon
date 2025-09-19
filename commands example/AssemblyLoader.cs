using System;
using System.Collections.Generic;
using System.Reflection;
using System.Globalization;
using UnityEngine;

namespace Mod
{
	public class AssemblyLoader
    {
        public static Type CommandManagerType;
        public static MethodInfo RegisterCommandMethod;
        public static bool isLoaded = false;

        internal static string cultureInfo;
        public static string TranslateText(string En, string Ru) => cultureInfo.Contains("ru") ? Ru : En;

        public static List<string> errorCodes = new List<string>()
        {
            "1 (ModNotFound)",
            "2 (TypeNotFound)",
            "3 (MethodNotFound)"
        };
            

        public static void Error(int code)
        {
            DialogBox dialog1 = DialogBoxManager.Dialog(TranslateText($"{ModAPI.Metadata.Name}: An error occurred\nError Code: {errorCodes[code-1]}", $"{ModAPI.Metadata.Name}: Произошла ошибка\nКод ошибки: {errorCodes[code-1]}"), new DialogButton[]
            {
                new DialogButton(TranslateText("Install/Update AvM Mod PPG", "Установить/Обновить AvM Mod PPG"), true, () =>
                {
                    Utils.OpenURL("https://steamcommunity.com/sharedfiles/filedetails/?id=3352694588");
                    DialogBox dialog2 = DialogBoxManager.Dialog(TranslateText("After installing/updating the mod, restart the game.", "После установки/обновления мода, перезапустите игру."), new DialogButton[]
                    {
                        new DialogButton(TranslateText("Ok", "Ок"), true)
                    });
                }),
                new DialogButton(TranslateText("Cancel", "Отмена"), true)
            });
        }
        public static void LoadMod()
        {
            cultureInfo = CultureInfo.CurrentCulture.Name.ToLower();
            ModScript modScript = null;

            foreach (KeyValuePair<ModMetaData, ModScript> pair in ModLoader.ModScripts)
            {
                if (pair.Key.Active && pair.Key.Name == "AvM Mod PPG" && pair.Key.Author == "0xc0000135")
                {
                    modScript = pair.Value;
                    break;
                }
            }
            if (modScript==null)
            {
                Error(1);
                return;
            }

            CommandManagerType = modScript.LoadedAssembly.GetType("AvMPPG.CommandManager");

            if (CommandManagerType==null)
            {
                Error(2);
                return;
            }

            RegisterCommandMethod = CommandManagerType.GetMethod("RegisterCommand");

            if (RegisterCommandMethod==null)
            {
                Error(3);
                return;
            }

            isLoaded = true;
            Debug.Log(TranslateText($"[{ModAPI.Metadata.Name}]Success loaded", $"[{ModAPI.Metadata.Name}]Успешно загружен"));
        }
    }

    public class CommandManager
    {
        public static void RegisterCommand(string str, Action<string[]> action)
        {
            if (AssemblyLoader.isLoaded)
            {
                AssemblyLoader.RegisterCommandMethod.Invoke(null, new object[]{str, action});
            }
        }
    }
}
