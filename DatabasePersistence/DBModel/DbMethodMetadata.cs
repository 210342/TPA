using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ModelContract;

namespace DatabasePersistence.DBModel
{
    public class DbMethodMetadata : AbstractMapper, IMethodMetadata
    {
        public DbMethodMetadata(IMethodMetadata methodMetadata)
        {
            Name = methodMetadata.Name;
            SavedHash = methodMetadata.SavedHash;
            IsExtension = methodMetadata.IsExtension;
            Modifiers = methodMetadata.Modifiers;

            // Generic Arguments
            if (methodMetadata.GenericArguments is null)
            {
                GenericArguments = null;
            }
            else
            {
                List<ITypeMetadata> genericArguments = new List<ITypeMetadata>();
                foreach (ITypeMetadata genericArgument in methodMetadata.GenericArguments)
                    if (AlreadyMapped.TryGetValue(genericArgument.SavedHash, out IMetadata mappedArgument))
                    {
                        genericArguments.Add(mappedArgument as ITypeMetadata);
                    }
                    else
                    {
                        genericArguments.Add(new DbTypeMetadata(genericArgument.SavedHash, genericArgument.Name));
                    }

                GenericArguments = genericArguments;
            }

            // Return type
            if (AlreadyMapped.TryGetValue(methodMetadata.ReturnType.SavedHash, out IMetadata item))
            {
                ReturnType = item as ITypeMetadata;
            }
            else
            {
                ReturnType = new DbTypeMetadata(methodMetadata.ReturnType.SavedHash, methodMetadata.ReturnType.Name);
            }

            // Parameters
            if (methodMetadata.Parameters is null)
            {
                Parameters = Enumerable.Empty<IParameterMetadata>();
            }
            else
            {
                List<IParameterMetadata> parameters = new List<IParameterMetadata>();
                foreach (IParameterMetadata parameter in methodMetadata.Parameters)
                    if (AlreadyMapped.TryGetValue(parameter.SavedHash, out item))
                    {
                        parameters.Add(item as IParameterMetadata);
                    }
                    else
                    {
                        IParameterMetadata newParameter = new DbParameterMetadata(parameter);
                        parameters.Add(newParameter);
                        AlreadyMapped.Add(newParameter.SavedHash, newParameter);
                    }

                Parameters = parameters;
            }

            FillChildren();
        }

        public DbMethodMetadata() { }

        private void FillChildren()
        {
            List<IMetadata> elems = new List<IMetadata> {ReturnType};
            elems.AddRange(Parameters);
            Children = elems;
        }

        #region EF

        public virtual ICollection<DbTypeMetadata> GenericArgumentsList { get; set; }
        public virtual ICollection<DbParameterMetadata> ParametersList { get; set; }

        #endregion

        #region IMethodMetadata

        [ForeignKey("TypesId")]
        public virtual ITypeMetadata ReturnType { get; private set; }
        public bool IsExtension { get; }
        public Tuple<AccessLevelEnum, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; }
        public string Name { get; set; }
        public int SavedHash { get; protected set; }

        [NotMapped]
        public IEnumerable<ITypeMetadata> GenericArguments
        {
            get => GenericArgumentsList;
            private set => GenericArgumentsList = value?.Cast<DbTypeMetadata>().ToList();
        }

        [NotMapped]
        public IEnumerable<IParameterMetadata> Parameters
        {
            get => ParametersList;
            internal set => ParametersList = value?.Cast<DbParameterMetadata>().ToList();
        }

        [NotMapped] public IEnumerable<IMetadata> Children { get; private set; }

        public void MapTypes()
        {
            if (ReturnType != null && string.IsNullOrEmpty(ReturnType.Name)
                && AlreadyMapped.TryGetValue(ReturnType.SavedHash, out IMetadata item))
            {
                ReturnType = item as ITypeMetadata;
            }
            if (GenericArguments != null)
            {
                ICollection<ITypeMetadata> actualGenericArguments = new List<ITypeMetadata>();
                foreach (ITypeMetadata type in GenericArguments)
                {
                    if (string.IsNullOrEmpty(type.Name) && AlreadyMapped.TryGetValue(type.SavedHash, out item))
                    {
                        actualGenericArguments.Add(item as ITypeMetadata);
                    }
                    else
                    {
                        actualGenericArguments.Add(type);
                    }
                }
                GenericArguments = actualGenericArguments;
            }
            foreach (IParameterMetadata parameter in Parameters)
            {
                parameter.MapTypes();
            }
        }

        #endregion
    }
}