using Persistance;
using SerializationModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace Serializing
{
    [Export(typeof(IPersister))]
    public class XmlModelSerializer : IPersister
    {
        private DataContractSerializer dataContractSerializer; 

        public Stream SerializationStream { get; set; }
        private Type NodeType { get; set;  }
        private IEnumerable<Type> KnownTypes { get; set; }
        private string _target;
        public string Target { get => _target; set => SetTarget(value); }

        private void SetTarget(string value)
        {
            this._target = value;
            SerializationStream = new FileStream(value, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        public XmlModelSerializer()
        {
            NodeType = typeof(SerializationAssemblyMetadata);
            KnownTypes = new Type[]
            {
                typeof(AbstractMapper), typeof(SerializationAttributeMetadata), typeof(SerializationMethodMetadata),
                typeof(SerializationNamespaceMetadata), typeof(SerializationParameterMetadata),
                typeof(SerializationPropertyMetadata), typeof(SerializationTypeMetadata)
            };

            dataContractSerializer = new DataContractSerializer(
                type: NodeType,
                knownTypes: KnownTypes,
                maxItemsInObjectGraph: 100000,
                ignoreExtensionDataObject: false,
                preserveObjectReferences: true,
                dataContractSurrogate: null);
            if(Target != null)
                SerializationStream = new FileStream(Target, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }
        public XmlModelSerializer(Stream inStream) : this()
        {
            SerializationStream = inStream;
        }

        public void Save(object toSave)
        {
            var settings = new XmlWriterSettings { Indent = true };
            if(SerializationStream != null)
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
            if (SerializationStream != null)
            {
                SerializationStream.Position = 0;
                using (XmlReader reader = XmlReader.Create(SerializationStream))
                {
                    read = dataContractSerializer.ReadObject(reader);
                }
            }
            return read;
        }

        public void Dispose()
        {
            SerializationStream.Dispose();
        }
    }
}
