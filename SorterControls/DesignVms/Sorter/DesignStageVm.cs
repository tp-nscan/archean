using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using SorterControls.ViewModel.Sorter;
using Sorting.KeyPairs;

namespace SorterControls.DesignVms.Sorter
{
    public class DesignStageVm : StageVmImpl
    {
        public DesignStageVm()
            : base
                (
                    keyCount: _keyCount,
                    keyPairVms: KeyPairVms.ToList(),
                    switchWidth: _switchWidth,
                    lineThickness: _lineThickness,
                    lineBrush: _lineBrush,
                    backgroundBrush: _backgroundBrush
               )
        {
        }

        public const int _keyCount = 8;
        private const double _switchWidth = 0.3;
        private const double _lineThickness = 0.07;
        private static readonly Brush _lineBrush = new SolidColorBrush(Colors.Black);
        private static readonly Brush _backgroundBrush = new SolidColorBrush(Colors.WhiteSmoke);


        public static IEnumerable<KeyPairVm> KeyPairVms
        {
           get
           {
               yield return
                   new KeyPairVm
                       (
                           keyPair: KeyPairRepository.KeyPairFromKeys(0, 1),
                           switchBrush: new SolidColorBrush(Colors.Olive),
                           position: 0
                       );

               yield return
                    new KeyPairVm
                       (
                           keyPair: KeyPairRepository.KeyPairFromKeys(3, 7),
                           switchBrush: new SolidColorBrush(Colors.SeaGreen),
                           position: 0
                       );

               yield return
                   new KeyPairVm
                       (
                           keyPair: KeyPairRepository.KeyPairFromKeys(1, 2),
                           switchBrush: new SolidColorBrush(Colors.OrangeRed),
                           position: 1
                       );

               yield return
                    new KeyPairVm
                       (
                           keyPair: KeyPairRepository.KeyPairFromKeys(5, 6),
                           switchBrush: new SolidColorBrush(Colors.SlateBlue),
                           position: 1
                       );

               yield return
                   new KeyPairVm
                       (
                           keyPair: KeyPairRepository.KeyPairFromKeys(3, 7),
                           switchBrush: new SolidColorBrush(Colors.PaleVioletRed),
                           position: 2
                       );

               yield return
                   new KeyPairVm
                       (
                           keyPair: KeyPairRepository.KeyPairFromKeys(0, 4),
                           switchBrush: new SolidColorBrush(Colors.Orchid),
                           position: 3
                       );

               yield return
                    new KeyPairVm
                       (
                           keyPair: KeyPairRepository.KeyPairFromKeys(1, 5),
                           switchBrush: new SolidColorBrush(Colors.Navy),
                           position: 4
                       );

               yield return
                     new KeyPairVm
                       (
                           keyPair: KeyPairRepository.KeyPairFromKeys(2, 6),
                           switchBrush: new SolidColorBrush(Colors.SaddleBrown),
                           position: 5
                       );


           }
        }
    }
}
