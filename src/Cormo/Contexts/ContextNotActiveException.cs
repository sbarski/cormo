﻿using System;

namespace Cormo.Contexts
{
    public class ContextNotActiveException : ContextException
    {
        public ContextNotActiveException(Type scope)
            :base (string.Format("Context not active: {0}", scope.Name))
        {
            
        }
    }
}