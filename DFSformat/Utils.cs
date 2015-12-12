using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DFSformat
{
    static class Utils
    {
        public static void WriteStruct(BinaryWriter writer, object o)
        {
            byte[] buffer = new byte[Marshal.SizeOf(o.GetType())];
            GCHandle gcHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            Marshal.StructureToPtr(o, gcHandle.AddrOfPinnedObject(), true);
            writer.Write(buffer, 0, buffer.Length);
            gcHandle.Free();
        }

        public static long GetTimestamp()
        {
            return DateTime.UtcNow.ToFileTimeUtc();
        }
    }
}
