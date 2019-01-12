using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Persistance;
using SerializationModel;

namespace Serializing
{
    [Export(typeof(IPersister))]
    public class XmlModelSerializer : IPersister
    {
        private readonly DataContractSerializer dataContractSerializer;
        private string _target;

        public Stream SerializationStream { get; set; }
        private Type NodeType { get; set; }
        private IEnumerable<Type> KnownTypes { get; set; }

        public string Target
        {
            get { return _target; }
            set
            {
                _target = value;
                SerializationStream?.Dispose();
                SerializationStream = new FileStream(value, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
        }

        public FileSystemDependency FileSystemDependency => FileSystemDependency.Dependent;

        public XmlModelSerializer()
        {
            NodeType = typeof(SerializationAssemblyMetadata);
            KnownTypes = new[]
            {
                typeof(AbstractMapper), typeof(SerializationAttributeMetadata), typeof(SerializationMethodMetadata),
                typeof(SerializationNamespaceMetadata), typeof(SerializationParameterMetadata),
                typeof(SerializationPropertyMetadata), typeof(SerializationTypeMetadata)
            };

            dataContractSerializer = new DataContractSerializer(
                NodeType,
                KnownTypes,
                100000,
                false,
                true,
                null);
        }

        public XmlModelSerializer(Stream inStream) : this()
        {
            SerializationStream = inStream;
        }

        public void Save(object toSave)
        {
            XmlWriterSettings settings = new XmlWriterSettings {Indent = true};
            if (SerializationStream != null)
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
            try
            {
                if (SerializationStream != null)
                {
                    SerializationStream.Position = 0;
                    using (XmlReader reader = XmlReader.Create(SerializationStream))
                    {
                        read = dataContractSerializer.ReadObject(reader);
                    }
                }
            }
            catch (SerializationException)
            {
                return null;
            }
            return read;
        }

        public void Dispose()
        {
            SerializationStream?.Dispose();
        }
    }
}