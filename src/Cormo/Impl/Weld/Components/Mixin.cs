using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cormo.Impl.Weld.Injections;
using Cormo.Injects;
using Cormo.Mixins;

namespace Cormo.Impl.Weld.Components
{
    public class Mixin : ManagedComponent
    {
        private readonly IEnumerable<Type> _mixinBinders;

        public Mixin(Type[] interfaceTypes, Type type, IEnumerable<IBinderAttribute> binders, Type scope, WeldComponentManager manager, MethodInfo[] postConstructs) 
            : base(type, binders, scope, manager, postConstructs)
        {
            _mixinBinders = binders.OfType<IMixinBinder>().Select(x=> x.GetType());
            InterfaceTypes = interfaceTypes;
        }

        public Type[] InterfaceTypes { get; private set; }

        public override IWeldComponent Resolve(Type requestedType)
        {
            return requestedType.IsAssignableFrom(Type) ? this : null;
        }

        protected override BuildPlan MakeConstructPlan(IEnumerable<MethodParameterInjectionPoint> injects)
        {
            var paramInjects = injects.GroupBy(x => x.Member)
                .Select(x => x.OrderBy(i => i.Position).ToArray())
                .DefaultIfEmpty(new MethodParameterInjectionPoint[0])
                .First();

            return context =>
            {
                var paramVals = paramInjects.Select(p => p.GetValue(context)).ToArray();
                return Activator.CreateInstance(Type, paramVals);
            };
        }

        public bool CanMixTo(IComponent component)
        {
            return _mixinBinders.Any(component.Binders.Select(x=> x.GetType()).Contains);
        }
    }
}