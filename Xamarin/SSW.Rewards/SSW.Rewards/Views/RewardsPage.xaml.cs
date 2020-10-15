using SSW.Rewards.ViewModels;
using Xamarin.Forms;


namespace SSW.Rewards.Views
{
    public partial class RewardsPage : ContentPage
    {
        public RewardsViewModel ViewModel { get; set; }
        public RewardsPage()
        {
            InitializeComponent();
            ViewModel = Resolver.Resolve<RewardsViewModel>();
            ViewModel.Navigation = Navigation;
            BindingContext = ViewModel;
        }

        public RewardsPage(RewardsViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            ViewModel.Navigation = Navigation;
            BindingContext = ViewModel;
        }
    }
}