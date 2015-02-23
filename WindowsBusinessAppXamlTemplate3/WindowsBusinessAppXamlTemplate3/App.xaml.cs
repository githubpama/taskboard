namespace WindowsBusinessAppXamlTemplate3
{
    using BusinessAppsWinRTToolkit.Services;
    using Microsoft.Practices.Prism.PubSubEvents;
    using Microsoft.Practices.Prism.StoreApps;
    using Microsoft.Practices.Prism.StoreApps.Interfaces;
    using Microsoft.Practices.Unity;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using Windows.ApplicationModel.Activation;
    using Windows.ApplicationModel.Resources;
    using Windows.System;
    using Windows.UI.ApplicationSettings;
    using Windows.UI.Xaml;
    using WindowsBusinessAppXamlTemplate3.Views;

    sealed partial class App : MvvmAppBase
    {
        private readonly IUnityContainer container = new UnityContainer();

        public IEventAggregator EventAggregator { get; set; }

        public App()
        {
            this.InitializeComponent();
            this.RequestedTheme = ApplicationTheme.Light;
            ExtendedSplashScreenFactory = (splashscreen) => new ExtendedSplashScreen(splashscreen);
        }

        protected override async Task OnLaunchApplication(LaunchActivatedEventArgs args)
        {
            if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                await this.LoadAppResources();
            }

            NavigationService.Navigate("Main", null);
        }

        private async Task LoadAppResources()
        {
            // TODO: Load resources from different sources asynchronously
            await Task.Delay(2000);
        }

        protected override void OnInitialize(IActivatedEventArgs args)
        {
            EventAggregator = new EventAggregator();
            container.RegisterInstance(NavigationService);
            container.RegisterInstance<ISessionStateService>(SessionStateService);
            container.RegisterInstance<IEventAggregator>(EventAggregator);
            container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));

            container.RegisterType<IAlertMessageService, AlertMessageService>(new ContainerControlledLifetimeManager());

            ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "WindowsBusinessAppXamlTemplate3.UILogic.ViewModels.{0}ViewModel, WindowsBusinessAppXamlTemplate3.UILogic", viewType.Name);
                var viewModelType = Type.GetType(viewModelTypeName);
                return viewModelType;
            });
        }

        protected override IList<SettingsCommand> GetSettingsCommands()
        {
            var settingsCommands = new List<SettingsCommand>();
            var resourceLoader = container.Resolve<IResourceLoader>();

            settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), resourceLoader.GetString("OptionsFlyout/Title"), (c) => new OptionsFlyout().Show()));
            settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), resourceLoader.GetString("PrivacyPolicyText"), async (c) => await Launcher.LaunchUriAsync(new Uri(resourceLoader.GetString("PrivacyPolicyUrl")))));
            settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), resourceLoader.GetString("AboutText"), (c) => new AboutFlyout().Show()));

            return settingsCommands;
        }

        protected override object Resolve(Type type)
        {
            return container.Resolve(type);
        }
    }
}
