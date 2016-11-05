
namespace TheBoxSoftware.Reflection.Core
{
    using System;

    public static class FieldReader
    {
        public static Int32 ToInt32(byte[] value, int offset)
        {
            return FieldReader.ToInt32(value, offset, 4);
        }

        public static Int32 ToInt32(byte[] value, int offset, int toRead)
        {
            Int32 theValue = 0;
            if(toRead == 2)
            {
                theValue = BitConverter.ToInt16(value, offset);
            }
            else
            {
                theValue = BitConverter.ToInt32(value, offset);
            }
            return theValue;
        }

        public static UInt32 ToUInt32(byte[] value, int offset)
        {
            return FieldReader.ToUInt32(value, offset, 4);
        }

        public static UInt32 ToUInt32(byte[] value, int offset, int toRead)
        {
            UInt32 theValue = 0;
            if(toRead == 2)
            {
                theValue = BitConverter.ToUInt16(value, offset);
            }
            else
            {
                theValue = BitConverter.ToUInt32(value, offset);
            }
            return theValue;
        }

        public static Int16 ToInt16(byte[] value, int offset)
        {
            return BitConverter.ToInt16(value, offset);
        }

        public static UInt16 ToUInt16(byte[] value, int offset)
        {
            return BitConverter.ToUInt16(value, offset);
        }
    }
}