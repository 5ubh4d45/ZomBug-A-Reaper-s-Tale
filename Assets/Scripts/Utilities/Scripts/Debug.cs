using UnityEngine;

namespace GameEssentials.Debug
{
    public static class DebugUtils
    {
        public static void Log(bool debug, object message)
        {
            if (!debug) return;
            UnityEngine.Debug.Log(message);
        }

        public static void Log(bool debug, object message, Object context)
        {
            if (!debug) return;
            UnityEngine.Debug.Log(message, context);
        }

        public static void LogWarning(bool debug, object message)
        {
            if (!debug) return;
            UnityEngine.Debug.LogWarning(message);
        }

        public static void LogWarning(bool debug, object message, Object context)
        {
            if (!debug) return;
            UnityEngine.Debug.LogWarning(message, context);
        }

        public static void LogError(bool debug, object message)
        {
            if (!debug) return;
            UnityEngine.Debug.LogError(message);
        }

        public static void LogError(bool debug, object message, Object context)
        {
            if (!debug) return;
            UnityEngine.Debug.LogError(message, context);
        }

        public static void BreakEditor(bool debug)
        {
            if (!debug) return;
            UnityEngine.Debug.Break();
        }

        public static void DrawLine(bool debug, Vector3 start, Vector3 end)
        {
            if (!debug) return;
            UnityEngine.Debug.DrawLine(start, end);
        }

        public static void DrawLine(bool debug, Vector3 start, Vector3 end, Color color)
        {
            if (!debug) return;
            UnityEngine.Debug.DrawLine(start, end, color);
        }

        public static void DrawLine(bool debug, Vector3 start, Vector3 end, Color color, float duration)
        {
            if (!debug) return;
            UnityEngine.Debug.DrawLine(start, end, color, duration);
        }

        public static void DrawRay(bool debug, Vector3 start, Vector3 dir)
        {
            if (!debug) return;
            UnityEngine.Debug.DrawRay(start, dir);
        }

        public static void DrawRay(bool debug, Vector3 start, Vector3 dir, Color color)
        {
            if (!debug) return;
            UnityEngine.Debug.DrawRay(start, dir, color);
        }

        public static void DrawRay(bool debug, Vector3 start, Vector3 dir, Color color, float duration)
        {
            if (!debug) return;
            UnityEngine.Debug.DrawRay(start, dir, color, duration);
        }
    }
}