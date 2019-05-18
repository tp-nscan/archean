using System;
using System.Windows;
using System.Windows.Controls;
using SorterControls.ViewModel.SorterOld;

namespace SorterControls.TemplateSelectorsOld
{
    public class SorterSelector : DataTemplateSelector
    {
        public DataTemplate UnstagedSorterTemplate { get; set; }

        public DataTemplate StagedSorterTemplate { get; set; }

        public DataTemplate DefaultTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var sorterVm = item as ISorterVm;

            if (sorterVm != null)
            {
                switch (sorterVm.SorterVmType)
                {
                    case SorterVmType.Unstaged:
                        return UnstagedSorterTemplate;
                    case SorterVmType.Staged:
                        return StagedSorterTemplate;
                    default:
                        throw new Exception("IScreenVm template not found in Clinical.Resources.ScreenSelector.SelectTemplate");
                }
            }

            return DefaultTemplate;
        }
    


    }
}
