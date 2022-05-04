using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data
{
    public class XmlParser
    {
        private XmlDocument document;

        public XmlParser(string data)
        {
            document = new XmlDocument();
            document.LoadXml(data);
        }

        public List<string> Parse(string type)
        {
            var data = new List<string>();
            var root = document.FirstChild;
            if (root.HasChildNodes)
            {
                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                    if (root.ChildNodes[i].Name == type)
                    {
                        data.Add(root.ChildNodes[i].InnerText);
                    }
                }
            }
            return data;
        }
    }
}
