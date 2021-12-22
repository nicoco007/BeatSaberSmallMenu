using IPA;
using IPA.Config;
using IPA.Config.Stores;
using IPA.Logging;
using SiraUtil.Zenject;

namespace SmallMenu
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        [Init]
        public Plugin(Zenjector zenjector, Logger logger, Config config)
        {
            zenjector.UseLogger(logger);
            zenjector.Install<SmallMenuInstaller>(Location.Menu, config.Generated<Settings>());
        }
    }
}
