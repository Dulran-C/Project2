namespace Project2
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainTabs();

        }

        //protected override Window CreateWindow(IActivationState? activationState)
        //{
         //   return new Window(new AppShell());
        //}
    }
}