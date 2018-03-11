using heitech.MediatorMessenger.Interface;
using System;

namespace heitech.MediatorMessenger.Implementation.Messenger
{
    internal class RequestResultContainer : IRequestResultContainer
    {
        private readonly Type requestType;
        private readonly Type expectedResultType;

        internal RequestResultContainer(Type requestType, Type expectedResult)
        {
            this.requestType = requestType;
            this.expectedResultType = expectedResult;
        }

        public override bool Equals(object obj)
        {
            if ((obj is RequestResultContainer other))
            {
                return this.requestType == other.requestType 
                    && expectedResultType == other.expectedResultType;
            }
            else
                return false;
        }

        public override int GetHashCode()
            => requestType.GetHashCode() | expectedResultType.GetHashCode();

        public bool HasRequestedTypeAndExpectedResult(Type req, Type exp)
            => requestType == req && expectedResultType == exp;
    }
}
