using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZdravoCorp.MainUI.UserWindows;

namespace ZdravoCorp.Surveys.HospitalSurveys
{
    /// <summary>
    /// Interaction logic for HospitalSurveyView.xaml
    /// </summary>
    public partial class HospitalSurveyView : Window
    {
        public HospitalSurveyView()
        {
            HospitalSurveyViewModel hospitalSurveyViewModel = new(this);
            this.DataContext = hospitalSurveyViewModel;
            InitializeComponent();
        }
    }
}
