using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Project.Dices
{
    public class DiceFactory
    {
        public DiceFactory()
        {
        }

        public Dice Create(EDice dice)
        {
            switch (dice)
            {
                case EDice.D4:
                    return new D4();
                case EDice.D6:
                    return new D6();
                case EDice.D8:
                    return new D8();
                case EDice.D10:
                    return new D10();
                case EDice.D12:
                    return new D12();
                default:
                    throw new Exception("Wrong Dice type.");
            }
        }

        public Dice Create(XmlNode node)
        {
            if (node.Name != "Dice")
            {
                throw new Exception("Wrong node name.");
            }
            switch (node.InnerText)
            {
                case "D4":
                    return new D4();
                case "D6":
                    return new D6();
                case "D8":
                    return new D8();
                case "D10":
                    return new D10();
                case "D12":
                    return new D12();
                default:
                    throw new Exception("Wrong Dice type.");
            }
        }
    }
}
