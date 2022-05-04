using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Project.Organizations.Ideologies
{ 
    public class Value
    {
        public string Name { get; private set; }
        public string Adjective { get; private set; }
        public string Description { get; private set; }
        public HashSet<Value> Compatible { get; private set; }
        public HashSet<Value> Contradictory { get; private set; }

        private static HashSet<Value> allValues;

        public Value(string name, string adjective, string description, HashSet<Value> compatible, HashSet<Value> contradictory)
        {
            Name = name;
            Adjective = adjective;
            Description = description;
            Compatible = compatible;
            Contradictory = contradictory;
            foreach (var compatibleValue in Compatible)
            {
                compatibleValue.Compatible.Add(this);
            }
            foreach (var contradictoryValue in Contradictory)
            {
                contradictoryValue.Contradictory.Add(this);
            }
            allValues.Add(this);
        }

        public static Value Parse(string path)
        {
            var name = "MISSING NAME";
            var adjective = "MISSING ADJECTIVE";
            var description = "MISSING DESCRIPTION";
            var compatible = new HashSet<Value>();
            var contradictory = new HashSet<Value>();
            var document = new XmlDocument();
            document.Load(path);
            XmlNode mainNode = document.DocumentElement.SelectSingleNode("/Value");
            foreach (XmlNode node in mainNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "Name":
                        name = node.InnerText;
                        break;
                    case "Adjective":
                        adjective = node.InnerText;
                        break;
                    case "Enchancement":
                        description = node.InnerText;
                        break;
                    case "Compatible":
                        foreach (XmlNode compatibleNode in node.ChildNodes)
                        {
                            foreach (var value in allValues)
                            {
                                if (compatibleNode.Name == "Value" && value.Name == compatibleNode.InnerText)
                                {
                                    compatible.Add(value);
                                    break;
                                }
                            }
                        }
                        break;
                    case "Contradictory":
                        foreach (XmlNode contradictoryNode in node.ChildNodes)
                        {
                            foreach (var value in allValues)
                            {
                                if (contradictoryNode.Name == "Value" && value.Name == contradictoryNode.InnerText)
                                {
                                    contradictory.Add(value);
                                    break;
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            return new Value(name,adjective,description,compatible,contradictory);
        }
    }
}
