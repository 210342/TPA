using ModelContract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabasePersistence.DBModel
{
    public class DbMethodMetadata : AbstractMapper, IMethodMetadata
    {
        #region IMethodMetadata

        public string Name { get; set; }
        public int SavedHash { get; set; }
        public bool IsExtension { get; set; }
        [NotMapped]
        public Tuple<AccessLevelEnum, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; set; }
        [NotMapped]
        public ITypeMetadata ReturnType
        {
            get => EFReturnType;
            set => EFReturnType = value as DbTypeMetadata;
        }
        [NotMapped]
        public IEnumerable<ITypeMetadata> GenericArguments
        {
            get => EFGenericArguments;
            set => EFGenericArguments = value?.Cast<DbTypeMetadata>().ToList();
        }
        [NotMapped]
        public IEnumerable<IParameterMetadata> Parameters
        {
            get => EFParameters;
            set => EFParameters = value?.Cast<DbParameterMetadata>().ToList();
        }
        [NotMapped]
        public IEnumerable<IMetadata> Children => throw new NotImplementedException();
        #endregion

        #region EF

        public bool IsAbstract { get; set; }
        public bool IsStatic { get; set; }
        public bool IsVirtual { get; set; }
        public AccessLevelEnum AccessLevel { get; set; }
        public DbTypeMetadata EFReturnType { get; set; }
        public ICollection<DbTypeMetadata> EFGenericArguments { get; set; }
        public ICollection<DbParameterMetadata> EFParameters { get; set; }

        #endregion

        public DbMethodMetadata() { }

        public DbMethodMetadata(IMethodMetadata methodMetadata)
        {
            Name = methodMetadata.Name;
            SavedHash = methodMetadata.SavedHash;

            //Modifiers
            Modifiers = methodMetadata.Modifiers;
            AccessLevel = Modifiers.Item1;
            IsAbstract = Modifiers.Item2.Equals(AbstractEnum.Abstract);
            IsStatic = Modifiers.Item3.Equals(StaticEnum.Static);
            IsVirtual = Modifiers.Item4.Equals(VirtualEnum.Virtual);
            IsExtension = methodMetadata.IsExtension;    

            // Generic Arguments
            if (methodMetadata.GenericArguments is null)
            {
                EFGenericArguments = null;
            }
            else
            {
                List<DbTypeMetadata> genericArguments = new List<DbTypeMetadata>();
                foreach (ITypeMetadata genericArgument in methodMetadata.GenericArguments)
                    if (AlreadyMappedTypes.TryGetValue(
                        genericArgument.SavedHash, out DbTypeMetadata mappedArgument))
                    {
                        genericArguments.Add(mappedArgument);
                    }
                    else
                    {
                        genericArguments.Add(new DbTypeMetadata(genericArgument.SavedHash, genericArgument.Name));
                    }

                EFGenericArguments = genericArguments;
            }

            // Return type
            if (methodMetadata.ReturnType is null)
            {
                EFReturnType = null;
            }
            else
            {
                if (AlreadyMappedTypes.TryGetValue(
                    methodMetadata.ReturnType.SavedHash, out DbTypeMetadata item))
                {
                    EFReturnType = item;
                }
                else
                {
                    EFReturnType = new DbTypeMetadata(methodMetadata.ReturnType.SavedHash, methodMetadata.ReturnType.Name);
                }
            }
            // Parameters
            if (methodMetadata.Parameters is null)
            {
                EFParameters = Enumerable.Empty<DbParameterMetadata>().ToList();
            }
            else
            {
                List<DbParameterMetadata> parameters = new List<DbParameterMetadata>();
                foreach (IParameterMetadata parameter in methodMetadata.Parameters)
                    if (AlreadyMappedParameters.TryGetValue(
                        parameter.SavedHash, out DbParameterMetadata item))
                    {
                        parameters.Add(item);
                    }
                    else
                    {
                        DbParameterMetadata newParameter = new DbParameterMetadata(parameter);
                        parameters.Add(newParameter);
                        AlreadyMappedParameters.Add(newParameter.SavedHash, newParameter);
                    }

                EFParameters = parameters;
            }
        }

        public void MapTypes()
        {
            if(EFReturnType != null)
            {
                if (AlreadyMappedTypes.TryGetValue(
                    EFReturnType.SavedHash, out DbTypeMetadata item))
                {
                    EFReturnType = item;
                }
                else
                {
                    AlreadyMappedTypes.Add(EFReturnType.SavedHash, EFReturnType);
                }
            }

            if (EFGenericArguments != null)
            {
                ICollection<DbTypeMetadata> actualGenericArguments = new List<DbTypeMetadata>();
                foreach (DbTypeMetadata type in EFGenericArguments)
                {
                    if (AlreadyMappedTypes.TryGetValue(type.SavedHash, out DbTypeMetadata item))
                    {
                        actualGenericArguments.Add(item);
                    }
                    else
                    {
                        actualGenericArguments.Add(type);
                        AlreadyMappedTypes.Add(type.SavedHash, type);
                    }
                }
                EFGenericArguments = actualGenericArguments;
            }
            foreach (DbParameterMetadata parameter in EFParameters)
            {
                parameter.MapTypes();
            }
        }
    }
}
