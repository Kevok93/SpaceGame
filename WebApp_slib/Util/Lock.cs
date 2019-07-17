using System;

namespace WebApp_slib.Util {

    public class Lock {
        private object _lock = new Lock();
        
        public TR doLocked<TR>(Func<TR> action) {
            lock (_lock) return action.Invoke();
        }
        public TR doLocked<T1,TR>(Func<T1,TR> action, T1 arg1) {
            lock (_lock) return action.Invoke(arg1);
        }
        public TR doLocked<T1,T2,TR>(Func<T1,T2,TR> action, T1 arg1, T2 arg2) {
            lock (_lock) return action.Invoke(arg1,arg2);
        }
    }
}