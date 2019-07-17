using System;

namespace WebApp_slib.StaticTypes {
    public abstract class AbstractResource {
        public String       path         { get; }
        public ResourceType resourceType { get; }


        internal AbstractResource(
            String       path,
            ResourceType type
        ) {
            this.path         = path;
            this.resourceType = type;
        }

        public enum ResourceType {IMAGE}
    }

    public class AbstractImage : AbstractResource {
        public AbstractImage(string path) : base(path, ResourceType.IMAGE) {
        }
    }
    
    
}