using System.Collections.Generic;
using System.Windows.Media;
using MathUtils.Rand;

namespace SorterControls.View.SorterOld
{
    public static class LineBrushFactory
    {
        static List<Brush> _randomSolidBrushes;
        public static List<Brush> RandomSolidBrushes(int seed, int brushCount)
        {
            if (_randomSolidBrushes != null)
            {
                return _randomSolidBrushes;
            }
            _randomSolidBrushes = new List<Brush>();
            var randy = Rando.Fast(seed);
            for (int i = 0; i < brushCount; i++)
            {
                var scb = new SolidColorBrush(
                    new Color
                    {
                        ScA = (float)1.0,
                        ScB = (float)randy.NextDouble(),
                        ScG = (float)randy.NextDouble(),
                        ScR = (float)randy.NextDouble()
                    });

                scb.Freeze();
                _randomSolidBrushes.Add(scb);
            }

            return _randomSolidBrushes;
        }

        static List<Brush> gradedBlueBrushes;
        public static List<Brush> GradedBlueBrushes(int brushCount)
        {
            if ((gradedBlueBrushes != null) && (gradedBlueBrushes.Count == brushCount))
            {
                return gradedBlueBrushes;
            }

            const float baseValue = (float)0.10;
            var increment = (float)((1.0 - baseValue) / brushCount);

            gradedBlueBrushes = new List<Brush>();
            for (var i = 0; i < brushCount; i++)
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
                gradedBlueBrushes.Add(scb);
            }

            return gradedBlueBrushes;
        }

        static List<Brush> gradedRedBrushes;
        public static List<Brush> GradedRedBrushes(int brushCount)
        {
            if ((gradedRedBrushes != null) && (gradedRedBrushes.Count == brushCount))
            {
                return gradedRedBrushes;
            }

            const float baseValue = (float)0.1;
            var increment = (float)((1.0 - baseValue) / brushCount);

            gradedRedBrushes = new List<Brush>();
            for (var i = 0; i < brushCount; i++)
            {
                var scb = new SolidColorBrush(
                    new Color
                    {
                        ScA = (float)1.0,
                        ScB = baseValue,
                        ScG = baseValue,
                        ScR = baseValue + increment * i,
                    });

                scb.Freeze();
                gradedRedBrushes.Add(scb);
            }

            return gradedRedBrushes;
        }
    }
}