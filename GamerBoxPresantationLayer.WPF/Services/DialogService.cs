using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerBoxPresantationLayer.WPF.Services
{
    public class DialogService : IDialogService
    {
        public void ShowMessage(string message, string title)
        {
            CustomMessageBox.Show(message, title);
        }

        public bool ShowConfirmation(string message, string title)
        {
            return CustomMessageBox.Show(message, title, true);
        }

        public string? OpenFile(string title, string filter)
        {
            OpenFileDialog op = new OpenFileDialog
            {
                Title = title,
                Filter = filter
            };

            if (op.ShowDialog() == true)
            {
                return op.FileName;
            }
            return null;
        }
    }
}