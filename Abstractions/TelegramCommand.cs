using System;
using System.Collections.Generic;
using System.Text;

namespace Tg.Abstractions
{
    abstract class TelegramCommand
    {
        public abstract void Execute();
    }
}
