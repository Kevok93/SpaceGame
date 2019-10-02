using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using WebApp_slib.Util;

using GR = WebApp_slib.StaticTypes.GameResourceType;
using GRKVP = System.Collections.Generic.KeyValuePair<WebApp_slib.StaticTypes.GameResourceType,int>;


namespace WebApp_slib.StaticTypes {

    public struct ResourceYield : IResourceYield  {
        private readonly IResourceYield _yield;

        internal ResourceYield(MutableResourceYield yield) 
            => this._yield = yield;

        private ResourceYield(IResourceYield yield) {
            Debug.Assert(yield != null, nameof(yield) + " != null");
            this._yield = yield;
        }
        
        public int this[GR key]        => this._yield[key];
        public int              Count  => this._yield.Count;
        public IEnumerable<GR > Keys   => this._yield.Keys;
        public IEnumerable<int> Values => this._yield.Values;
        
        IEnumerator IEnumerable.GetEnumerator() 
            => _yield.GetEnumerator();
        public IEnumerator<KeyValuePair<GameResourceType, int>> GetEnumerator() 
            => _yield.GetEnumerator();
        public bool ContainsKey(GameResourceType key)
            => this._yield.ContainsKey(key);
        public bool TryGetValue(GameResourceType key, out int value)
            => this._yield.TryGetValue(key, out value);
        public SingularYield getYield(GameResourceType type)
            => this._yield.getYield(type);
        [Pure] public MutableResourceYield cloneUnlocked()
            => this._yield.cloneUnlocked();
        [Pure] public ResourceYield cloneLocked()
            => this._yield.cloneLocked();
        
        public static implicit operator ResourceYield (MutableResourceYield yield) 
            => new ResourceYield(yield);
        
        public static implicit operator ResourceYield? (MutableResourceYield? yield) 
            => (yield != null) ? new ResourceYield(yield) : (ResourceYield?) null;


        [Pure] public static MutableResourceYield operator + (
            ResourceYield a,
            ResourceYield b
        ) => ResourceYieldFunc.combinePure(a, b);
        [Pure] public static MutableResourceYield operator + (
            ResourceYield a,
            ResourceYield? b
        ) => ResourceYieldFunc.combinePure(a, b);
        [Pure] public static MutableResourceYield operator + (
            ResourceYield? a,
            ResourceYield b
        ) => ResourceYieldFunc.combinePure(a, b);

        [Pure] public static MutableResourceYield operator +(
            ResourceYield? a,
            ResourceYield? b
        ) => ResourceYieldFunc.combinePure(a, b);


        [Pure] public static MutableResourceYield operator * (
            ResourceYield? src,
            int                   mult
        ) => ResourceYieldFunc.scalePure(src, mult);
        
        [Pure] public static MutableResourceYield operator * (
            int mult,
            ResourceYield? src
        ) => ResourceYieldFunc.scalePure(src, mult);
        
        [Pure] public static MutableResourceYield operator * (
            ResourceYield  src,
            int            mult
        ) => ResourceYieldFunc.scalePure(src, mult);
        
        [Pure] public static MutableResourceYield operator * (
            int            mult,
            ResourceYield  src
        ) => ResourceYieldFunc.scalePure(src, mult);
    }
    
    
    internal interface IResourceYield
        : IReadOnlyDictionary<GameResourceType, int> 
    {
        [Pure] MutableResourceYield  cloneUnlocked();
        [Pure] ResourceYield         cloneLocked();
        SingularYield                getYield(GameResourceType type);
    }

    public static class ResourceYieldFunc {

        public static MutableResourceYield combineDirty(
            this MutableResourceYield  src,
            /**/ ResourceYield?        mod
        ) => _combineDirty(src, mod);
        public static MutableResourceYield combineDirty(
            this MutableResourceYield  src,
            /**/ MutableResourceYield? mod
        ) => _combineDirty(src, mod);
        private static MutableResourceYield _combineDirty (
            this MutableResourceYield  src, 
            /**/ IResourceYield        mod
        ) {
            if (mod == null) return src;
            foreach (var yield in mod) {
                int prevYield = src.ContainsKey(yield.Key) ? src[yield.Key] : 0;
                src[yield.Key] = prevYield + yield.Value;
            }
            return src;
        }

        [Pure] public static MutableResourceYield combinePure(
            this MutableResourceYield? a,
            /**/ ResourceYield?        b
        ) => _combinePure(a, b); 
        [Pure] public static MutableResourceYield combinePure(
            this MutableResourceYield a,
            /**/ ResourceYield? b
        ) => _combinePure(a, b); 
        [Pure] public static MutableResourceYield combinePure(
            this ResourceYield  a,
            /**/ ResourceYield? b
        ) => _combinePure(a, b);
        [Pure] public static MutableResourceYield combinePure(
            this ResourceYield? a,
            /**/ ResourceYield? b
        ) => _combinePure(a, b);
        
        [Pure] private static MutableResourceYield _combinePure (
            this IResourceYield a, 
            /**/ IResourceYield b
        ) {
            if (a == null && b == null) return EMPTY;
            if (a == null) return b.cloneUnlocked();
            if (b == null) return a.cloneUnlocked();
            var total = new MutableResourceYield();
            foreach (var yield in Enumerable.Concat(first: a, second: b)) {
                int prevYield = total.ContainsKey(yield.Key) ? total[yield.Key] : 0;
                total[yield.Key] = prevYield + yield.Value;
            }
            return total;
        }

