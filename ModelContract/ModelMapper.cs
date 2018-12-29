using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ModelContract
{
    public class ModelMapper
    {
        Dictionary<IMetadata, IMetadata> filtered = new Dictionary<IMetadata, IMetadata>();
        IEnumerable<Type> typesMappedTo;

        public IEnumerable<IMetadata> MapModel(HashSet<IMetadata> data, ICollection<IMetadata> outCollection, Assembly model)
        {
            typesMappedTo = GetAllTypesImplementingContract(typeof(IMetadata), model);
            filtered.Clear();
            foreach (var element in data)
            {
                IMetadata pSerial = Create(element);
                var pChildList = new List<IMetadata>();
                if (element.Children != null)
                {
                    foreach (var pChild in element.Children)
                    {
                        IMetadata filteredChild = GetFiltered(data, pChild);
                        if (filteredChild != null)
                            pChildList.Add(filteredChild);
                    }
                }
                pSerial.Children = pChildList;
                outCollection.Add(pSerial);
            }
            return outCollection;
        }

        private IMetadata GetFiltered(HashSet<IMetadata> data, IMetadata pChild)
        {
            if (!data.TryGetValue(pChild, out IMetadata takenOut))
            {
                return null;

            }
            else if (filtered.TryGetValue(pChild, out IMetadata filteredKid))
            {
                return filteredKid;
            }
            else
            {
                IMetadata newContract = Create(takenOut);
                filtered.Add(pChild, newContract);
                return newContract;
            }
        }

        public IMetadata Create(IMetadata obj)
        {
            IMetadata newContract = null;
            foreach (Type contract in typesMappedTo)
            {
                if (GetClosestInterface(obj.GetType()).IsAssignableFrom(contract))
                {
                    MethodInfo create = typeof(ModelMapper).GetMethod("CreateInstance");
                    create = create.MakeGenericMethod(GetClosestInterface(contract));
                    newContract = (IMetadata)create.Invoke(this, new object[] { contract, obj });
                    return newContract;
                }
            }
            return newContract;
        }

        public IMetadata Create(IMetadata obj, IEnumerable<Type> model)
        {
            IMetadata newContract = null;
            foreach (Type contract in model)
            {
                if (GetClosestInterface(obj.GetType()).IsAssignableFrom(contract))
                {
                    MethodInfo create = typeof(ModelMapper).GetMethod("CreateInstance");
                    create = create.MakeGenericMethod(GetClosestInterface(contract));
                    newContract = (IMetadata)create.Invoke(this, new object[] { contract, obj });
                    return newContract;
                }
            }
            return newContract;
        }

        public IMetadata CreateInstance<T>(Type type, IMetadata obj) where T : class
        {
            return (IMetadata)Activator.CreateInstance(type, (T)obj);
        }

        public Type GetClosestInterface(Type type)
        {
            Type typeInterface = null;
            type.GetInterfaces().ToList().ForEach(n => { if (!n.Equals(typeof(IMetadata))) typeInterface = n; });
            return typeInterface;
        }

        public IEnumerable<Type> GetAllContractsConnectedWith(Type contract)
        {
            return from type in contract.Assembly.GetTypes()
                   where contract.IsAssignableFrom(type) && !type.Equals(contract)
                   select type;
        }

        public IEnumerable<Type> GetAllTypesImplementingContract(Type contract, Assembly model)
        {
            return from type in model.GetTypes()
                   where contract.IsAssignableFrom(type) && !type.Equals(contract) && !type.IsInterface
                   select type;
        }
    }
}
