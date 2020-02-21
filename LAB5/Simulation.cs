using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Xml;
using System.Xml.Linq;
using LAB5.Base;
using LAB5.Exception_Classes;
using LAB5.Hierarchy;

namespace LAB5
{
    internal class Simulation
    {
        private static void Main()
        {
            try
            {
                var workingDirectory = Environment.CurrentDirectory;
                var binPath = Directory.GetParent(workingDirectory).Parent?.FullName + @"\Files\Bin.bin";
                var jsonPath = Directory.GetParent(workingDirectory).Parent?.FullName + @"\Files\JSON.json";
                var nsPath = Directory.GetParent(workingDirectory).Parent?.FullName + @"\Files\NewtonSoft.json";
                var xmlPath = Directory.GetParent(workingDirectory).Parent?.FullName + @"\Files\XML.xml";
                var xdocPath = Directory.GetParent(workingDirectory).Parent?.FullName + @"\Files\xdoc.xml";
                var xdoccPath = Directory.GetParent(workingDirectory).Parent?.FullName + @"\Files\xdocc.xml";
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Starting simulation...\n");
                //var nile = new Village("Nile River");
                var f1 = new FarmerSlave("Pashedu", "Farmer");
                var f2 = new FarmerSlave("Nykara", "Farmer");
                var c1 = new Craftsmen("Nikaure", "Craftsmen");
                var c2 = new Craftsmen("Djedi", "Craftsmen");
                var m1 = new Merchant("Paser", "Merchant");
                var scr1 = new Scribe("Pipi", "Scribe");
                var sld1 = new Soldier("Qen", "Soldier");
                var sld2 = new Soldier("Shoshenq", "Soldier");
                var pr1 = new PriestNoblesOfficials("Wennefer", "Priest");
                var pr2 = new PriestNoblesOfficials("Siese", "Treasurer");
                var pr3 = new PriestNoblesOfficials("Djedptahiufankh", "Prophet");
                var pharaoh = new Pharaoh("Tutankhamun", "Pharaoh");

                var coll1 = new Egypt<Egyptian>("Egypt", f1, f2, c1, c2, m1, scr1, sld1, sld2, pr1, pr2, pr3);
                var coll2 = new Egypt<Egyptian>("2Egypt2", pharaoh);
                var coll = coll1 + coll2;

                CustomSerializer.BinSerialize(coll, binPath);
                var coll51 = CustomSerializer.BinDeserialize(binPath);
                Console.WriteLine(coll51);

                CustomSerializer.NewtonsoftSerialize(coll,nsPath);
                var p1 = CustomSerializer.NewtonsoftDeserialize<Egypt<Egyptian>>(nsPath);
                Console.WriteLine(p1);

                CustomSerializer.JsonSerialize(pr3, jsonPath);
                var p2 = CustomSerializer.JsonDeserialize<PriestNoblesOfficials>(jsonPath);
                Console.WriteLine(p2);

                CustomSerializer.XmlSerialize<Soldier>(sld1, xmlPath);
                var p3 = CustomSerializer.XmlDeserialize<Soldier>(xmlPath);
                Console.WriteLine(p3);

                var xdoc = XDocument.Load(xdocPath);

                var ids = from dialog in xdoc.Element("game_dialogs")?.Elements("dialog")
                    where dialog.Attribute("weight") != null
                    select dialog;

                foreach (var xElement in ids)
                {
                    Console.WriteLine(xElement + "\n");
                }

                var xdocc = new XDocument(
                    new XDeclaration("1.0", "windows-1251", "yes"),
                    new XElement("RootItem",
                        new XElement("Item", 
                            new XElement("Text", "Text node"),
                            new XElement("Text", "Another text node"),
                            new XAttribute("id", "1")),
                        new XElement("Item",
                            new XElement("Text", "Only one text node"),
                            new XAttribute("id", "2")))
                    );

                xdocc.Save(xdoccPath);
            }
            catch (EgyptDirectoryNotFoundException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nError: {e.Message}");
                Console.WriteLine("Unfound paths: ");
                foreach (var pathh in e.Invalidpaths) Console.Write($"-->{pathh}\n");
            }
            catch (PersonArgumentException e) when (e.IntValue != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nError: {e.Message}");
                Console.WriteLine($"Invalid int value: \'{e.IntValue}\'");
                Console.WriteLine($"Method: {e.TargetSite}");
                Console.WriteLine($"Stack: {e.StackTrace}");
            }
            catch (PersonArgumentException e) when (e.StringValue != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nError: {e.Message}");
                Console.WriteLine($"Invalid string value: \'{e.StringValue}\'");
                Console.WriteLine($"Method: {e.TargetSite}");
                Console.WriteLine($"Stack: {e.StackTrace}");
            }
            catch (EgyptIndexOutOfRangeException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nError: {e.Message}");
                Console.WriteLine($"Method: {e.TargetSite}");
                Console.WriteLine($"Stack: {e.StackTrace}");
            }
            catch (PersonNotFoundException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nError: {e.Message}");
                Console.WriteLine($"Method: {e.TargetSite}");
                Console.WriteLine($"Stack: {e.StackTrace}");
            }
            catch (EgyptException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nError: {e.Message}");
                Console.WriteLine($"Method: {e.TargetSite}");
                Console.WriteLine($"Stack: {e.StackTrace}");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nError: {e.Message}");
                Console.WriteLine($"Method: {e.TargetSite}");
                Console.WriteLine($"Stack: {e.StackTrace}");
            }

            finally
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Program finally finished");
            }
        }
    }
}