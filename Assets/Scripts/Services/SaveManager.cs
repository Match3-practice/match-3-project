using System.IO;
using Unity.Plastic.Newtonsoft.Json;

public static class SaveManager
{

    public static void SaveGame(string path, Crystal[] crystals)
    {
        if (crystals.Length == 0) return;
        string jsonTypes = JsonConvert.SerializeObject(GetTypes(crystals));

        File.WriteAllText(path, jsonTypes);
    }


    public static Types[] LoadGame(string path)
    {
        string jsonTypes = string.Empty;
        if (File.Exists(path))
        {
            jsonTypes = File.ReadAllText(path);
        }

        return JsonConvert.DeserializeObject<Types[]>(jsonTypes);
    }

    private static Types[] GetTypes(Crystal[] crystals)
    {
        var types = new Types[crystals.Length];
        for (int i = 0; i < crystals.Length; i++)
        {
            types[i] = crystals[i].Type;
        }
        return types;
    }
}
