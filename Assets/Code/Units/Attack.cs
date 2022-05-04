using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.Units
{
    [Serializable]
    public class Attack: Utility.IXmlNode
    {
        [HideInInspector]
        public int Piercing { get; set; }
        [HideInInspector]
        public int Breakthrough { get; set; }
        [HideInInspector]
        public int Terror { get; set; }
        [HideInInspector]
        public int ManpowerAttackBonus { get; set; }
        [HideInInspector]
        public int CohesionAttackBonus { get; set; }
        [HideInInspector]
        public List<Dices.Dice> ManpowerAttackDices { get; set; } = new List<Dices.Dice>();
        [HideInInspector]
        public List<Dices.Dice> CohesionAttackDices { get; set; } = new List<Dices.Dice>();

        private Dices.DiceFactory diceFactory = new Dices.DiceFactory();

        public XmlNode ToXmlNode(XmlDocument document)
        {
            var attackNode = document.CreateNode(XmlNodeType.Element, "Attack", null);

            var piercingNode = document.CreateNode(XmlNodeType.Element, "Piercing", null);
            piercingNode.InnerText = Piercing.ToString();
            attackNode.AppendChild(piercingNode);

            var breakthroughNode = document.CreateNode(XmlNodeType.Element, "Breakthrough", null);
            breakthroughNode.InnerText = Breakthrough.ToString();
            attackNode.AppendChild(breakthroughNode);

            var terrorNode = document.CreateNode(XmlNodeType.Element, "Terror", null);
            terrorNode.InnerText = Terror.ToString();
            attackNode.AppendChild(terrorNode);
            
            var manpowerAttackBonusNode = document.CreateNode(XmlNodeType.Element, "ManpowerAttackBonus", null);
            manpowerAttackBonusNode.InnerText = ManpowerAttackBonus.ToString();
            attackNode.AppendChild(manpowerAttackBonusNode);

            var cohesionAttackBonusNode = document.CreateNode(XmlNodeType.Element, "CohesionAttackBonus", null);
            cohesionAttackBonusNode.InnerText = CohesionAttackBonus.ToString();
            attackNode.AppendChild(cohesionAttackBonusNode);

            var manpowerAttackDicesNode = document.CreateNode(XmlNodeType.Element, "ManpowerAttackDices", null);
            foreach(var dice in ManpowerAttackDices)
            {
                manpowerAttackDicesNode.AppendChild(dice.ToXmlNode(document));
            }
            attackNode.AppendChild(manpowerAttackDicesNode);

            var cohesionAttackDicesNode = document.CreateNode(XmlNodeType.Element, "CohesionAttackDices", null);
            foreach (var dice in CohesionAttackDices)
            {
                cohesionAttackDicesNode.AppendChild(dice.ToXmlNode(document));
            }
            attackNode.AppendChild(cohesionAttackDicesNode);

            return attackNode;
        }
    
        public bool LoadFromXmlNode(XmlNode node)
        {
            if(node.Name!= "Attack")
            {
                return false;
            }
            ManpowerAttackDices.Clear();
            CohesionAttackDices.Clear();
            foreach (XmlNode child in node.ChildNodes)
            {

                switch (child.Name)
                {
                    case "Piercing":
                        Piercing = int.Parse(child.InnerText);
                        break;
                    case "Breakthrough":
                        Breakthrough = int.Parse(child.InnerText);
                        break;
                    case "Terror":
                        Terror = int.Parse(child.InnerText);
                        break;
                    case "ManpowerAttackBonus":
                        ManpowerAttackBonus = int.Parse(child.InnerText);
                        break;
                    case "CohesionAttackBonus":
                        CohesionAttackBonus = int.Parse(child.InnerText);
                        break;
                    case "ManpowerAttackDices":
                        foreach(XmlNode dice in child.ChildNodes)
                        {
                            ManpowerAttackDices.Add(diceFactory.Create(dice));
                        }
                        break;
                    case "CohesionAttackDices":
                        foreach (XmlNode dice in child.ChildNodes)
                        {
                            CohesionAttackDices.Add(diceFactory.Create(dice));
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;
        }
    }
}
