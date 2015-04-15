using System;
using System.Collections.Generic;
using System.Text;

namespace BussinesLogicLayer
{
    public class TyreNotFoundException : Exception
    {
        public TyreNotFoundException()
        { }

        public TyreNotFoundException(string message)
            : base(message)
        { }

        public TyreNotFoundException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    public class RackException : Exception
    {
        public RackException()
        { }

        public RackException(string message)
            : base(message)
        { }

        public RackException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    public class PropisNotFoundException : Exception
    {
        public PropisNotFoundException()
        { }

        public PropisNotFoundException(string message)
            : base(message)
        { }

        public PropisNotFoundException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    public class RadnikException : Exception
    {
        public RadnikException()
        { }

        public RadnikException(string message)
            : base(message)
        { }

        public RadnikException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    public class PozicijaException : Exception
    {
        public PozicijaException()
        { }

        public PozicijaException(string message)
            : base(message)
        { }

        public PozicijaException(string message, Exception inner)
            : base(message, inner)
        { }
    }

    public class defaultException : Exception
    {
        public defaultException()
        { }

        public defaultException(string message)
            : base(message)
        { }

        public defaultException(string message, Exception inner)
            : base(message, inner)
        { }
    }


    public class SyncingException : Exception
    {
        public SyncingException(string message)
            : base(message)
        { }
    }

    public class DataFlowException : Exception
    {
        public DataFlowException(string message)
            : base(message)
        { }
    }

    public class ChangedZahtevException : Exception
    {
        public ChangedZahtevException(string message)
            : base(message)
        { }
    }


}
