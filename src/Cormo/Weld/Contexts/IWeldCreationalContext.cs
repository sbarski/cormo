﻿using System.Collections.Generic;
using Cormo.Contexts;
using Cormo.Injects;
using Cormo.Weld.Components;

namespace Cormo.Weld.Contexts
{
    public interface IWeldCreationalContext : ICreationalContext
    {
        IEnumerable<IContextualInstance> DependentInstances { get; }
        void AddDependentInstance(IContextualInstance contextualInstance);
        void Release(IContextual contextual, object instance);
    }
}