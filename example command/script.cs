using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mod
{
  public class Mod
  {
    //Arguments is required
    public static void Test(string[] args)
    {
      Debug.Log("Tested!");
    }
    public static void OnLoad()
    {
      //To find the mod and get types and methods from it
      AssemblyLoader.LoadMod();

      //1) Command; 2) Method that will be executed when the command is entered
      CommandManager.RegisterCommand("test", Test);
    }
  }
}
