using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;

namespace WebApp_slib.StaticTypes {
    public class ResourceYield : ReadOnlyDictionary<GameResourceType, int> {


        
        public static ResourceYield NOTHING = new ResourceYield(new UnfinishedResourceYield());

        [Pure] public static ResourceYield operator +(ResourceYield a, ResourceYield b) {
            if (a == null && b == null) return NOTHING;
            if (a == null) return b;
            if (b == null) return a;
            var total = new UnfinishedResourceYield();
            foreach (var yield in Enumerable.Concat(first: a, second: b)) {
                int prevYield = total.ContainsKey(yield.Key) ? total[yield.Key] : 0;
                total[yield.Key] = prevYield + yield.Value;
            }
            return new ResourceYield(total);
        }

        private ResourceYield(IDictionary<GameResourceType, int> dictionary) : base(dictionary) { }
        
        public class UnfinishedResourceYield : Dictionary<GameResourceType,int> {
            public ResourceYield readOnly() { return new ResourceYield(this); }

            public void Add(SingularYield yield) {
                this.Add(yield.type, yield.value);
            }
            
            public void Set(SingularYield yield) {
                this[yield.type] = yield.value;
            }

            public SingularYield Get(GameResourceType type) {
                return new SingularYield(type, this[type]);
            }
        }
        public SingularYield Get(GameResourceType type) {
            return new SingularYield(type, this[type]);
        }
    }
}