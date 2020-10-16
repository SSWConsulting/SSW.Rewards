using SSW.Rewards.ViewModels;
using System.Diagnostics;
using Xamarin.Forms;


namespace SSW.Rewards.Views
{
    public partial class RewardsPage : ContentPage
    {
        //public RewardsViewModel ViewModel { get; set; }

        public RewardsPage()
        {
            Debug.WriteLine("Rewards page default consturctor called");
            InitializeComponent();
            var ViewModel = Resolver.Resolve<RewardsViewModel>();
            ViewModel.Navigation = Navigation;
            BindingContext = ViewModel;
        }

        public RewardsPage(RewardsViewModel viewModel)
        {
            Debug.WriteLine("Rewards page viewmodel constructor called");
            InitializeComponent();
            var ViewModel = viewModel;
            ViewModel.Navigation = Navigation;
            BindingContext = ViewModel;
        }
    }
}