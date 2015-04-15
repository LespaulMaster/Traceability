using System;
using System.Collections.Generic;
using System.Text;
using DesignLibrary;
using System.Windows.Forms;
namespace ForkLift
{
    class CustomFunctions
    {
        public static void showError(String message, Form parentForm)
        {
            FormException formException = new FormException(message, true);
            XFormMasking maskingForm = new XFormMasking(formException);
            maskingForm = new XFormMasking(formException);
            maskingForm.ShowDialog(parentForm);
            
        }
    }
}
