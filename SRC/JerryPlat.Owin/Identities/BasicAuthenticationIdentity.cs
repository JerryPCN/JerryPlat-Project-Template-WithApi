using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace JerryPlat.Owin.Identities
{
    public class BasicAuthenticationIdentity<T> : GenericIdentity
    {
        public T Session { get; set; }
        public BasicAuthenticationIdentity(string name, T session)
            : base(name, "Basic")
        {
            this.Session = session;
        }
    }
}