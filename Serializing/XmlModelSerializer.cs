using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using ModelContract;
using Persistence;
using SerializationModel;

namespace Serializing
{
    [Export(typeof(IPersister))]
    public class XmlModelSerializer : IPersister
    {
        private readonly DataContractSerializer dataContractSerializer;

        public Stream SerializationStream { get; set; }
        private Type NodeType { get; }
        private IEnumerable<Type> KnownTypes { get; }

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

        public void Access(string target)
        {
            if (!string.IsNullOrEmpty(target))
            {
                SerializationStream?.Dispose();
                SerializationStream = new FileStream(target, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            }
        }

        public async Task Save(IAssemblyMetadata toSave)
        {
            SerializationAssemblyMetadata graph = toSave as SerializationAssemblyMetadata
                    ?? new SerializationAssemblyMetadata(toSave);
            XmlWriterSettings settings = new XmlWriterSettings {Indent = true};
            if (SerializationStream != null)
            {
                SerializationStream.Position = 0;
                using (XmlWriter writer = XmlWriter.Create(SerializationStream, settings))
                {
                    dataContractSerializer.WriteObject(writer, graph);
                }

                await SerializationStream.FlushAsync();
            }
        }

        public Task<IAssemblyMetadata> Load()
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
            return Task.FromResult(read as IAssemblyMetadata);
        }

        public void Dispose()
        {
            SerializationStream?.Dispose();
        }
    }
}