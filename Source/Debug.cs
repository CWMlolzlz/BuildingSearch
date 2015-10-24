using System;
using System.Collections.Generic;
using ColossalFramework;


public class Debug {
    private static readonly string title = "BuildingSearch";
   
    static bool DEBUG = true;
    static bool usingModTools = false;
    public static void Print() { }
    public static void Print(IEnumerable<object> input) {
        foreach (object o in input) {
            Print(o);
        }
    }
    public static void Print(params object[] input) {
        foreach (object o in input) {
            Print(o);
        }
    }
    public static void Print(object obj) {

        if (DEBUG) {
            string output = obj == null ? "null" : obj.ToString();
            #if DEBUG
                if (usingModTools) {
                    UnityEngine.Debug.Log(output);
                } else {
                    DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Message, "[" + title + "] " + output);
                }
            #endif
        }
    }
}