using System;
using System.IO;
using System.Linq;
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
                var metadataPath = Directory.GetParent(workingDirectory).Parent?.FullName + @"\Files\Metadata.txt";
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Starting simulation...\n");
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

                foreach (Egyptian p in coll)
                {
                    p.Work();
                    p.Work();
                }

                f1.SellStuff();
                c1.SellStuff();
                m1.SellStuff();
                f2.SellLiveStock();
                f2.SellLiveStock();
                c2.BuyResources(3);
                c2.BuyResources(300);
                m1.BuyResources(1);

                //coll.DetailPrint();
                coll.Print();
                coll.FindWithName("Siese");

                Console.WriteLine("\nReturned with predicate");
                Console.WriteLine(coll.FirstOrDefault(n => n.Money > 50000) + "\n");
                var rich = new Egypt<Egyptian>("RIchness", coll.FindAll(n => n.Money < 1000).ToArray());
                //rich.DetailPrint();

                coll.Kill(scr1);
                coll.FindWithName("Pipi");
                coll.Kill(pharaoh);
                coll.FindWithName("Tutankhamun");
                //coll.DetailPrint();
                Console.WriteLine(c2.GetHashCode());
                Console.WriteLine(pharaoh.GetHashCode());
                Console.WriteLine(pr2.GetHashCode());

                coll.JsonSaveToFile(jsonPath);
                var coll3 = new Egypt<Egyptian>("JSON Egypt", jsonPath);
                coll3.JsonReadFromFile(jsonPath);
                coll3.Print();
                coll.FindWithName("GL HF");
                coll.Kill(coll[5]);
                coll2 -= coll;
                coll2.Print();
                var coll6 = new Egypt<Egyptian>("Egypt 3.0", jsonPath);
                coll6.Print();

                //coll.MetadataSaveToJson(metadataPath);

                ////!Exception generator!////

                //coll3[111].Work();
                //pharaoh.Age = 110;
                //pharaoh.Name = "a";
                //pharaoh.HardcoreLvl = -2;
                //const string errpath1 = @"H:\PROA\LB5,6,7\LB5\Egypt.bin";
                //const string errpath2 = @"H:\PRGA\LB5,6,7\LB5\Egypt.bin";
                //const string errpath3 = @"H:\3SEM\PROGA\\LB5\Egpt.bin";
                //CheckPathValidity(errpath1, errpath2, errpath3);

                ////!Exception generator!////

                //REFLECTION//
                var metadata = new Reflector.ReflectionMetadata(typeof(Egypt<>));
                Reflector.PrintMetadata(metadata);
                Console.ForegroundColor = ConsoleColor.White;
                //Reflector.PrintMetadata(pharaoh.Metadata);
                Reflector.Analyze(metadata, metadataPath);
                object[] parms = {"Tuta", "Pharaoh"};
                var obj = Reflector.Create("LAB5.Hierarchy.Pharaoh", parms);
                Console.WriteLine(obj);
                Reflector.Invoke(obj, "Work", false);
                Console.WriteLine(obj);
                var obj2 = Reflector.Invoke(typeof(Pharaoh), "Work", true);
                Console.WriteLine(obj2);
                Console.ForegroundColor = ConsoleColor.White;

                //SERIALIZATION
                CustomSerializer.BinSerialize(coll, binPath);
                var coll51 = CustomSerializer.BinDeserialize(binPath);
                Console.WriteLine(coll51);

                CustomSerializer.NewtonsoftSerialize(scr1, nsPath);
                var p1 = CustomSerializer.NewtonsoftDeserialize<Egyptian>(nsPath);
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

                foreach (var xElement in ids) Console.WriteLine(xElement + "\n");

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