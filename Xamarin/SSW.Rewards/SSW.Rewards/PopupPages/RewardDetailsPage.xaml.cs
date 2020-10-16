using Rg.Plugins.Popup.Pages;
using SSW.Rewards.Models;
using SSW.Rewards.Services;
using SSW.Rewards.ViewModels;
using Xamarin.Forms.Xaml;

namespace SSW.Rewards.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RewardDetailsPage : PopupPage
    {
        private RewardDetailsViewModel viewModel { get; set; }

        public RewardDetailsPage(Reward reward, IRewardService rewardService)
        {
            InitializeComponent();
            var userService = Resolver.Resolve<IUserService>();
            viewModel = new RewardDetailsViewModel(reward, rewardService, userService);
            BindingContext = viewModel;
        }
    }
}