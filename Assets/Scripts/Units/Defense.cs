using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace Project.Units
{
    [Serializable]
    public class Defense : Utility.IXmlNode
    {
        [HideInInspector]
        public int Armor { get; set; }
        [HideInInspector]
        public int MaxEntrenchment { get; set; }
        [HideInInspector]
        public int Morale { get; set; }

        public XmlNode ToXmlNode(XmlDocument document)
        {
            var defenseNode = document.CreateNode(XmlNodeType.Element, "Defense", null);

            var armorNode = document.CreateNode(XmlNodeType.Element, "Armor", null);
            armorNode.InnerText = Armor.ToString();
            defenseNode.AppendChild(armorNode);

            var maxEntrenchmentNode = document.CreateNode(XmlNodeType.Element, "MaxEntrenchment", null);
            maxEntrenchmentNode.InnerText = MaxEntrenchment.ToString();
            defenseNode.AppendChild(maxEntrenchmentNode);

            var moraleNode = document.CreateNode(XmlNodeType.Element, "Morale", null);
            moraleNode.InnerText = Morale.ToString();
            defenseNode.AppendChild(moraleNode);

            return defenseNode;
        }

        public bool LoadFromXmlNode(XmlNode node)
        {
            if (node.Name != "Defense")
            {
                return false;
            }
            foreach (XmlNode child in node.ChildNodes)
            {
                switch (child.Name)
                {
                    case "Armor":
                        Armor = int.Parse(child.InnerText);
                        break;
                    case "MaxEntrenchment":
                        MaxEntrenchment = int.Parse(child.InnerText);
                        break;
                    case "Morale":
                        Morale = int.Parse(child.InnerText);
                        break;
                    default:
                        break;
                }
            }
            return true;
        }
    }
}
