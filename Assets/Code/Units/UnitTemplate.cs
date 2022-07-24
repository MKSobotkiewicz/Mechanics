using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.IO; 
using UnityEngine;
using UnityEditor;


namespace Project.Units
{
    [Serializable]
    public class UnitTemplate : MonoBehaviour ,Utility.IXmlNode
    {
        [HideInInspector]
        public UnityEngine.Material UnitMaterial;
        [HideInInspector]
        public bool Enchancement = false;
        [HideInInspector]
        public Attack Attack = new Attack();
        [HideInInspector]
        public Defense Defense = new Defense();
        [HideInInspector]
        public float Speed;
        [HideInInspector]
        public bool IgnoreTerrain;
        [HideInInspector]
        public int MaxManpower = 100;
        [HideInInspector]
        public int MaxCohesion = 100;
        [HideInInspector]
        public int MaxSupply = 100;

        public XmlNode ToXmlNode(XmlDocument document)
        {
            var unitTemplateNode = document.CreateNode(XmlNodeType.Element, "UnitTemplate", null);

            var iconNode = document.CreateNode(XmlNodeType.Element, "UnitMaterial", null);
            if (UnitMaterial != null)
            {
                iconNode.InnerText = AssetDatabase.GetAssetPath(UnitMaterial).Replace("Assets/Resources/","").Replace(".mat", "");
            }
            unitTemplateNode.AppendChild(iconNode);

            var enchancementNode = document.CreateNode(XmlNodeType.Element, "Enchancement", null);
            enchancementNode.InnerText = Enchancement.ToString();
            unitTemplateNode.AppendChild(enchancementNode);
            
            unitTemplateNode.AppendChild(Attack.ToXmlNode(document));

            unitTemplateNode.AppendChild(Defense.ToXmlNode(document));

            var speedNode = document.CreateNode(XmlNodeType.Element, "Speed", null);
            speedNode.InnerText = Speed.ToString();
            unitTemplateNode.AppendChild(speedNode);

            var ignoreTerrainNode = document.CreateNode(XmlNodeType.Element, "IgnoreTerrain", null);
            ignoreTerrainNode.InnerText = IgnoreTerrain.ToString();
            unitTemplateNode.AppendChild(ignoreTerrainNode);

            var maxManpowerNode = document.CreateNode(XmlNodeType.Element, "MaxManpower", null);
            maxManpowerNode.InnerText = MaxManpower.ToString();
            unitTemplateNode.AppendChild(maxManpowerNode);

            var maxCohesionNode = document.CreateNode(XmlNodeType.Element, "MaxCohesion", null);
            maxCohesionNode.InnerText = MaxCohesion.ToString();
            unitTemplateNode.AppendChild(maxCohesionNode);

            var maxSupplyNode = document.CreateNode(XmlNodeType.Element, "MaxSupply", null);
            maxSupplyNode.InnerText = MaxSupply.ToString();
            unitTemplateNode.AppendChild(maxSupplyNode);

            return unitTemplateNode;
        }

        public bool SaveAsXml()
        {
            var appPath = Application.dataPath;
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false
            };
            using (var xmlWriter = XmlWriter.Create(appPath.Substring(0,appPath.Length - 6) + AssetDatabase.GetAssetPath(this).Replace("prefab","xml"), settings))
            {
                //Debug.Log(appPath.Substring(0,appPath.Length - 6) + AssetDatabase.GetAssetPath(this).Replace("prefab", "xml"));
                XmlDocument document = new XmlDocument();
                ToXmlNode(document).WriteTo(xmlWriter);
            }
            return true;
        }

        public bool LoadFromXmlNode(XmlNode node)
        {
            if (node.Name != "UnitTemplate")
            {
                return false;
            }
            foreach (XmlNode child in node.ChildNodes)
            {
                switch (child.Name)
                {
                    case "UnitMaterial":
                        UnitMaterial = UnityEngine.Resources.Load<UnityEngine.Material>(child.InnerText);
                        break;
                    case "Enchancement":
                        Enchancement = bool.Parse(child.InnerText);
                        break;
                    case "Attack":
                        Attack.LoadFromXmlNode(child);
                        break;
                    case "Defense":
                        Defense.LoadFromXmlNode(child);
                        break;
                    case "Speed":
                        Speed = float.Parse(child.InnerText);
                        break;
                    case "IgnoreTerrain":
                        IgnoreTerrain = bool.Parse(child.InnerText);
                        break;
                    case "MaxManpower":
                        MaxManpower = int.Parse(child.InnerText);
                        break;
                    case "MaxCohesion":
                        MaxCohesion = int.Parse(child.InnerText);
                        break;
                    case "MaxSupply":
                        MaxSupply = int.Parse(child.InnerText);
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        public bool LoadFromXml()
        {
            var appPath = Application.dataPath;
            var path = (appPath.Substring(0, appPath.Length - 6) + AssetDatabase.GetAssetPath(this).Replace("prefab", "xml"));
            if (!File.Exists(path))
            {
                return false;
            }
            var document = new XmlDocument();
            document.Load(path);
            XmlNode node = document.DocumentElement.SelectSingleNode("/UnitTemplate");
            return LoadFromXmlNode(node);
        }
    }
}
