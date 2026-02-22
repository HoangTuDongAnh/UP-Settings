using System.Collections.Generic;

namespace HTDA.Framework.Settings.Core
{
    public interface IValidatableSettings
    {
        IEnumerable<string> Validate();
    }
}