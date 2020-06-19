using System.Windows.Input;
using static PicView.Fields;

namespace PicView.Shortcuts
{
    public static class CustomTextBoxShortcuts
    {
        public static void CustomTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    EditTitleBar.HandleRename();
                    break;

                case Key.Escape:
                    EditTitleBar.Refocus();
                    IsDialogOpen = true; // Hack to make escape not fall through
                    break;
            }
        }
    } 
}

