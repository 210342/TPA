using System.Collections.Generic;

namespace Library.Logic
{
    public class ClassRepresentation
    {
        public string ClassName { get; private set; }
        public List<string> ClassProperties { get; private set; }
        public List<string> ClassMethods { get; private set; }
        public List<string> ClassAttributes { get; private set; }
        public List<string> ClassFields { get; private set; }

        public ClassRepresentation(string className, List<string> classProp, List<string> classAtt,
            List<string> classMeth, List<string> classFields)
        {
            ClassName = className;
            ClassAttributes = classAtt;
            ClassFields = classFields;
            ClassProperties = classProp;
            ClassMethods = classMeth;
        }
        public ClassRepresentation()
        {

        }

        public override string ToString()
        {
            return ClassName;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType().Equals(this.GetType()))
            {
                return ClassName.Equals(((ClassRepresentation)obj).ClassName);

            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
            // TODO
        }
    }
}
