using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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

        public static ulong GetTimestamp()
        {
            return (ulong)DateTime.UtcNow.ToFileTimeUtc();
        }
    }
}
