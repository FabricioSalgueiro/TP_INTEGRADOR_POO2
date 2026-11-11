using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DungeonSurvivor.Domain.Persistence
{
    public static class SaveSystem
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions { WriteIndented = true };

        public static async Task GuardarEnArchivoAsync(string path, SaveData data)
        {
            string json = JsonSerializer.Serialize(data, _options);
            await File.WriteAllTextAsync(path, json);
        }

        public static async Task<SaveData?> CargarDesdeArchivoAsync(string path)
        {
            if (!File.Exists(path)) return null;

            string json = await File.ReadAllTextAsync(path);
            return JsonSerializer.Deserialize<SaveData>(json, _options);
        }
    }
}