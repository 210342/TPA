using ModelContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dbp.Model
{
    public class DbMethodMetadata : AbstractMapper
    {
        public string Name { get; set; }
        public int SavedHash { get; set; }
        public DbTypeMetadata ReturnType { get; set; }
        public ICollection<DbTypeMetadata> GenericArguments { get; set; }
        public ICollection<DbParameterMetadata> Parameters { get; set; }

        public DbMethodMetadata(IMethodMetadata methodMetadata)
        {
            Name = methodMetadata.Name;
            SavedHash = methodMetadata.SavedHash;

            // Generic Arguments
            if (methodMetadata.GenericArguments is null)
            {
                GenericArguments = null;
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

                GenericArguments = genericArguments;
            }

            // Return type
            if (methodMetadata.ReturnType is null)
            {
                ReturnType = null;
            }
            else
            {
                if (AlreadyMappedTypes.TryGetValue(
                    methodMetadata.ReturnType.SavedHash, out DbTypeMetadata item))
                {
                    ReturnType = item;
                }
                else
                {
                    ReturnType = new DbTypeMetadata(methodMetadata.ReturnType.SavedHash, methodMetadata.ReturnType.Name);
                }
            }
            // Parameters
            if (methodMetadata.Parameters is null)
            {
                Parameters = Enumerable.Empty<DbParameterMetadata>().ToList();
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

                Parameters = parameters;
            }
        }

        public void MapTypes()
        {
            if(ReturnType != null)
            {
                if (AlreadyMappedTypes.TryGetValue(
                    ReturnType.SavedHash, out DbTypeMetadata item))
                {
                    ReturnType = item;
                }
                else
                {
                    AlreadyMappedTypes.Add(ReturnType.SavedHash, ReturnType);
                }
            }

            if (GenericArguments != null)
            {
                ICollection<DbTypeMetadata> actualGenericArguments = new List<DbTypeMetadata>();
                foreach (DbTypeMetadata type in GenericArguments)
                {
                    if (string.IsNullOrEmpty(type.Name) 
                        && AlreadyMappedTypes.TryGetValue(type.SavedHash, out DbTypeMetadata item))
                    {
                        actualGenericArguments.Add(item);
                    }
                    else
                    {
                        actualGenericArguments.Add(type);
                        AlreadyMappedTypes.Add(type.SavedHash, type);
                    }
                }
                GenericArguments = actualGenericArguments;
            }
            foreach (DbParameterMetadata parameter in Parameters)
            {
                parameter.MapTypes();
            }
        }
    }
}
