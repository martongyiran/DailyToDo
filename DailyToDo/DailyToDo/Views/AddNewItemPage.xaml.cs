using Xamarin.Forms;

namespace DailyToDo.Views
{
    public partial class AddNewItemPage
    {
        public AddNewItemPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ItemEntry.Focus();
        }
    }
}