        public static MutableResourceYield scaleDirty (
            this MutableResourceYield src,
            /**/ int                  mult
        ) {
            src.ToList().ForEach(kvp => src[kvp.Key] *= mult);
            return src;
        }
        
        [Pure] public static MutableResourceYield scalePure (
            this ResourceYield src, 
            /**/ int           mult
        ) => _scalePure(src, mult);
        [Pure] public static MutableResourceYield scalePure (
            this MutableResourceYield src, 
            /**/ int           mult
        ) => _scalePure(src, mult);
        [Pure] public static MutableResourceYield scalePure(
            this ResourceYield? src,
            /**/ int            mult
        ) => _scalePure(src, mult);
        
        [Pure] private static MutableResourceYield _scalePure (
            IResourceYield src,
            int mult
        ) {
            if (src == null) return EMPTY;
            var total = new MutableResourceYield();
            foreach (var resourceType in src.Keys) {
                total[resourceType] = src[resourceType] * mult;
            }
            return total;
        }
        
        
        public static readonly ResourceYield NOTHING_CONST =  MutableResourceYield.New().readOnly();
        public static MutableResourceYield   EMPTY         => MutableResourceYield.New();
        
    }

    public struct MutableResourceYield : 
        IDictionary<GameResourceType, int>, 
        IResourceYield 
    {
        
        public static MutableResourceYield New(
            IDictionary<GameResourceType, int> dictionary = null
        ) => new MutableResourceYield(dictionary);
        
        private MutableResourceYield(
            IDictionary<GameResourceType, int> dictionary = null
        ) => __yield = 
            (dictionary != null) 
                ? new MutableResourceYieldInternal(dictionary)
                : new MutableResourceYieldInternal();

        //TBPH I hate this
        //I can't stop you from making an initialized yield
        //So if you try that nonsense, I'll just lazy init it for you
        //At the cost of all my cycles
        private MutableResourceYieldInternal _yield {
            get {
                if (__yield == null) 
                    __yield = new MutableResourceYieldInternal();
                return __yield;
            }
        }

        private MutableResourceYieldInternal __yield { get; set; }

        IEnumerator IEnumerable.GetEnumerator()   => _yield.GetEnumerator();
        public IEnumerator<GRKVP> GetEnumerator() => _yield.GetEnumerator();
        public void Clear()                       => _yield.Clear();
        public int Count                          => _yield.Count;


        void ICollection<GRKVP>.Add(GRKVP item) 
            => (_yield as ICollection<GRKVP>).Add(item);
        
        public bool Contains(GRKVP item)
            => _yield.Contains(item);

        public void CopyTo(GRKVP[] array, int arrayIndex)
            => (_yield as ICollection<GRKVP>).CopyTo(array, arrayIndex);

        public bool Remove(GRKVP item)
            => (_yield as ICollection<GRKVP>).Remove(item);

        public bool IsReadOnly 
            => (this._yield as ICollection<GRKVP>).IsReadOnly;

        public void Add(GR key, int value) 
            => _yield.Add(key, value);

        public bool ContainsKey(GR key) 
            => _yield.ContainsKey(key);

        public bool Remove(GR key)
            => _yield.Remove(key);

        public bool TryGetValue(GR key, out int value)
            => _yield.TryGetValue(key, out value);
        
        public int this[GR key] {
            get => _yield[key];
            set => _yield[key] = value;
        }
        
        
        public void Add(SingularYield yield) => _yield.Add(yield);
        public void Set(SingularYield yield) => _yield.Set(yield);

        ICollection<GR > IDictionary<GR, int>.Keys   => _yield.Keys;
        ICollection<int> IDictionary<GR, int>.Values => _yield.Values;
        IEnumerable<GR > IReadOnlyDictionary<GR, int>.Keys   => _yield.Keys;
        IEnumerable<int> IReadOnlyDictionary<GR, int>.Values => _yield.Values;
        public Dictionary<GR,int>.  KeyCollection Keys   => _yield.Keys;
        public Dictionary<GR,int>.ValueCollection Values => _yield.Values;

        public MutableResourceYield cloneUnlocked()
            => new MutableResourceYield(this);
        public ResourceYield cloneLocked()
            => cloneUnlocked().readOnly();
        public SingularYield getYield(GameResourceType type) 
            => _yield.getYield(type);

        public static ResourceYield        NOTHING_CONST => ResourceYieldFunc.NOTHING_CONST;
        public static MutableResourceYield EMPTY         => ResourceYieldFunc.EMPTY;

        public ResourceYield readOnly() => new ResourceYield(this);

    }
    
    public class MutableResourceYieldInternal 
        : Dictionary<GameResourceType,int>
    {

        public void Add(SingularYield yield) => this.Add(yield.type, yield.value);
        public void Set(SingularYield yield) => this[yield.type] = yield.value;
        public SingularYield getYield(GameResourceType type) 
            => new SingularYield(type, this[type]);

        internal MutableResourceYieldInternal() {}
        internal MutableResourceYieldInternal(
            IDictionary<GameResourceType, int> dictionary
        ) : base(dictionary) { }

    }
}