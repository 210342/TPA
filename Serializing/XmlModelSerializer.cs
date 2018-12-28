using Persistance;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Serializing
{
    [Export(typeof(IPersister))]
    public class XmlModelSerializer : ISerializer
    {
        private DataContractSerializer dataContractSerializer; 
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
                SerializationStream = new FileStream(SourceName, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
        }
        public Type NodeType { get; set;  }
        public bool IsInitialised { get; private set; } = false;
        public IEnumerable<Type> KnownTypes { get; set; }

        /*
         * TODO:
         * Create new model for serialization  
         * After that serialization using plugin will be possible
         * Because we won't have to provide known classes for serializer.
         */
        public XmlModelSerializer()
        {
        }

        public void Save(object toSave)
        {
            var settings = new XmlWriterSettings { Indent = true };
            if(SerializationStream != null && IsInitialised)
            {
                SerializationStream.Position = 0;
                using (XmlWriter writer = XmlWriter.Create(SerializationStream, settings))
                {
                    dataContractSerializer.WriteObject(writer, toSave);
                }
                SerializationStream.FlushAsync();
            }
        }

        public object Load()
        {
            object read = null;
            if (SerializationStream != null && IsInitialised)
            {
                SerializationStream.Position = 0;
                using (XmlReader reader = XmlReader.Create(SerializationStream))
                {
                    read = dataContractSerializer.ReadObject(reader);
                }
            }
            return read;
        }

        public void InitialiseSerialization()
        {
            dataContractSerializer = new DataContractSerializer(
                type: NodeType,
                knownTypes: KnownTypes,
                maxItemsInObjectGraph: 100000,
                ignoreExtensionDataObject: false,
                preserveObjectReferences: true,
                dataContractSurrogate: null);
            IsInitialised = true;
        }
    }
}
