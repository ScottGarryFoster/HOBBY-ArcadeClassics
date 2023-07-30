using System;

namespace FQ.Libraries
{
    [Serializable]
    public class InvalidCallOrder : Exception
    {
        public InvalidCallOrder() {  }

        public InvalidCallOrder(string name = "")
            : base(name)
        {

        }
    }
}