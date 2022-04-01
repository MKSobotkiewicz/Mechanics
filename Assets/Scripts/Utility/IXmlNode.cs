using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Project.Utility
{
    public interface IXmlNode
    {
        XmlNode ToXmlNode(XmlDocument document);
        bool LoadFromXmlNode(XmlNode node);
    }
}
