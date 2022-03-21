using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Halyard.Extensions
{
    static class StringExtensions
    {
        public static async Task CheckFileAsync(this string value, byte[] bytes, bool exact = false)
        {
            if (exact)
            {
                if (!await value.FileEqualToAsync(bytes))
                    await value.CreateFileAsync(bytes);
            }
            else
            {
                if (!File.Exists(value))
                    await File.WriteAllBytesAsync(value, bytes);
            }
        }

        public static Task CheckDirectoryAsync(this string value)
        {
            if (!Directory.Exists(value))
                Directory.CreateDirectory(value);
            return Task.CompletedTask;
        }

        static async Task CreateFileAsync(this string value, byte[] bytes)
        {
            if (!await value.FileEqualToAsync(bytes))
                await File.WriteAllBytesAsync(value, bytes);
        }

        static async Task<bool> FileEqualToAsync(this string value, byte[] bytes)
        {
            if (File.Exists(value))
            {
                var file = await File.ReadAllBytesAsync(value);
                return file.SequenceEqual(bytes);
            }
            return false;
        }
    }
}
