using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace Project.Dices
{
    public abstract class Dice : Utility.IXmlNode
    {
        public abstract int Roll();
        public abstract EDice ToEnum();

        public XmlNode ToXmlNode(XmlDocument document)
        {
            var node = document.CreateNode(XmlNodeType.Element, "Dice", null);
            node.InnerText = ToEnum().ToString();
            return node;
        }

        public bool LoadFromXmlNode(XmlNode node)
        {
            throw new Exception("Can't load dices from xml node like that. Use DiceFactory.");
        }
    }

    public enum EDice
    {
        D4,
        D6,
        D8,
        D10,
        D12
    }
}
