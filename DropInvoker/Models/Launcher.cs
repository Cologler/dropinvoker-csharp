using DropInvoker.Models.Configurations;

using System;
using System.Collections.Generic;

namespace DropInvoker.Models
{
    struct Launcher
    {
        private readonly LauncherJson _json;

        public Launcher(LauncherJson json) => this._json = json;

        public IEnumerable<string> ExpandArguments(IEnumerable<string> invokerArgs)
        {
            if (this._json is null)
            {
                return VariablesHelper.ExpandArguments(invokerArgs, Array.Empty<string>());
            }
            else
            {
                return VariablesHelper.ExpandArguments(this._json.Args ?? Array.Empty<string>(), invokerArgs);
            }
        }

        public string? Application => this._json?.Application;

        public string? WorkDir => this._json?.WorkDir;
    }
}
