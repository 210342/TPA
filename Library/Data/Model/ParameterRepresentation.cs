using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Library.Data.Model
{
    internal class ParameterRepresentation : IRepresentation
    {
        #region Properties
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public TypeRepresentation Type { get; private set; }
        public IEnumerable<IRepresentation> Children
        {
            get
            {
                yield return Type;
            }
        }
        public string ToStringProperty
        {
            get
            {
                return ToString();
            }
        }
        #endregion
        
        public ParameterRepresentation(TypeRepresentation type, string methodName, ParameterInfo parameter)
        {
            Name = parameter.Name;
            FullName = ExpectedFullName(methodName, parameter);
            Type = type;
        }

        #region Methods
        public static string ExpectedFullName(string methodName, ParameterInfo parameter)
        {
            return $"{methodName}.{parameter.Name}";
        }

        public static string PrintParametersHumanReadable(IEnumerable<ParameterRepresentation> parameters)
        {
            if (parameters.Count() != 0)
            {
                StringBuilder sb = new StringBuilder("(");
                foreach (ParameterRepresentation parameter in parameters)
                {
                    sb.Append($"{parameter.Type.Name} {parameter.Name}, ");
                }
                sb.Remove(sb.Length - 2, 2); // remove last comma and space
                sb.Append(")");
                return sb.ToString();
            }
            else
            {
                return "()";
            }
        }

        public IEnumerable<string> Print()
        {
            yield return $"NAME: {Name}";
            yield return $"Type: {Type.Name}";
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Print());
        }
        #endregion
    }
}