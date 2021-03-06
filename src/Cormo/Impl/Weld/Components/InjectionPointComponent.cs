﻿using System;
using Cormo.Contexts;
using Cormo.Injects;

namespace Cormo.Impl.Weld.Components
{
    public class InjectionPointComponent: AbstractComponent
    {
        public InjectionPointComponent(WeldComponentManager manager) : 
            base("", typeof(IInjectionPoint), new IQualifier[0], typeof(DependentAttribute), manager)
        {
        }

        public override IWeldComponent Resolve(Type requestedType)
        {
            return requestedType.IsAssignableFrom(Type) ? this : null;
        }

        public override void Destroy(object instance, ICreationalContext creationalContext)
        {
        }

        protected override BuildPlan GetBuildPlan()
        {
            return context => Manager.GetService<CurrentInjectionPoint>().Peek();
        }

        public override bool IsConcrete
        {
            get { return true; }
        }
    }
}