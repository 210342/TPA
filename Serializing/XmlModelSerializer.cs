using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Serializing
{
    [Export(typeof(IPersister))]
    public class XmlModelSerializer : IPersister
    {
        DataContractSerializer dcs; 
        public string SourceName { get; set; }

        public XmlModelSerializer(List<Type> knownTypes, Type nodeType)
        {
            dcs = new DataContractSerializer(nodeType, knownTypes, 100000, false, true, null);
        }
        public void Save(object toSave)
        {
            var settings = new System.Xml.XmlWriterSettings { Indent = true };
            using (var w = System.Xml.XmlWriter.Create(SourceName, settings))
            {
                dcs.WriteObject(w, toSave);
            }
                
        }

        public object Load()
        {
            object read = null;
            using (var r = System.Xml.XmlReader.Create(SourceName))
                read = dcs.ReadObject(r);
            return read;
        }
    }
}
