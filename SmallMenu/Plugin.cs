using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPA.Logging;
using SiraUtil.Zenject;

namespace SmallMenu;

[Plugin(RuntimeOptions.SingleStartInit)]
public class Plugin
{
    [Init]
    public Plugin(Zenjector zenjector, Logger logger, Config config)
    {
        zenjector.Install<SmallMenuInstaller>(Location.Menu, logger, config.Generated<Settings>());
    }
}
