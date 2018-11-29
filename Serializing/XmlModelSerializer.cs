using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Serializing
{
    [Export(typeof(IPersister))]
    public class XmlModelSerializer : IPersister
    {
        private DataContractSerializer dcs; 
        private string sourceName;

        public Stream SerializationStream { get; set; }
        public string SourceName
        {
            get
            {
                return sourceName;
            }
            set
            {
                sourceName = value;
                if(SerializationStream == null)
                {
                    SerializationStream = new FileStream(SourceName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                }
            }
        }

        public XmlModelSerializer(List<Type> knownTypes, Type nodeType)
        {
            dcs = new DataContractSerializer(
                type: nodeType, 
                knownTypes: knownTypes,
                maxItemsInObjectGraph: 100000,
                ignoreExtensionDataObject: false,
                preserveObjectReferences: true,
                dataContractSurrogate: null);
        }

        public void Save(object toSave)
        {
            var settings = new XmlWriterSettings { Indent = true };
            if(SerializationStream != null)
            {
                using (XmlWriter writer = XmlWriter.Create(SerializationStream, settings))
                {
                    dcs.WriteObject(writer, toSave);
                }
            }
            else
            {
                using (XmlWriter writer = XmlWriter.Create(SourceName, settings))
                {
                    dcs.WriteObject(writer, toSave);
                }
            }
        }

        public object Load()
        {
            object read = null;
            if (SerializationStream != null)
            {
                using (XmlReader reader = XmlReader.Create(SerializationStream))
                {
                    read = dcs.ReadObject(reader);
                }
            }
            else
            {
                using (XmlReader reader = XmlReader.Create(SourceName))
                {
                    read = dcs.ReadObject(reader);
                }
            }
            return read;
        }
    }
}
