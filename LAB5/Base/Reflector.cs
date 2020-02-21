using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using LAB5.Exception_Classes;
using Newtonsoft.Json;

namespace LAB5.Base
{
    [Serializable]
    public static class Reflector
    {
        public static void Analyze(ReflectionMetadata metadata, string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(metadata, Formatting.Indented));
        }


        public static object Create(Type type, params object[] parms)
        {
            var instance = Activator.CreateInstance(type ?? throw new EgyptException("Class not found"), parms);
            return instance;
        }

        public static object Create(string type, params object[] parms)
        {
            return Create(Type.GetType(type), parms);
        }


        public static object Invoke(object obj, string method, bool createObj, params string[] parms)
        {
            if (createObj) return Invoke((Type) obj, method, parms);

            var toInvoke = obj.GetType().GetMethod(method);
            if (toInvoke != null) toInvoke.Invoke(obj, parms);
            else throw new EgyptException($"Method not found ({method})");
            return default;
        }

        public static object Invoke(Type type, string method, params string[] methodparms)
        {
            var instance = Create(type);
            Invoke(instance, method, false, methodparms);
            return instance;
        }

        public static object Invoke(string type, string method, params string[] methodparms)
        {
            var instance = Invoke(Type.GetType(type), method, methodparms);
            Invoke(instance, method, false, methodparms);
            return instance;
        }


        public static void PrintMetadata(ReflectionMetadata metadata)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n-------------------------METADATA INFORMATION--------------------------");
            Console.WriteLine($"GUID: {metadata.GUID}");
            Console.WriteLine($"Type: {metadata.Type}");
            Console.WriteLine($"Base Type: {metadata.BaseType}");
            Console.WriteLine($"Assembly: {metadata.Assembly}");
            Console.WriteLine($"Has constructor: {metadata.HasConstructor}");
            Console.WriteLine($"IsNested: {metadata.IsNested}");
            Console.WriteLine($"IsSealed: {metadata.IsSealed}");
            Console.WriteLine("Nested Types:");
            foreach (var type in metadata.NestedTypes) Console.WriteLine($"\t{type}");

            Console.WriteLine("Fields:");
            foreach (var field in metadata.PublicFields) Console.WriteLine($"\t(public) {field}");
            foreach (var field in metadata.PrivateFields) Console.WriteLine($"\t(private) {field}");

            Console.WriteLine("Properties:");
            foreach (var property in metadata.PublicProperties) Console.WriteLine($"\t(public) {property}");
            foreach (var property in metadata.PrivateProperties) Console.WriteLine($"\t(private) {property}");

            Console.WriteLine("Methods:");
            foreach (var method in metadata.PublicMethods) Console.WriteLine($"\t(public) {method}");
            foreach (var method in metadata.PrivateMethods) Console.WriteLine($"\t(private) {method}");
            Console.WriteLine("------------------------------------------------------------------\n");
            Console.ResetColor();
        }

        [Serializable]
        public readonly struct ReflectionMetadata
        {
            public readonly bool HasConstructor;
            public readonly bool IsNested;
            public readonly bool IsSealed;
            public readonly string Type;
            public readonly string GUID;
            public readonly string Assembly;
            public readonly string BaseType;
            public readonly List<string> NestedTypes;
            public readonly List<string> PublicFields;
            public readonly List<string> PrivateFields;
            public readonly List<string> PublicProperties;
            public readonly List<string> PrivateProperties;
            public readonly List<string> PublicMethods;
            public readonly List<string> PrivateMethods;

            public ReflectionMetadata(Type reflectionBase)
            {
                var myType = reflectionBase;
                IsSealed = myType.IsSealed;
                IsNested = myType.IsNested;
                GUID = myType.GUID.ToString();
                HasConstructor = myType.GetConstructors().Length != 0;
                Type = myType.ToString();
                Assembly = myType.Assembly.FullName;
                BaseType = myType.BaseType != null ? myType.BaseType.FullName : default;

                NestedTypes = new List<string>();
                PublicFields = new List<string>();
                PrivateFields = new List<string>();
                PublicProperties = new List<string>();
                PrivateProperties = new List<string>();
                PublicMethods = new List<string>();
                PrivateMethods = new List<string>();

                foreach (var type in myType.GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Public |
                                                           BindingFlags.Instance))
                    NestedTypes.Add(type.Name);

                foreach (var field in myType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                    PrivateFields.Add(field.Name);

                foreach (var property in myType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    PublicProperties.Add(property.Name);

                foreach (var property in myType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance))
                    PrivateProperties.Add(property.Name);

                foreach (var method in myType.GetMethods(BindingFlags.Public & BindingFlags.Instance))
                    PublicMethods.Add(method.Name);

                foreach (var method in myType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
                    PrivateMethods.Add(method.Name);
            }
        }
    }
}