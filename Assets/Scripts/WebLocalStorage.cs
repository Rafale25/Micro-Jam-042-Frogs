using System.Runtime.InteropServices;

public class WebLocalStorage //Internal
{
    [DllImport("__Internal")]
    public static extern void HelloWorld();

    [DllImport("__Internal")]
    public static extern void LocalStorageSet(string key, string value);

    [DllImport("__Internal")]
    public static extern string LocalStorageGet(string key);

    [DllImport("__Internal")]
    public static extern void LocalStorageRemove(string key);

    [DllImport("__Internal")]
    public static extern void LocalStorageClear();
}

// public class WebLocalStorage
// {
//     public static void LocalStorageSet(string key, string value);
//     public static string LocalStorageGet(string key);
//     public static void LocalStorageRemove(string key);
//     public static void LocalStorageClear();
// }
