using System;

namespace WebApp_slib.StaticTypes {
    public abstract class AbstractResource : DeserializedElement {
        public string path { get; }


        protected AbstractResource(
            ElementType  type,
            ElementId    id,
            String       path
        ) : base (id, type) {
            this.path = path;
        }

    }

    public class AbstractImage : AbstractResource {
        public AbstractImage(
            ElementId id,
            string    path
        ) : base(ElementType.AbstractImage, id, path) { }
    }
    
    
}