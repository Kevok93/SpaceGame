using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;
using WebApp_slib.Util;

namespace WebApp_slib.StaticTypes {


    public interface FinishedResourceYield
        : IReadOnlyDictionary<GameResourceType, int> 
    {
        [Pure] ResourceYield cloneUnlocked();
        [Pure] FinishedResourceYield cloneLocked();
        SingularYield getYield(GameResourceType type);
    }

    public static class ResourceYieldFunc {
        public static ResourceYield combineDirty (
            this ResourceYield src, 
            /**/ FinishedResourceYield mod
        ) {
            if (src == null) throw new ArgumentNullException(paramName: nameof(src));
            if (mod == null) return src;
            foreach (var yield in mod) {
                int prevYield = src.ContainsKey(yield.Key) ? src[yield.Key] : 0;
                src[yield.Key] = prevYield + yield.Value;
            }
            return src;
        }  
        [Pure] public static FinishedResourceYield combinePure (
            this FinishedResourceYield a, 
            /**/ FinishedResourceYield b
        ) {
            if (a == null && b == null) return ResourceYield.NOTHING;
            if (a == null) return b;
            if (b == null) return a;
            var total = new ResourceYield();
            foreach (var yield in Enumerable.Concat(first: a, second: b)) {
                int prevYield = total.ContainsKey(yield.Key) ? total[yield.Key] : 0;
                total[yield.Key] = prevYield + yield.Value;
            }
            return total;
        }  
        public static ResourceYield scaleDirty (
            this ResourceYield src, 
            /**/ int                   mult
        ) {
            if (src == null) throw new ArgumentNullException(paramName: nameof(src));
            src.ToList().ForEach(kvp => src[kvp.Key] *= mult);
            return src;
        }
        [Pure] public static FinishedResourceYield scalePure (
            this FinishedResourceYield src, 
            /**/ int                   mult
        ) {
            if (src == null) return ResourceYield.NOTHING;
            var total = new ResourceYield();
            foreach (var resourceType in src.Keys) {
                total[resourceType] = src[resourceType] * mult;
            }
            return total;
        }
    }
    public class ResourceYield 
        : Dictionary<GameResourceType,int>
        , FinishedResourceYield 
    {
        
        //Can't make both variables allow the interface,
        //so these overrides are pretty much useless
        //[Pure] public static FinishedResourceYield operator +(
        //    FinishedResourceYield a,
        //    FinishedResourceYield b
        //) => a.addition(b);
        //[Pure] public static FinishedResourceYield operator + (
        //    ResourceYield         a,
        //    FinishedResourceYield b
        //) => a?.combinePure(b) ?? b ?? NOTHING;
        //[Pure] public static FinishedResourceYield operator + (
        //    FinishedResourceYield a,
        //    ResourceYield         b
        //) => a?.combinePure(b) ?? b ?? NOTHING;
        //[Pure] public static FinishedResourceYield operator + (
        //    ResourceYield         a,
        //    ResourceYield         b
        //) => a?.combinePure(b) ?? b ?? NOTHING;
        //[Pure] public static FinishedResourceYield operator * (
        //    ResourceYield src,
        //    int           mult
        //) => src?.scalePure(mult) ?? NOTHING;
        //[Pure] public static FinishedResourceYield operator * (
        //    int           mult,
        //    ResourceYield src
        //) => src?.scalePure(mult) ?? NOTHING;
        
        public FinishedResourceYield readOnly() => this;

        public void Add(SingularYield yield) => this.Add(yield.type, yield.value);
        public void Set(SingularYield yield) => this[yield.type] = yield.value;
        public SingularYield getYield(GameResourceType type) => new SingularYield(type, this[type]);

        public static readonly FinishedResourceYield NOTHING = new ResourceYield();

        [Pure] public ResourceYield cloneUnlocked () 
            => new ResourceYield(this);
        [Pure] public FinishedResourceYield cloneLocked () 
            => cloneUnlocked().readOnly();

        public ResourceYield(){}
        private ResourceYield(
            IDictionary<GameResourceType, int> dictionary
        ) : base(dictionary) { }

    }
}