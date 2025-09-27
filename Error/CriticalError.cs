using System;
using System.Collections.Generic;
using System.Text;

namespace MrX.Web.Error
{
    public class CriticalError(string Message) : Exception(Message);
}
