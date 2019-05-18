using System;
using System.Windows;
using System.Windows.Controls;
using SorterControls.ViewModel.Genome;

namespace SorterControls.View.Genome
{
    public class GenomeEditorTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UnstagedSorterTemplate { get; set; }

        public DataTemplate StagedSorterTemplate { get; set; }

        public DataTemplate DefaultTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var screenVm = item as ISorterGenomeEditorVm;

            if (screenVm != null)
            {
                switch (screenVm.GenomeEditorType)
                {
                    case GenomeEditorType.SwitchIndex:
                        return UnstagedSorterTemplate;
                    case GenomeEditorType.Permutation:
                        return StagedSorterTemplate;
                    default:
                        throw new Exception("IScreenVm template not found in Clinical.Resources.ScreenSelector.SelectTemplate");
                }
            }

            return DefaultTemplate;
        }



    }
}
