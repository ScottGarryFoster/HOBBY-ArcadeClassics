using System;

namespace FQ.Libraries
{
    [Serializable]
    public class InvalidParameter : Exception
    {
        public InvalidParameter() {  }

        public InvalidParameter(string name = "")
            : base(name)
        {

        }
    }
}