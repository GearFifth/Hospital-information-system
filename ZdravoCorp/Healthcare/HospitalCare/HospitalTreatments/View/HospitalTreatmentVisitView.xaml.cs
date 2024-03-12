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
using ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.ViewModel;

namespace ZdravoCorp.Healthcare.HospitalCare.HospitalTreatments.View
{
    /// <summary>
    /// Interaction logic for HospitalTreatmentVisitView.xaml
    /// </summary>
    public partial class HospitalTreatmentVisitView : Window
    {
        public HospitalTreatmentVisitView()
        {
            DataContext = new HospitalTreatmentVisitViewModel(this);
            InitializeComponent();
        }
    }
}
