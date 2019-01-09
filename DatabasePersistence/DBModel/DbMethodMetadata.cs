﻿using ModelContract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DatabasePersistence.DBModel
{
    public class DbMethodMetadata : AbstractMapper, IMethodMetadata
    {
        #region EF
        public ICollection<DbTypeMetadata> GenericArgumentsList
        {
            get => GenericArguments?.Cast<DbTypeMetadata>().ToList();
            set => GenericArguments = value;
        }
        public ICollection<DbParameterMetadata> ParametersList
        {
            get => Parameters?.Cast<DbParameterMetadata>().ToList();
            set => Parameters = value;
        }
        #endregion

        #region IMethodMetadata

        public ITypeMetadata ReturnType { get; private set; }
        public bool IsExtension { get; private set; }
        public Tuple<AccessLevelEnum, AbstractEnum, StaticEnum, VirtualEnum> Modifiers { get; private set; }
        public string Name { get; set; }
        public int SavedHash { get; protected set; }
        [NotMapped]
        public IEnumerable<ITypeMetadata> GenericArguments { get; private set; }
        [NotMapped]
        public IEnumerable<IParameterMetadata> Parameters { get; private set; }
        [NotMapped]
        public IEnumerable<IMetadata> Children { get; private set; }

        #endregion

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
                {
                    if (AlreadyMapped.TryGetValue(genericArgument.SavedHash, out IMetadata mappedArgument))
                    {
                        genericArguments.Add(mappedArgument as ITypeMetadata);
                    }
                    else
                    {
                        ITypeMetadata newType = new DbTypeMetadata(genericArgument);
                        genericArguments.Add(newType);
                        AlreadyMapped.Add(newType.SavedHash, newType);
                    }
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
                ITypeMetadata newType = new DbTypeMetadata(methodMetadata.ReturnType);
                ReturnType = newType;
                AlreadyMapped.Add(newType.SavedHash, newType);
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
                {
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
                }
                Parameters = parameters;
            }

            FillChildren();
        }

        public DbMethodMetadata()
        {
        }

        private void FillChildren()
        {
            List<IMetadata> elems = new List<IMetadata> { ReturnType };
            elems.AddRange(Parameters);
            Children = elems;
        }
    }
}
