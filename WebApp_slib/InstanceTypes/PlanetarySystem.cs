using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;
using WebApp_slib.StaticTypes;
using WebApp_slib.Util;
using static WebApp_slib.StaticTypes.GameResourceType;

namespace WebApp_slib.InstanceTypes {
    public class SystemCluster {
        public class PlanetCounts : Dictionary<PlanetType, uint>{}
        
        public static Random rng = new Random();
        
        public ClusterType clusterType { get; }
        public PlanetCounts planetCounts { get; }

        public uint  freePlanetCount { get; private set; }
        public uint totalPlanetCount { get; private set; }

        private readonly MutExExecutor _lock = new MutExExecutor();

        public SystemCluster(
            [NotNull] ClusterType clusterType, 
            uint initialPlanetCount
        ) {
            this.clusterType      = clusterType;
            this.totalPlanetCount = initialPlanetCount;
            this.freePlanetCount  = initialPlanetCount;
            this.planetCounts      = new PlanetCounts();
        }

        public  uint  getPlanetCount(PlanetType type) => this._lock.doLocked(_getPlanetCount, type);
        private uint _getPlanetCount(PlanetType type) => this.planetCounts.ContainsKey(type) ? this.planetCounts[type] : 0;
        
        public  void  modPlanetCount(PlanetType type, int countModification) => this._lock.doLocked(_modPlanetCount, type, countModification);
        private int  _modPlanetCount(PlanetType type, int countModification) {
            uint planetTypeCount = _getPlanetCount(type);
            if (freePlanetCount < countModification)  {
                throw new ArgumentOutOfRangeException(paramName: "Planet Count", message:"Cannot allocate more planets to a type than free");
            } else if (planetTypeCount + countModification < 0) {
                throw new ArgumentOutOfRangeException(paramName: "Planet Count", message:"Cannot remove more planets from a type than free");
            } else {
                freePlanetCount   -= (uint)countModification;
                this.planetCounts[type] =  (uint)(planetTypeCount + countModification);
            }
            
            return 0;
        }

        public  void  modTotalPlanetCount(int countModification) => _lock.doLocked(_modTotalPlanetCount, countModification);
        private int  _modTotalPlanetCount(int countModification) {
            if (totalPlanetCount + countModification < 0) throw new ArgumentException(
                paramName: "Planet Count", 
                message: "Cannot remove more planets from a system than available"
            );
            
            this.totalPlanetCount = (uint) (this.totalPlanetCount + countModification);

            long freePlanetsAfterMod = freePlanetCount + countModification;
            
            if (freePlanetsAfterMod <= 0) {
                //Take away the obvious
                freePlanetCount = 0;
                //Start stripping off a random planet for each remaining planet lost
                long removedPlanetsRemaining = -freePlanetsAfterMod;

                HashSet<PlanetType> availableTypes = new HashSet<PlanetType>(this.planetCounts.Keys);
                while (removedPlanetsRemaining > 0) {
                    PlanetType type = availableTypes.ElementAt(rng.Next(availableTypes.Count));
                    //don't trust
                    Debug.Assert(this.planetCounts[type] >= 0);
                    if (this.planetCounts[type] <= 0) {
                        availableTypes.Remove(type);
                        continue;
                    }
                    _modPlanetCount(type: type, countModification: -1);
                    removedPlanetsRemaining -= 1;
                    Debug.Assert(removedPlanetsRemaining >= 0);
                    //Side effect of don't trust: we delete zeroes on the next loop
                }
                    
            } else freePlanetCount = (uint)freePlanetsAfterMod;

            return 0;
        }

        public  ResourceYield  getYield() => _lock.doLocked(_getYield);
        private ResourceYield _getYield() {
            MutableResourceYield yield = new MutableResourceYield();
            foreach (PlanetType type in this.planetCounts.Keys) 
                yield.combineDirty(type.getYield(this.planetCounts[type]));
            return clusterType.modifyYield(yield.readOnly());
        }

    }
}
