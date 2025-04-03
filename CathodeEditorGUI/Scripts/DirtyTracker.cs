using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandsEditor
{
    public static class DirtyTracker
    {
        private static bool _isDirty = false;
        public static bool IsDirty => _isDirty;

        public static Action OnDirty;
        public static Action OnClean;
        public static Action<bool> OnChanged;

        static DirtyTracker()
        {
            Singleton.OnLevelLoaded += MarkClean;
            Singleton.OnSaved += MarkClean;

            //Singleton.OnCompositeDeleted; <- todo
            Singleton.OnCompositeRenamed += MarkDirty;

            Singleton.OnEntityAdded += MarkDirty;
            Singleton.OnEntityDeleted += MarkDirty;
            Singleton.OnEntityRenamed += MarkDirty;

            //todo: need events for params being added/removed/modified
        }

        private static void MarkClean(object a) => MarkClean();
        private static void MarkClean()
        {
            bool changed = _isDirty;
            _isDirty = false;
            OnClean?.Invoke();
            if (changed) OnChanged?.Invoke(false);
        }

        private static void MarkDirty(object a, object b) => MarkDirty();
        private static void MarkDirty(object a) => MarkDirty();
        private static void MarkDirty()
        {
            bool changed = !_isDirty;
            _isDirty = true;
            OnDirty?.Invoke();
            if (changed) OnChanged?.Invoke(true);
        }
    }
}
