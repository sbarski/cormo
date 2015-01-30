using System;
using System.Collections.Generic;
using Cormo.Contexts;
using Cormo.Injects;

namespace Cormo.Weld.Injections
{
    public interface IWeldInjetionPoint : IInjectionPoint
    {
        IWeldInjetionPoint TranslateGenericArguments(IComponent component, IDictionary<Type, Type> translations);
        void Inject(object target, ICreationalContext context);
        IComponent Component { get; }
        Type Scope { get; }
    }
}