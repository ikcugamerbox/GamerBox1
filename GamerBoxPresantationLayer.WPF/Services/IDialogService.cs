using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerBoxPresantationLayer.WPF.Services
{
    public interface IDialogService
    {
        void ShowMessage(string message, string title);
        bool ShowConfirmation(string message, string title);
        string? OpenFile(string title, string filter);
    }
}
