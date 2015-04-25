﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MapWinGIS;
using MW5.Api.Enums;
using MW5.Api.Helpers;
using MW5.Api.Interfaces;
using Image = MapWinGIS.Image;

namespace MW5.Api.Concrete
{
    public class RasterSource : BitmapSource, IRasterSource
    {
        public RasterSource(Image image)
            : base(image)
        {

        }

        #region Static methods

        public static IRasterSource Open(string filename)
        {
            var img = new Image();
            if (!img.Open(filename))
            {
                throw new ApplicationException("Failed to open datasource: " + img.ErrorMsg[img.LastErrorCode]);
            }
            if (img.SourceType != tkImageSourceType.istGDALBased)
            {
                // TODO: force opening BMP files with GDAL as well for uniformity
                throw new ApplicationException("Image format isn't supported by RasterSource");
            }
            return new RasterSource(img);
        }

        #endregion

        public override double Dx
        {
            get { return _image.OriginalDX; }
            set { throw new InvalidOperationException("Dx property isn't supported for RasterSource."); }
        }

        public override double Dy
        {
            get { return _image.OriginalDY; }
            set { throw new InvalidOperationException("Dy property isn't supported for RasterSource."); }
        }

        public override int Width
        {
            get { return _image.OriginalWidth; }
        }

        public override int Height
        {
            get { return _image.OriginalHeight; }
        }

        public override double XllCenter
        {
            get { return _image.OriginalXllCenter; }
            set { throw new InvalidOperationException("XllCenter property isn't supported for RasterSource."); }
        }

        public override double YllCenter
        {
            get { return _image.OriginalYllCenter; }
            set { throw new InvalidOperationException("YllCenter property isn't supported for RasterSource."); }
        }

        public override string ToolTipText
        {
            get
            {
                string s = base.ToolTipText;
                // TODO: add number of bands
                return s;
            }
        }

        public override int NumBands
        {
            get { return _image.NoBands; }
        }

        public int NumOverviews
        {
            get { return _image.NumOverviews; }
        }

        public bool BuildOverviews(RasterOverviewSampling method, IEnumerable<int> scales)
        {
            scales = scales.ToList();
            return _image.BuildOverviews((tkGDALResamplingMethod) method, scales.Count(), scales.ToArray());
        }

        public double BufferDx
        {
            get { return _image.dX; }
        }

        public double BufferDy
        {
            get { return _image.dY; }
        }

        public double BufferWidth
        {
            get { return _image.Width; }
        }

        public double BufferHeight
        {
            get { return _image.Height; }
        }

        public double BufferXllCenter
        {
            get { return _image.XllCenter; }
        }

        public double BufferYllCenter
        {
            get { return _image.YllCenter; }
        }

        public void BufferToProjection(int bufferX, int bufferY, out double projX, out double projY)
        {
            _image.BufferToProjection(bufferX, bufferY, out projX, out projY);
        }

        public void ProjectionToBuffer(double projX, double projY, out int bufferX, out int bufferY)
        {
            _image.ProjectionToBuffer(projX, projY, out bufferX, out bufferY);
        }

        public IRasterBandCollection Bands
        {
            get { return new ImageBandCollection(_image); }
        }

        public bool UseHistogram
        {
            get { return _image.UseHistogram; }
            set { _image.UseHistogram = value; }
        }

        public PaletteInterpretation PaletteInterpretation
        {
            get { return (PaletteInterpretation)_image.PaletteInterpretation2; }
        }

        public bool IsRgb
        {
            get { return _image.IsRgb; }
        }

        public bool Warped
        {
            get { return _image.Warped; }
        }

        public bool HasBuiltInColorTable
        {
            get { return _image.HasColorTable; }
        }

        public int ActiveBandIndex
        {
            get { return _image.SourceGridBandIndex; }
            set { _image.SourceGridBandIndex = value; }
        }

        public RasterColorScheme CustomColorScheme
        {
            get
            {
                var scheme = _image.CustomColorScheme;
                return scheme != null ? new RasterColorScheme(scheme) : null;
            }
            set { _image.CustomColorScheme = value.GetInternal(); }
        }

        public bool ForceGridRendering
        {
            get { return _image.AllowGridRendering == tkGridRendering.grForceForAllFormats; }
            set { _image.AllowGridRendering = value ? tkGridRendering.grForceForAllFormats : tkGridRendering.grForGridsOnly; }
        }

        public RenderingType RenderingType
        {
            get
            {
                if (_image.GridRendering)
                {
                    return RenderingType.Grid;
                }
                
                return _image.NoBands == 1 ? RenderingType.Grayscale : RenderingType.Rgb;
            }
        }


        public RasterColorScheme RgbBandMapping
        {
            get
            {
                // TODO: just a stub; shoud be stored in the IImage object instead
                var scheme = new RasterColorScheme();
                scheme.AddInterval(new RasterInterval() {LowColor = Color.Red, Caption = "Red: Band 1"});
                scheme.AddInterval(new RasterInterval() { LowColor = Color.Green, Caption = "Green: Band 2" });
                scheme.AddInterval(new RasterInterval() { LowColor = Color.Blue, Caption = "Blue: Band 3" });
                return scheme;
            }
        }

        public RasterColorScheme GrayScaleColorScheme
        {
            get
            {
                var scheme = new RasterColorScheme();
                
                scheme.AddInterval(new RasterInterval()
                {
                    LowColor = Color.White,
                    HighColor = Color.Black,
                    Caption = "0 - 255"     // TODO: should depend on min / max really used
                });

                return scheme;
            }
        }

        public RasterBand ActiveBand
        {
            get { return Bands[_image.SourceGridBandIndex]; }
        }

        public override GdalDataType DataType
        {
            get
            {
                if (NumBands > 0)
                {
                    return Bands[1].DataType;
                }

                return GdalDataType.Unknown;
            }
        }
    }
}
