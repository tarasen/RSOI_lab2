using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DesignPatterns.Model;

namespace PictureAuction.Api.ServiceModel.Types
{
    public class CustomEntity : IHasIntId
    {
        public int Id { get; set; }
    }
}
