using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Mod
{
	public class AssemblyLoader
        {
                public static Type CommandManagerType;
                public static MethodInfo RegisterCommandMethod;

                public static void LoadMod()
                {
                        ModScript modScript = null;
			
                        try
                        {
                                foreach (KeyValuePair<ModMetaData, ModScript> pair in ModLoader.ModScripts)
                                {
                                        if (pair.Key.Active && pair.Key.Name == "AvM Mod PPG" && pair.Key.Author == "0xc0000135")
                                        {
                                                modScript = pair.Value;
                                                break;
                                        }
                                }
                        }
			catch
			{
                                throw new DllNotFoundException("Mod is not found");
                                return;
			}
			
                        try
                        {
                                CommandManagerType = modScript.LoadedAssembly.GetType("AvMPPG.CommandManager");
                        }
                        catch
                        {
                                throw new MissingMemberException("Command Manager is not found");
                                return;
                        }

                        try
                        {
                                RegisterCommandMethod = CommandManagerType.GetMethod("RegisterCommand");
                        }
                        catch
                        {
                                throw new MissingMethodException("Method Register Command is not found");
                                return;
                        }

                        Debug.Log($"Success loaded {ModAPI.Metadata.Name}");
                }
        }

        public class CommandManager
        {
                public static void RegisterCommand(string str, Action<string[]> action)
                {
                        AssemblyLoader.RegisterCommandMethod.Invoke(null, new object[]{str, action});
                }
        }
}