using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace SorterControls.View.Common
{
    public static class BrushFactory
    {
        static List<Brush> _gradedBlueBrushes;
        public static IReadOnlyList<Brush> GradedBlueBrushes()
        {
            if ((_gradedBlueBrushes != null) && (_gradedBlueBrushes.Count == 16))
            {
                return _gradedBlueBrushes;
            }

            const float baseValue = (float)0.10;
            var increment = (float)((1.0 - baseValue) / 16);

            _gradedBlueBrushes = new List<Brush>();
            for (var i = 0; i < 16; i++)
            {
                var scb = new SolidColorBrush(
                    new Color
                    {
                        ScA = (float)1.0,
                        ScB = baseValue + increment * i,
                        ScG = baseValue,
                        ScR = baseValue
                    });

                scb.Freeze();
                _gradedBlueBrushes.Add(scb);
            }

            return _gradedBlueBrushes;
        }

        static List<Brush> _grayThenBlueToRedBrushes;
        public static IReadOnlyList<Brush> GrayThenBlueToRedBrushes()
        {
            if ((_grayThenBlueToRedBrushes != null) && (_grayThenBlueToRedBrushes.Count == 16))
            {
                return _grayThenBlueToRedBrushes;
            }
            _grayThenBlueToRedBrushes = new List<Brush>();
            _grayThenBlueToRedBrushes.Add(Brushes.Gray);

            for (float i = 1; i < 16; i++)
            {
                var scb = new SolidColorBrush(
                    new Color
                    {
                        ScA = (float)1.0,
                        ScB = (float)(1.0 - i / 16.0),
                        ScG = (float)0.0,
                        ScR = (float)(i / 16.0)
                    });

                scb.Freeze();
                _grayThenBlueToRedBrushes.Add(scb);
            }

            return _grayThenBlueToRedBrushes;
        }

        static List<Brush> _grayThenBlueToBlackBrushes;
        public static IReadOnlyList<Brush> GrayThenBlueToBlackBrushes()
        {
            if ((_grayThenBlueToBlackBrushes != null) && (_grayThenBlueToBlackBrushes.Count == 16))
            {
                return _grayThenBlueToBlackBrushes;
            }
            _grayThenBlueToBlackBrushes = new List<Brush>();
            _grayThenBlueToBlackBrushes.Add(Brushes.LightGray);

            for (float i = 1; i < 16; i++)
            {
                var scb = new SolidColorBrush(
                    new Color
                    {
                        ScA = (float)1.0,
                        ScB = (float)(1.0 - i / 32.0),
                        ScG = (float)(0.5 - i / 32.0),
                        ScR = (float)(0.5 - i / 32.0)
                    });

                scb.Freeze();
                _grayThenBlueToBlackBrushes.Add(scb);
            }

            return _grayThenBlueToBlackBrushes;
        }


        public static Brush LogBrushOfInt(int value, int max, IReadOnlyList<Brush> brushList)
        {
            if (value == 0)
            {
                return brushList[0];
            }

            var offset = Math.Log(value) * (brushList.Count -1) / Math.Log(max);
            var intDex = 1 + (int) offset;

            return brushList[intDex];
        }
    }

}
