using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Drawing;
using CathodeLib;
using System.Linq.Expressions;

namespace CommandsEditor.Popups.UserControls
{
    public class CS2Reader : IModelReader
    {
        public Model3DGroup Read(int ModelIndex)
        {
            return Editor.resource.models.GetMesh(Editor.resource.models.GetAtWriteIndex(ModelIndex));
        }

        /* Convert a DDS file to System Bitmap */
        private Bitmap GetAsBitmap(string FileName)
        {
            Bitmap toReturn = null;
            if (Path.GetExtension(FileName).ToUpper() != ".DDS") return toReturn;

            try
            {
                MemoryStream imageStream = new MemoryStream(File.ReadAllBytes(FileName));
                using (var image = Pfim.Pfim.FromStream(imageStream))
                {
                    System.Drawing.Imaging.PixelFormat format = System.Drawing.Imaging.PixelFormat.DontCare;
                    switch (image.Format)
                    {
                        case Pfim.ImageFormat.Rgba32:
                            format = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
                            break;
                        case Pfim.ImageFormat.Rgb24:
                            format = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
                            break;
                        default:
                            Console.WriteLine("Unsupported DDS: " + image.Format);
                            break;
                    }
                    if (format != System.Drawing.Imaging.PixelFormat.DontCare)
                    {
                        var handle = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
                        try
                        {
                            var data = Marshal.UnsafeAddrOfPinnedArrayElement(image.Data, 0);
                            toReturn = new Bitmap(image.Width, image.Height, image.Stride, format, data);
                        }
                        finally
                        {
                            handle.Free();
                        }
                    }
                }
            }
            catch { }

            return toReturn;
        }

        public Model3DGroup Read(string path)
        {
            throw new NotImplementedException();
        }

        public Model3DGroup Read(Stream s)
        {
            throw new NotImplementedException();
        }
    }
}