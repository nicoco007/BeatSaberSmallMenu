using System.Reflection;
using Zenject;
using Logger = IPA.Logging.Logger;

namespace SmallMenu;

internal class SmallMenuInstaller : Installer
{
    private readonly Logger _logger;
    private readonly Settings _settings;

    public SmallMenuInstaller(Logger logger, Settings settings)
    {
        _logger = logger;
        _settings = settings;
    }

    public override void InstallBindings()
    {
        Container.Bind<Logger>().FromInstance(_logger).When(c => c.ObjectType.Assembly == Assembly.GetExecutingAssembly());
        Container.Bind<Settings>().FromInstance(_settings);
        Container.BindInterfacesAndSelfTo<MenuScaleController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SettingsHost>().AsSingle().NonLazy();
    }
}
