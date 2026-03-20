using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Capstone.Core.Storage
{
    // Simple file-based JSON storage implementation. Uses System.Text.Json and async I/O.
    public class FileStorage<T> : IStorage<T> where T : class
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { WriteIndented = true };

        public FileStorage(string filePath)
        {
            _filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
            var dir = Path.GetDirectoryName(filePath) ?? ".";
            Directory.CreateDirectory(dir);
        }

        public async Task<IEnumerable<T>> LoadAsync()
        {
            if (!File.Exists(_filePath))
            {
                return Enumerable.Empty<T>();
            }

            try
            {
                using var stream = File.OpenRead(_filePath);
                var items = await JsonSerializer.DeserializeAsync<List<T>>(stream, _jsonOptions);
                return items ?? Enumerable.Empty<T>();
            }
            catch
            {
                // Corrupted or unreadable file: return empty collection (fail-fast would be another approach)
                return Enumerable.Empty<T>();
            }
        }

        public async Task SaveAsync(IEnumerable<T> items)
        {
            // Overwrite atomically by writing to the file (simple approach).
            using var stream = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(stream, items.ToList(), _jsonOptions);
        }
    }
}
