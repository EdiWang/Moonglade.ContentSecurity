using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonglade.ContentSecurity;

public class Payload
{
    public string OriginAspNetRequestId { get; set; }

    public string Content { get; set; }
}