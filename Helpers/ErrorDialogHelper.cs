using ProductManagement.WPF.Views;

namespace ProductManagement.WPF.Helpers
{
    public static class ErrorDialogHelper
    {
        public static void ShowErrorDialog(string message)
        {
            var errorDialog = new ErrorDialog(message);
            errorDialog.ShowDialog();
        }
    }
}
