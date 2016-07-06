﻿using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace stakx.WIC.Interop
{
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static class IWICBitmapDecoderInfoExtensions
    {
        public static string GetColorManagementVersion(this IWICBitmapDecoderInfo bitmapDecoderInfo)
        {
            StringBufferMethod fn = bitmapDecoderInfo.GetColorManagementVersion;
            return fn.InvokeAndConvertBufferToString();
        }

        public static string GetDeviceManufacturer(this IWICBitmapDecoderInfo bitmapDecoderInfo)
        {
            StringBufferMethod fn = bitmapDecoderInfo.GetDeviceManufacturer;
            return fn.InvokeAndConvertBufferToString();
        }

        public static string GetDeviceModels(this IWICBitmapDecoderInfo bitmapDecoderInfo)
        {
            StringBufferMethod fn = bitmapDecoderInfo.GetDeviceModels;
            return fn.InvokeAndConvertBufferToString();
        }

        public static string[] GetMimeTypes(this IWICBitmapDecoderInfo bitmapDecoderInfo)
        {
            StringBufferMethod fn = bitmapDecoderInfo.GetMimeTypes;
            return fn.InvokeAndConvertBufferToString().Split(',');
        }

        public static string[] GetFileExtensions(this IWICBitmapDecoderInfo bitmapDecoderInfo)
        {
            StringBufferMethod fn = bitmapDecoderInfo.GetFileExtensions;
            return fn.InvokeAndConvertBufferToString().Split(',');
        }

        public static WICBitmapPattern[] GetPatterns(this IWICBitmapDecoderInfo bitmapDecoderInfo)
        {
            int count;
            int size;
            bitmapDecoderInfo.GetPatterns(0, IntPtr.Zero, out count, out size);
            if (count == 0)
            {
                return new WICBitmapPattern[0];
            }
            using (var buffer = new CoTaskMemPtr(Marshal.AllocCoTaskMem(size)))
            {
                bitmapDecoderInfo.GetPatterns(size, buffer, out count, out size);
                IntPtr at = buffer;
                var patterns = new WICBitmapPattern[count];
                for (int i = 0, stride = Marshal.SizeOf(typeof(WICBitmapPatternRaw)); i < count; ++i, at += stride)
                {
                    var raw = (WICBitmapPatternRaw)Marshal.PtrToStructure(at, typeof(WICBitmapPatternRaw));
                    int length = raw.Length;
                    patterns[i] = new WICBitmapPattern()
                    {
                        Length = length,
                        Position = raw.Position,
                        Pattern = new byte[length],
                        Mask = new byte[length],
                        EndOfStream = raw.EndOfStream
                    };
                    Marshal.Copy(raw.Pattern, patterns[i].Pattern, 0, length);
                    Marshal.Copy(raw.Mask, patterns[i].Mask, 0, length);
                }
                return patterns;
            }
        }
    }
}