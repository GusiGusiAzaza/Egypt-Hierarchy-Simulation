using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Xml.Serialization;
using LAB5.Exception_Classes;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace LAB5.Base
{
    internal static class CustomSerializer
    {
        public static void BinSerialize(object obj, string path)
        {
            FileSystemManager.CheckPathValidity(path.Remove(path.LastIndexOf('\\')));
            var formatter = new BinaryFormatter();
            using var stream = new FileStream(path, FileMode.OpenOrCreate);
            formatter.Serialize(stream, obj);
            stream.Close();
        }

        public static object BinDeserialize(string path)
        {
            FileSystemManager.CheckPathValidity(path);
            using var openFileStream = File.OpenRead(path);
            var deserializer = new BinaryFormatter();
            var obj = deserializer.Deserialize(openFileStream);
            openFileStream.Close();
            return obj;
        }

        public static void XmlSerialize<T>(object obj, string path)
        {
            var ser = new XmlSerializer(typeof(T));
            TextWriter writer = new StreamWriter(path);
            ser.Serialize(writer, obj);
            writer.Close();
        }

        public static object XmlDeserialize<T>(string path)
        {
            var ser = new XmlSerializer(typeof(T));
            Stream stream = new FileStream(path, FileMode.Open);
            var obj = ser.Deserialize(stream);
            return obj;
        }

        public static void JsonSerialize(object obj, string path)
        {
            FileSystemManager.CheckPathValidity(path.Remove(path.LastIndexOf('\\')));
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            using var stream = File.CreateText(path);
            stream.Write(JsonSerializer.Serialize(obj, options));
            stream.Close();
        }

        public static object JsonDeserialize<T>(string path)
        {
            var obj = JsonSerializer.Deserialize<T>(File.ReadAllText(path));
            return obj;
        }

        public static void NewtonsoftSerialize(object obj, string path)
        {
            FileSystemManager.CheckPathValidity(path.Remove(path.LastIndexOf('\\')));
            File.WriteAllText(path, JsonConvert.SerializeObject(obj, Formatting.Indented));
        }

        public static object NewtonsoftDeserialize<T>(string path)
        {
            FileSystemManager.CheckPathValidity(path);
            var obj = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return obj;
        }

        public static bool CheckPathValidity(params string[] arr)
        {
            var invalidpaths = arr.Where(path => !File.Exists(path)).ToList();

            if (invalidpaths.Count > 0)
                throw new EgyptDirectoryNotFoundException("File directory not found", invalidpaths);
            return true;
        }
    }
}