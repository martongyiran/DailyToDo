using System;
using Xamarin.Forms;

namespace DailyToDo
{
    public partial class MainPage
    {
        double totalChanged = 0;
        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as MainPageViewModel).EditModeOpened += OpenedEvent;
            (BindingContext as MainPageViewModel).AddNewItemVisibility += OnNewItemVisibilityChanged;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            (BindingContext as MainPageViewModel).EditModeOpened -= OpenedEvent;
            (BindingContext as MainPageViewModel).AddNewItemVisibility -= OnNewItemVisibilityChanged;
        }

        public async void OpenedEvent(object sender, bool open)
        {
            if (open)
            {
                totalChanged = 0;
                await EditFrame.TranslateTo(0, 100, 0);
                EditFrame.IsVisible = true;
                await EditFrame.TranslateTo(0, 20);
            }
            else
            {
                await EditFrame.TranslateTo(0, 100);
                EditFrame.IsVisible = false;
            }
        }

        public void OnNewItemVisibilityChanged(object sender, bool open)
        {
            HandleNewItemVisibility();
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Running:
                    totalChanged += e.TotalY;
                    break;
                case GestureStatus.Completed:
                    PanFinished();
                    break;
            }
        }

        private void PanFinished()
        {
            if(totalChanged > 0)
            {
                (BindingContext as MainPageViewModel).CloseEditCommand.Execute();
            }

            totalChanged = 0;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            HandleNewItemVisibility();
        }

        private async void HandleNewItemVisibility()
        {
            if (!NewItemView.IsVisible)
            {
                await NewItemView.FadeTo(0, 0);
                NewItemView.IsVisible = true;
                await NewItemView.FadeTo(1);
                NewItemEntry.Focus();
            }
            else
            {
                await NewItemView.FadeTo(0);
                NewItemView.IsVisible = false;
            }
        }
    }
}
