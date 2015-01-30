﻿using System;

namespace Cormo.Contexts
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Parameter | AttributeTargets.Field)]
    public abstract class ScopeAttribute : Attribute
    {
        
    }

    public abstract class NormalScopeAttribute : ScopeAttribute
    {
         
    }
}